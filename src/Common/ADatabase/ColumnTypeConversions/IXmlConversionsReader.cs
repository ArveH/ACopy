using System.Xml;

namespace ADatabase
{
    public interface IXmlConversionsReader
    {
        XmlNode GetRootNode(string xmlText);
        IColumnTypeDescription GetColumnTypeDescription(XmlNode rootNode);
        string GetSourceSystem(XmlNode rootNode);
        string GetDestinationSystem(XmlNode rootNode);
    }
}