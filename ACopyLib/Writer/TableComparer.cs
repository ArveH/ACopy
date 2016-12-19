using System.Collections.Generic;
using ADatabase.Interfaces;

namespace ACopyLib.Writer
{
    public class TableComparer : IComparer<ITableShortInfo>
    {
        public TableComparer()
        {
        }

        // This will sort in descending order
        public int Compare(ITableShortInfo x, ITableShortInfo y)
        {
            if (x.RowCount < y.RowCount)
            {
                return 1;
            }
            if (x.RowCount > y.RowCount)
            {
                return -1;
            }
            return 0;
        }
    }
}
