using System.Xml;

namespace ADatabase
{
    public interface IXmlConversionsReader
    {
        XmlNode GetRootNode(string xmlText);
        ITypeDescription GetColumnTypeDescription(XmlNode rootNode);
        string GetRdbms(XmlNode rootNode);
        string GetDirection(XmlNode rootNode);
    }
}