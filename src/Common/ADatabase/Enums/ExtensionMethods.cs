using System;

namespace ADatabase
{
    public static class ExtensionMethods
    {
        public static string ConvertToString(this Enum name)
        {
            return Enum.GetName(name.GetType(), name);
        }

        public static TEnumType ConverToEnum<TEnumType>(this string enumValue)
        {
            return (TEnumType)Enum.Parse(typeof(TEnumType), enumValue);
        }
    }
}