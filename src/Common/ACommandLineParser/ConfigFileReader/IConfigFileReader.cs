using System.Collections.Generic;
using System.Xml;

namespace ACommandLineParser.ConfigFileReader
{
    public interface IConfigFileReader
    {
        Dictionary<string, string> GetConnectionStrings(XmlDocument xmlDoc);
        string GetConversionFile(XmlDocument xmlDoc, string rdbms, string direction);
        List<IArgument> GetCommandLineParameters(XmlDocument xmlDoc);
    }
}