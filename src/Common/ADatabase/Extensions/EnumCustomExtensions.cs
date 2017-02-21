using System;

namespace ADatabase.Extensions
{
    public static class EnumCustomExtensions
    {
        public static string ConvertToString(this Enum name)
        {
            return Enum.GetName(name.GetType(), name);
        }
    }
}