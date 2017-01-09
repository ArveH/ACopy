using System.Runtime.InteropServices;

namespace ADatabase
{
    public class ColumnTypeConverter : IColumnTypeConverter
    {
        private string _conversionXml;

        public void Initialize(string conversionXml)
        {
            _conversionXml = conversionXml;
        }
    }
}