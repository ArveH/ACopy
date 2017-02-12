using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using ADatabase.Exceptions;
using ADatabase.Extensions;
using ADatabase.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace ADatabase.Oracle
{
    public class OracleSchema: DbSchema
    {
        public OracleSchema(IDbContext dbContext) 
            : base(dbContext)
        {
        }

        // Write to file from DB
        public override ITableDefinition GetTableDefinition(IColumnTypeConverter columnTypeConverter, string tableName)
        {
            List<IColumn> columns = new List<IColumn>();
            GetColumnInfo(columnTypeConverter, tableName, columns);

            ITableDefinition tableDefinition = DbContext.PowerPlant.CreateTableDefinition(tableName, columns, GetSegmentName(tableName));
            tableDefinition.HasBlobColumn = (from col in tableDefinition.Columns
                                               where col.Type == ColumnTypeName.Blob
                                               select col).Any();

            return tableDefinition;
        }

        private void GetColumnInfo(IColumnTypeConverter columnTypeConverter, string tableName, List<IColumn> columns)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            string selectStmt = CreateSelectStatementForColumns(tableName);
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    int length = reader.GetInt16(2);
                    int prec = reader.GetInt16(3);
                    int scale = reader.GetInt32(4);
                    bool isNullable = reader.GetString(5) == "Y";
                    string def = reader.IsDBNull(6) ? "" : reader.GetString(6).TrimEnd();
                    string sourceType = type.AddParameters();
                    var colType = columnTypeConverter.GetDestinationType(sourceType, ref length, ref prec, ref scale).ColumnTypeName();
                    columns.Add(columnFactory.CreateInstance(colType, name, length, prec, scale, isNullable, def, ""));
                }
            }
            finally
            {
                cursor.Close();
            }
        }

        private static string CreateSelectStatementForColumns(string tableName)
        {
            string selectStmt = "";
            selectStmt += "SELECT column_name, " + "\n";
            selectStmt += "       data_type, " + "\n";
            selectStmt += "       nvl(Decode(char_length, 0, data_length, " + "\n";
            selectStmt += "                           char_length), 0) AS col3, " + "\n";
            selectStmt += "       nvl(data_precision, 0) as col4, " + "\n";
            selectStmt += "       nvl(data_scale, 0) as col5, " + "\n";
            selectStmt += "       nullable, " + "\n";
            selectStmt += "       data_default as col6 " + "\n";
            selectStmt += "FROM   user_tab_columns " + "\n";
            selectStmt += "WHERE  table_name = '" + tableName.ToUpper() + "' \n";
            selectStmt += "ORDER BY column_id " + "\n";
            return selectStmt;
        }

        private string GetSegmentName(string tableName)
        {
            string selectStmt = string.Format("SELECT tablespace_name from user_tables where table_name = '{0}'", tableName.ToUpper());
            return (string)Commands.ExecuteScalar(selectStmt);
        }

        public override bool IsTable(string tableName)
        {
            bool tableExists;
            var connection = new InternalOracleConnection(DbContext.ConnectionString);
            try
            {
	            using (InternalOracleCommand command = new InternalOracleCommand("SELECT table_name FROM user_tables WHERE table_name = :tableName ", connection))
	            {
	                command.Command.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));
	                tableExists = command.Command.ExecuteScalar() != null;
	            }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR with statement in IsTable", ex);
            }
            finally
            {
                connection.Dispose();
            }

            return tableExists;
        }

        public override void CreateTable(ITableDefinition tableDefinition)
        {
            StringBuilder createStmt = new StringBuilder(string.Format("create table {0} (", tableDefinition.Name));
            for (int i = 0; i < tableDefinition.Columns.Count; i++)
            {
                if (i > 0)
                {
                    createStmt.Append(", ");
                }
                createStmt.Append(tableDefinition.Columns[i].Name);
                createStmt.Append(" ");
                createStmt.Append(tableDefinition.Columns[i].GetColumnDefinition());
            }
            createStmt.Append(")");

            Commands.ExecuteNonQuery(createStmt.ToString());
        }

        public override List<ITableShortInfo> GetTableNames(string searchString)
        {
            string selectStmt = GetStatementForSelectTableNames(searchString);

            List<ITableShortInfo> tables = new List<ITableShortInfo>();
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
	            while (reader.Read())
	            {
                    tables.Add(TableShortInfoFactory.CreateInstance(reader.GetString(0), GetRowCount(reader.GetString(0))));
	            }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR with statement in GetTableNames", ex);
            }
            finally
            {
                cursor.Close();
            }

            return tables;
        }

        private static string GetStatementForSelectTableNames(string searchString)
        {
            string selectStmt = "select table_name from user_tables where ";
            bool firstTable = true;
            foreach (var tab in searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (firstTable)
                {
                    firstTable = false;
                }
                else
                {
                    selectStmt += "and ";
                }
                selectStmt += "table_name like '" + tab.ToUpper() + "'";
            }
            return selectStmt;
        }

        public override bool IsIndex(string indexName, string tableName)
        {
            string selectStmt = "";
            selectStmt += "SELECT count(*) as indexCount ";
            selectStmt += "FROM user_indexes i ";
            selectStmt += string.Format("WHERE i.table_name = upper('{0}') ", tableName);
            selectStmt += string.Format("AND i.index_name = upper('{0}') ", indexName);
            var tmp = Commands.ExecuteScalar(selectStmt);
            var indexCount = Convert.ToInt32(tmp);
            return indexCount != 0;
        }

        protected override List<IIndexColumn> GetIndexColumnsForIndex(IIndexDefinition index)
        {
            string selectStmt = "";
            selectStmt += "SELECT ic.column_name, " + "\n";
            selectStmt += "       Nvl(iexpr.column_position, -1) is_expression, " + "\n";
            selectStmt += "       iexpr.column_expression " + "\n";
            selectStmt += "FROM   user_ind_columns ic " + "\n";
            selectStmt += "       LEFT OUTER JOIN user_ind_expressions iexpr " + "\n";
            selectStmt += "                    ON ( ic.index_name = iexpr.index_name " + "\n";
            selectStmt += "                         AND ic.column_position = iexpr.column_position ) " + "\n";
            selectStmt += string.Format("WHERE  ic.index_name = Upper('{0}') ", index.IndexName);
            selectStmt += "ORDER BY ic.column_position " + "\n";

            List<IIndexColumn> columns = new List<IIndexColumn>();
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    bool isExpression = reader.GetInt32(1) != -1;
                    string expression = reader.IsDBNull(2) ? " " : reader.GetString(2);
                    columns.Add(IndexColumnFactory.CreateInstance(name, expression));
                }
            }
            finally
            {
                cursor.Close();
            }

            return columns;
        }

        protected override List<IIndexDefinition> GetIndexesForTable(string tableName)
        {
            List<IIndexDefinition> indexes = new List<IIndexDefinition>();
            string selectStmt = "";
            selectStmt = selectStmt + "SELECT i.index_name, " + "\n";
            selectStmt = selectStmt + "       i.tablespace_name, " + "\n";
            selectStmt = selectStmt + "       i.uniqueness " + "\n";
            selectStmt = selectStmt + "FROM   user_indexes i " + "\n";
            selectStmt = selectStmt + string.Format("WHERE  i.table_name = Upper('{0}') ", tableName) + "\n";
            selectStmt = selectStmt + "AND    i.index_type != 'LOB'" + "\n";
            selectStmt = selectStmt + "ORDER BY i.index_name " + "\n";
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string indexName = reader.GetString(0);
                    string location = reader.GetString(1);
                    bool isUnique = reader.GetString(2) == "UNIQUE";
                    indexes.Add(DbContext.PowerPlant.CreateIndexDefinition(indexName, tableName, location, isUnique));
                }
            }
            finally
            {
                cursor.Close();
            }

            return indexes;
        }

        public override string GetLocationAsSql(string location)
        {
            return "TABLESPACE " + location;
        }

        public override bool IsView(string viewName)
        {
            bool tableExists;
            try
            {
                var tmp = Commands.ExecuteScalar(string.Format("SELECT view_name FROM user_views WHERE view_name = upper('{0}') ", viewName));
	            tableExists = tmp != null;
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR with statement in IsView", ex);
            }

            return tableExists;
        }

        public override bool CanConnect(CancellationToken token)
        {
            bool canConnect;

            InternalOracleConnection connection = null;
            try
            {
                connection = new InternalOracleConnection(DbContext.ConnectionString);
                canConnect = true;
            }
            catch
            {
                canConnect = false;
            }
            finally
            {
                if (connection != null) connection.Dispose();
            }

            return canConnect;
        }
    }
}
