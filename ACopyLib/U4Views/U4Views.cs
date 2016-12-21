using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ADatabase;
using ALogger;
using AParser;
using DbType = ADatabase.DbType;

namespace ACopyLib.U4Views
{
    internal class U4Views: IU4Views
    {
        private readonly IDbContext _dbContext;

        public U4Views(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private string _aagTableName = "aagview";
        public string AagTableName
        {
            get { return _aagTableName; }
            set { _aagTableName = value; }
        }

        private string _asysTableName = "asysview";
        public string AsysTableName
        {
            get { return _asysTableName; }
            set { _asysTableName = value; }
        }

        private string GetDbTypeName()
        {
            return _dbContext.DbType == DbType.Oracle ? "oracle" : "mssql";
        }

        public void DoViews(out int totalViews, out int failedViews, IALogger tmpLogger = null)
        {
            IALogger logger = tmpLogger ?? new TestLogger();
            totalViews = 0;
            failedViews = 0;

            List<IViewDefinition> viewDefinitions = new List<IViewDefinition>();
            viewDefinitions.AddRange(GetViewsFromAagView());
            viewDefinitions.AddRange(GetViewsFromAsysViewThatAreNotInAagView());

            ICommands cmd = _dbContext.PowerPlant.CreateCommands();
            IDbSchema schema = _dbContext.PowerPlant.CreateDbSchema();
            IASTNodeFactory nodeFactory = new ASTNodeFactory();
            IAParser parser = AParserFactory.CreateInstance(nodeFactory);
            if (_dbContext.DbType == DbType.Oracle)
            {
                parser.ExpandEmptyStrings = true;
            }
            IATranslator translator = ATranslatorFactory.CreateInstance(_dbContext.DbType, nodeFactory);
            foreach (var view in viewDefinitions)
            {
                if (view.DbType != DbType.Any && view.DbType != _dbContext.DbType)
                {
                    continue;
                }

                totalViews++;
                try
                {
                    schema.DropView(view.ViewName);
                    CreateView(cmd, parser, translator, view);
	                logger.Write(string.Format("View '{0}' created", view.ViewName));
                }
                catch (Exception ex)
                {
                    logger.Write(String.Format("ERROR: Can't create view '{0}'", view.ViewName));
                    logger.Write(ex.Message);
                    failedViews++;
                }
            }
        }

        private void CreateView(ICommands cmd, IAParser parser, IATranslator translator, IViewDefinition view)
        {
            ASTNodeList aNodes = null;
            StringBuilder createViewStmt = new StringBuilder("create view ");
            createViewStmt.Append(view.ViewName);

            if (view.DbType == DbType.Any || parser.ExpandEmptyStrings)
            {
                aNodes = parser.CreateNodeList(view.SelectStatement);
            }

            if (view.DbType == DbType.Any)
            {
                if (!ColumnListContainStar(aNodes))
                {
                    createViewStmt.Append(CreateColumnList(aNodes));
                }

                aNodes = translator.Translate(aNodes);
            }

            createViewStmt.Append(" as ");

            if (aNodes != null)
            {
                createViewStmt.Append(aNodes);
            }
            else
            {
                createViewStmt.Append(view.SelectStatement);
            }

            cmd.ExecuteNonQuery(createViewStmt.ToString());
        }

        private bool ColumnListContainStar(ASTNodeList aNodes)
        {
            for (int i = 0; i < aNodes.Count && !aNodes[i].TextEqualTo("from"); i++)
            {
                if (aNodes[i].Text.Contains('*'))
                {
                    return true;
                }
            }

            return false;
        }

        private string CreateColumnList(ASTNodeList aNodes)
        {
            StringBuilder columnList = new StringBuilder();
            columnList.Append(" (");
            int nodeCounter = 0;
            IASTNode currentNode = null;
            while (nodeCounter < aNodes.Count)
            {
                var previousNode = currentNode;
                currentNode = aNodes[nodeCounter++];
                if (currentNode.Text[0] == ',' || currentNode.TextEqualTo("from"))
                {
                    columnList.Append(previousNode.Text.Trim('"', '[', ']'));
                    if (currentNode.TextEqualTo("from"))
                    {
                        break;
                    }
                    columnList.Append(", ");
                }
            }
            columnList.Append(")");

            return columnList.ToString();
        }

        private List<IViewDefinition> ReadViewDefinitionsFromDatabase(string selectStatement)
        {
            List<IViewDefinition> viewDefinitions = new List<IViewDefinition>();

            IDataCursor cursor = null;
            try
            {
                cursor = _dbContext.PowerPlant.CreateDataCursor();
                IDataReader reader = cursor.ExecuteReader(selectStatement);
                while (reader.Read())
                {
                    string database = reader.GetString(0);
                    string viewName = reader.GetString(1);
                    string select = reader.GetString(2);
                    viewDefinitions.Add(ViewDefinitionFactory.CreateInstance(ViewDefinition.ConvertFromStringToDbType(database), viewName, select));
                }
            }
            finally
            {
                if (cursor != null) cursor.Close();
            }

            return viewDefinitions;
        }

        private List<IViewDefinition> GetViewsFromAagView()
        {
            string selectStatement = "";
            selectStatement += string.Format("SELECT '{0}' as db_name, table_name, query ", GetDbTypeName());
            selectStatement += string.Format("  FROM {0} ", AagTableName);
            selectStatement += " WHERE status = 'N' ";
            selectStatement += "ORDER BY priority desc, table_name";

            return ReadViewDefinitionsFromDatabase(selectStatement);
        }

        private List<IViewDefinition> GetViewsFromAsysViewThatAreNotInAagView()
        {
            string selectStatement = "";
            selectStatement += "SELECT db_name, table_name, query ";
            selectStatement += string.Format("  FROM {0} ", AsysTableName);
            selectStatement += string.Format(" WHERE ( db_name = ' ' OR db_name = '{0}' ) ", GetDbTypeName());
            selectStatement += "   AND status = 'N' ";
            selectStatement += "   AND table_name not in ";
            selectStatement += string.Format("(SELECT table_name FROM {0} WHERE status = 'N') ", AagTableName);
            selectStatement += "ORDER BY priority desc, table_name";

            return ReadViewDefinitionsFromDatabase(selectStatement);
        }

        public bool HasViewsSource
        {
            get 
            {
                var schema = _dbContext.PowerPlant.CreateDbSchema();
                return schema.IsTable(AagTableName) && schema.IsTable(AsysTableName);
            }
        }
    }
}
