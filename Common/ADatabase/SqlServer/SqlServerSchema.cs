using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using ADatabase.Exceptions;
using ADatabase.Interfaces;
using ADatabase.SqlServer.Columns;

namespace ADatabase.SqlServer
{
    public class SqlServerSchema : DbSchema
    {
        public SqlServerSchema(IDbContext dbContext)
            : base(dbContext)
        {
        }

        public override List<ITableShortInfo> GetTableNames(string searchString)
        {
            string selectStmt = GetStatementForSelectTableNames(searchString);

            List<ITableShortInfo> tables = new List<ITableShortInfo>();
            InternalSqlServerConnection connection = null;
            try
            {
                connection = new InternalSqlServerConnection(DbContext.ConnectionString);
                using (InternalSqlServerCommand command = new InternalSqlServerCommand(selectStmt, connection))
                {
                    SqlDataReader reader = command.Command.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        tables.Add(TableShortInfoFactory.CreateInstance(reader.GetString(0), GetRowCount(reader.GetString(0))));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR when getting table names: " + selectStmt, ex);
            }
            finally
            {
                if (connection != null) connection.Close();
            }

            return tables;
        }

        private static string GetStatementForSelectTableNames(string searchString)
        {
            //string selectStmt = "select name from sys.objects where type = 'U' ";
            //foreach (var tab in searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    selectStmt += "and name like '" + tab + "'";
            //}
            //return selectStmt;

            return searchString
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(
                    "select name from sys.objects where type = 'U' ", 
                    (current, tab) => current + ("and name like '" + tab + "'"));
        }

        private string GetSegmentName(string tableName)
        {
            string selectStmt = "";
            selectStmt += "SELECT DISTINCT s.name AS segname " + "\n";
            selectStmt += "FROM   sys.objects t, " + "\n";
            selectStmt += "       sys.indexes i, " + "\n";
            selectStmt += "       sys.filegroups s " + "\n";
            selectStmt += "WHERE  t.type = 'U' " + "\n";
            selectStmt += "       AND t.object_id = i.object_id " + "\n";
            selectStmt += "       AND ( i.index_id = 0 " + "\n";
            selectStmt += "              OR i.index_id = 1 ) " + "\n";
            selectStmt += "       AND i.data_space_id = s.data_space_id " + "\n";
            selectStmt += string.Format("       AND t.name = '{0}' ", tableName);

            return (string)Commands.ExecuteScalar(selectStmt);
        }

        public override ITableDefinition GetTableDefinition(string tableName)
        {
            string selectStmt = "";
            selectStmt += "SELECT c.name, " + "\n";
            selectStmt += "       t.name AS t_name, " + "\n";
            selectStmt += "       isnull(c.max_length, 0) as length, " + "\n";
            selectStmt += "       isnull(c.precision, 0) as prec, " + "\n";
            selectStmt += "       isnull(c.scale, 0) as scale, " + "\n";
            selectStmt += "       c.is_nullable, " + "\n";
            selectStmt += "       convert(varchar(256), isnull(c.collation_name, '')) as collation, " + "\n";
            selectStmt += "       isnull(object_definition(c.default_object_id), '') as def, " + "\n";
            selectStmt += "       c.is_identity " + "\n";
            selectStmt += "FROM   sys.columns c " + "\n";
            selectStmt += "       JOIN sys.types t " + "\n";
            selectStmt += "         ON c.user_type_id = t.user_type_id " + "\n";
            selectStmt += String.Format("WHERE  c.object_id = Object_id('{0}') ", tableName) + "\n";
            selectStmt += "ORDER  BY c.column_id ";

            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>();
            bool tableHasRawColumn = false;
            IDataCursor cursor = null;
            try
            {
                cursor = DbContext.PowerPlant.CreateDataCursor();
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    int length = reader.GetInt16(2);
                    int prec = reader.GetByte(3);
                    int scale = reader.GetByte(4);
                    bool isNullable = reader.GetBoolean(5);
                    string collation = reader.GetString(6);
                    string def = reader.GetString(7);
                    ColumnType colType = SqlServerColumnTypeConverter.GetColumnTypeFromNativeType(type, ref length, prec, scale);
                    if (reader.GetBoolean(8))
                    {
                        colType = ColumnType.Identity;
                    }
                    if (colType == ColumnType.Raw)
                    {
                        tableHasRawColumn = true;
                    }
                    columns.Add(
                        columnFactory.CreateInstance(
                            colType,
                            name,
                            length,
                            prec,
                            scale,
                            isNullable,
                            def,
                            collation)
                            );
                }
            }
            catch (Exception ex)
            {
                throw new ADatabaseException("ERROR when Get TableDefinition: " + selectStmt, ex);
            }
            finally
            {
                if (cursor != null) cursor.Close();
            }

            ITableDefinition tableDefinition = DbContext.PowerPlant.CreateTableDefinition(tableName, columns, GetSegmentName(tableName));
            tableDefinition.HasRawColumn = tableHasRawColumn;

            tableDefinition.Columns.RemoveAll(c => c.Name == "agrtid");

            return tableDefinition;
        }

        public override bool IsTable(string tableName)
        {
            return null != Throttle.Execute(
                DbContext,
                "SELECT name FROM sys.objects WHERE type = 'U' AND name = @objectName ",
                cmd => 
                    {
                        cmd.Command.Parameters.AddWithValue("objectName", tableName);
                        return cmd.Command.ExecuteScalar();
                    }
                );
        }

        public override bool IsView(string viewName)
        {
            return null != Throttle.Execute(
                DbContext,
                "SELECT name FROM sys.objects WHERE type = 'V' AND name = @objectName ",
                cmd =>
                    {
                        cmd.Command.Parameters.AddWithValue("objectName", viewName);
                        return cmd.Command.ExecuteScalar();
                    }
                );
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
            createStmt.Append(AddIdentityColumn(tableDefinition));
            createStmt.Append(")");

            Commands.ExecuteNonQuery(createStmt.ToString());
        }

        private string AddIdentityColumn(ITableDefinition tableDefinition)
        {
            if (!tableDefinition.Columns.Exists(c => c.Type == ColumnType.Identity))
            {
                IColumn col = DbContext.PowerPlant.CreateColumnFactory().CreateInstance(ColumnType.Identity, "agrtid", false, "");
                return ", agrtid " + col.GetColumnDefinition();
            }

            return "";
        }

        public override bool IsIndex(string indexName, string tableName)
        {
            string selectStmt = "";
            selectStmt += "SELECT count(*) as indexCount ";
            selectStmt += "FROM sys.indexes i ";
            selectStmt += "WHERE i.index_id BETWEEN 1 AND 254 ";
            selectStmt += "AND i.is_primary_key = 0 ";
            selectStmt += "AND i.is_unique_constraint = 0 ";
            selectStmt += "AND i.is_hypothetical = 0 ";
            selectStmt += string.Format("AND i.object_id = object_id('{0}') ", tableName);
            selectStmt += string.Format("AND i.name = '{0}' ", indexName);
            var tmp = Commands.ExecuteScalar(selectStmt);
            var indexCount = Convert.ToInt32(tmp);
            return indexCount != 0;
        }

        protected override List<IIndexColumn> GetIndexColumnsForIndex(IIndexDefinition index)
        {
            string selectStmt = "";
            selectStmt += "SELECT tc.name as col_name " + "\n";
            selectStmt += "FROM   sys.index_columns ic, " + "\n";
            selectStmt += "       sys.columns tc " + "\n";
            selectStmt += "WHERE  ic.column_id = tc.column_id " + "\n";
            selectStmt += "       AND ic.object_id = tc.object_id " + "\n";
            selectStmt += "       AND is_included_column = 0 " + "\n";
            selectStmt += String.Format("       AND index_id = {0} ", index.IndexId) + "\n";
            selectStmt += String.Format("       AND ic.object_id = Object_id('{0}') ", index.TableName) + "\n";
            selectStmt += "       AND is_included_column = 0 ";

            List<IIndexColumn> columns = new List<IIndexColumn>();
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    columns.Add(IndexColumnFactory.CreateInstance(name));
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
            String selectStmt = "";
            selectStmt += "SELECT DISTINCT i.name, " + "\n";
            selectStmt += "                s.name      loc_name, " + "\n";
            selectStmt += "                i.index_id  indid, " + "\n";
            selectStmt += "                i.is_unique unique_flag, " + "\n";
            selectStmt += "                i.type_desc " + "\n";
            selectStmt += "FROM   sys.indexes i, " + "\n";
            selectStmt += "       sys.filegroups s " + "\n";
            selectStmt += "WHERE  i.index_id BETWEEN 1 AND 254 " + "\n";
            selectStmt += "       AND i.data_space_id = s.data_space_id " + "\n";
            selectStmt += "       AND i.is_primary_key = 0 " + "\n";
            selectStmt += "       AND i.is_unique_constraint = 0 " + "\n";
            selectStmt += "       AND i.is_hypothetical = 0 " + "\n";
            selectStmt += string.Format("       AND i.object_id = Object_id('{0}') ", tableName) + "\n";
            selectStmt += "ORDER  BY i.name ";
            List<IIndexDefinition> indexes = new List<IIndexDefinition>();
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string location = reader.GetString(1);
                    int id = reader.GetInt32(2);
                    bool isUnique = reader.GetBoolean(3);
                    bool isClustered = reader.GetString(4) == "CLUSTERED";
                    indexes.Add(DbContext.PowerPlant.CreateIndexDefinition(name, tableName, location, isUnique, id, isClustered));
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
            return string.Format("ON '{0}' ", location);
        }

        public override string GetCollation()
        {
            return (string)Commands.ExecuteScalar("SELECT CONVERT (varchar, DATABASEPROPERTYEX(DB_NAME(),'collation'))");
        }

        public override bool CanConnect(CancellationToken token)
        {
            var canConnect = false;

            using (var connection = new SqlConnection(DbContext.ConnectionString))
            {
                connection.OpenAsync(token).Wait(5000, token);
                if (connection.State == ConnectionState.Open)
                {
                    canConnect = true;
                }
                else
                {
                    connection.Close();
                }
            }

            return canConnect;
        }
    }
}
