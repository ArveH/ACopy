using ADatabase;

namespace ACopyLib.U4Views
{
    public interface IViewDefinition
    {
        string ViewName { get; set; }
        DbTypeName DbType { get; }
        string SelectStatement { get; set; }
    }
}
