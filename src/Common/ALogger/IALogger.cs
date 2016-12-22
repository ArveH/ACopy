using System;

namespace ALogger
{
    public interface IALogger
    {
        void Write(string message);
        void Write(Exception ex);
    }
}
