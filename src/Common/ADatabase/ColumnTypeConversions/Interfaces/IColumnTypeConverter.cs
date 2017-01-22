namespace ADatabase
{
    public interface IColumnTypeConverter
    {
        void Initialize(string conversionXml);
        string GetDestinationType(string nativeType, int? length, int? prec, int? scale);
    }
}