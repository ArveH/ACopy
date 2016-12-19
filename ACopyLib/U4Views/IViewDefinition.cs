using ADatabase;

namespace ACopyLib.U4Views
{
    public interface IViewDefinition
    {
        string ViewName { get; set; }
        DbType DbType { get; }
        string SelectStatement { get; set; }
    }
}
