using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using ACopyLib.Exceptions;
using ADatabase;
using ADatabase.Extensions;
using ADatabase.Interfaces;

namespace ACopyLib.Xml
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class AXmlReader : IAXmlReader
    {
        private readonly IDbContext _dbContext;

        public AXmlReader(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ITableDefinition ReadSchema(IColumnTypeConverter columnsTypeConverter, string schemaXml)
        {
            ITableDefinition tableDefinition = _dbContext.PowerPlant.CreateTableDefinition();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(schemaXml);
            ReadTableInfo(tableDefinition, xmlDocument);
            ReadColumns(columnsTypeConverter, tableDefinition, xmlDocument);
            ReadIndexes(tableDefinition, xmlDocument);

            return tableDefinition;
        }

        private void ReadIndexes(ITableDefinition tableDefinition, XmlDocument xmlDocument)
        {
            var indexesNode = xmlDocument.SelectSingleNode("/Table/Indexes");
            if (indexesNode == null)
            {
                return;
            }

            tableDefinition.Indexes = new List<IIndexDefinition>();
            foreach (XmlNode index in indexesNode.ChildNodes)
            {
                ReadIndexInfo(tableDefinition, index);
            }
        }

        private void ReadIndexInfo(ITableDefinition tableDefinition, XmlNode index)
        {
            Debug.Assert(index != null, "index != null");
            string indexName = index["IndexName"].InnerText;
            string location = index["Location"].InnerText;
            bool isUnique = Convert.ToBoolean(index["IsUnique"].InnerText);
            string db = index["DbSpecific"].InnerText;
            IIndexDefinition indexDefinition = _dbContext.PowerPlant.CreateIndexDefinition(indexName, tableDefinition.Name, location, isUnique);
            indexDefinition.DbSpecific = TableDefinition.ConvertStringToDbType(db);

            indexDefinition.Columns = new List<IIndexColumn>();
            foreach (XmlNode indexCol in index["IndexColumns"].ChildNodes)
            {
                indexDefinition.Columns.Add(IndexColumnFactory.CreateInstance(indexCol.Name, indexCol.InnerText));
            }
            tableDefinition.Indexes.Add(indexDefinition);
        }

        private static void ReadTableInfo(ITableDefinition tableDefinition, XmlDocument xmlDocument)
        {
            var rootNode = xmlDocument.DocumentElement.SelectSingleNode("/Table");
            tableDefinition.Name = rootNode.Attributes["Name"].InnerText;
            tableDefinition.Location = rootNode.Attributes["Location"].InnerText;
        }

        private void ReadColumns(IColumnTypeConverter columnsTypeConverter, ITableDefinition tableDefinition, XmlDocument xmlDocument)
        {
            var rootNode = xmlDocument.DocumentElement.SelectSingleNode("/Table");
            IColumnFactory columnFactory = _dbContext.PowerPlant.CreateColumnFactory();
            foreach (XmlNode col in rootNode.ChildNodes[0].ChildNodes)
            {
                ReadColumn(columnFactory, columnsTypeConverter, tableDefinition.Columns, col);
            }
        }

        private static void ReadColumn(IColumnFactory columnFactory, IColumnTypeConverter columnsTypeConverter, List<IColumn> columns, XmlNode col)
        {
            string colName = col.Attributes["Name"].InnerText;
            string type = col["Type"].InnerText.ToLower();
            bool isNullable = Convert.ToBoolean(col["IsNullable"].InnerText);
            string def = col["Default"].InnerText;

            Dictionary<string, object> colDetails = ReadColumnDetails(col);
            var length = colDetails.ContainsKey("Length") ? Convert.ToInt32(colDetails["Length"]) : 0;
            var prec = colDetails.ContainsKey("Prec") ? Convert.ToInt32(colDetails["Prec"]) : 0;
            var scale = colDetails.ContainsKey("Scale") ? Convert.ToInt32(colDetails["Scale"]) : 0;
            var collation = colDetails.ContainsKey("Collation") ? (string)colDetails["Collation"] : "";
            bool isIdentity = colDetails.ContainsKey("IsIdentity") ? Convert.ToBoolean(col["IsIdentity"]) : false;
            var destinationType = columnsTypeConverter.GetDestinationType(type, ref length, ref prec, ref scale).ColumnTypeName();
            columns.Add(columnFactory.CreateInstance(destinationType, colName, length, prec, scale, isNullable, isIdentity, def, collation));
        }

        private static Dictionary<string, object> ReadColumnDetails(XmlNode col)
        {
            Dictionary<string, object> colDetails = new Dictionary<string, object>();
            if (col["Details"] != null)
            {
                for (var detail = col["Details"].FirstChild; detail != null; detail = detail.NextSibling)
                {
                    colDetails.Add(detail.Name, detail.InnerText);
                }
            }

            return colDetails;
        }
    }
}