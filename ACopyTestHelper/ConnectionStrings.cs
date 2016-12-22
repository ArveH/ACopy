using System.Collections.Generic;
using System.IO;
using ADatabase.Exceptions;
using Newtonsoft.Json.Linq;

namespace ACopyTestHelper
{
    public class ConnectionStrings
    {
        private Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();

        public ConnectionStrings(string settingsFileFullPath)
        {
            var settingsFileText = File.ReadAllText(settingsFileFullPath);
            var json = JObject.Parse(settingsFileText);
            var connectionstringsJson = (JArray)json["connectionStrings"];
            foreach (var connectionString in connectionstringsJson)
            {
                JProperty conn = connectionString.First.Value<JProperty>();
                _connectionStrings.Add(conn.Name, conn.Value.ToString());
            }
        }

        public string GetConnectionString(string key)
        {
            if (!_connectionStrings.ContainsKey(key)) throw new ADatabaseException($"Can't find connection string for '{key}'");

            return _connectionStrings[key];
        }

        public string GetSqlServer()
        {
            return GetConnectionString("mss_test");
        }

        public string GetOracle()
        {
            return GetConnectionString("ora_test");
        }
    }
}
