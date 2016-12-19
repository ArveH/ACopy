using ALogger;

namespace ACopyLib.U4Views
{
    public interface IU4Views
    {
        string AagTableName { get; set; }
        string AsysTableName { get; set; }
        bool HasViewsSource { get; }
        void DoViews(out int totalViews, out int failedViews, IALogger logger=null);
    }
}
