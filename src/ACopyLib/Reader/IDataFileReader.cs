using System;
using System.Collections.Generic;
using ADatabase;

namespace ACopyLib.Reader
{
    public interface IDataFileReader: IDisposable
    {
        List<string> ReadLine(List<IColumn> columns);
        bool IsEndOfFile { get; }
        long RowCounter { get; }
        string FileName { get; }
    }
}