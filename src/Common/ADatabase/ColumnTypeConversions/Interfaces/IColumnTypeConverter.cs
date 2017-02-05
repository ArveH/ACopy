namespace ADatabase
{
    public interface IColumnTypeConverter
    {
        void Initialize(string conversionXml);
        string GetDestinationType(string sourceType, ref int length, ref int prec, ref int scale);
    }
}