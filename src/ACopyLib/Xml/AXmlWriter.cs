using System;
using System.Collections.Generic;
using System.Xml;
using ADatabase;
using ADatabase.Interfaces;

namespace ACopyLib.Xml
{
    public class AXmlWriter : IAXmlWriter
    {
        public void WriteSchema(ITableDefinition tableDefinition, string fullPath)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            XmlWriter writer = XmlWriter.Create(fullPath, settings);
            try
            {
	            writer.WriteStartDocument();
	            writer.WriteComment(string.Format("Table definition file for U4Copy program"));
	
	            BodyToXmlFile(tableDefinition, writer);
	            writer.WriteEndDocument();
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }

        }

        private static void BodyToXmlFile(ITableDefinition tableDefinition, XmlWriter writer)
        {
            writer.WriteStartElement("Table");
            writer.WriteAttributeString("Name", tableDefinition.Name);
            writer.WriteAttributeString("Location", tableDefinition.Location);

            ColumnsToXmlFile(tableDefinition.Columns, writer);
            IndexesToXmlFile(tableDefinition.Indexes, writer);

            writer.WriteEndElement();
        }

        private static void ColumnsToXmlFile(List<IColumn> columns, XmlWriter writer)
        {
            writer.WriteStartElement("Columns");
            foreach (var col in columns)
            {
                ColumnToXmlFile(col, writer);
            }

            writer.WriteEndElement();
        }

        private static string ParseDefaultValue(string nativeDefaultValue)
        {
            if ((nativeDefaultValue.IndexOf("1900", StringComparison.Ordinal) > 1 && nativeDefaultValue.IndexOf("01", StringComparison.Ordinal) > 0))
            {
                return "MIN_DATE";
            }
            if (nativeDefaultValue.IndexOf("2099", StringComparison.Ordinal) > 1 && nativeDefaultValue.IndexOf("31", StringComparison.Ordinal) > 0)
            {
                return ("MAX_DATE");
            }
            return nativeDefaultValue;
        }

        private static void ColumnToXmlFile(IColumn column, XmlWriter writer)
        {
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("Name", column.Name);
            writer.WriteElementString("Type", column.Type.ToString());
            writer.WriteElementString("IsNullable", column.IsNullable.ToString());
            writer.WriteElementString("Default", ParseDefaultValue(column.Default));
            ColumnDetailsToXmlFile(column.Details, writer);
            writer.WriteEndElement();
        }

        private static void ColumnDetailsToXmlFile(Dictionary<string, object> details, XmlWriter writer)
        {
            if (details.Count > 0)
            {
                writer.WriteStartElement("Details");
                foreach (var detail in details)
                {
                    writer.WriteElementString(detail.Key, detail.Value.ToString());
                }
                writer.WriteEndElement();
            }
        }

        private static void IndexesToXmlFile(List<IIndexDefinition> indexes, XmlWriter writer)
        {
            if (indexes == null || indexes.Count == 0)
            {
                return;
            }

            writer.WriteStartElement("Indexes");
            foreach (var index in indexes)
            {
                IndexToXmlFile(index, writer);
            }

            writer.WriteEndElement();
        }

        private static void IndexToXmlFile(IIndexDefinition index, XmlWriter writer)
        {
            writer.WriteStartElement("Index");
            writer.WriteElementString("IndexName", index.IndexName);
            writer.WriteElementString("Location", index.Location);
            writer.WriteElementString("IsUnique", index.IsUnique.ToString());
            writer.WriteElementString("DbSpecific", index.DbSpecific.ToString());
            IndexColumnsToXmlFile(index.Columns, writer);
            writer.WriteEndElement();
        }

        private static void IndexColumnsToXmlFile(List<IIndexColumn> indexColumns, XmlWriter writer)
        {
            writer.WriteStartElement("IndexColumns");
            foreach (var col in indexColumns)
            {
                writer.WriteElementString(col.Name, col.IsExpression ? col.Expression : "");
            }

            writer.WriteEndElement();
        }

    }
}