namespace ADatabase
{
    public interface IColumnTypeConverter
    {
        void Initialize(string conversionXml);
        string GetDestinationType(string sourceType, int? length, int? prec, int? scale);
    }
}