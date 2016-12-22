using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ACopyLib.Utils
{
    public static class FileHelper
    {
        public static List<string> GetSchemaFiles(string folder, List<string> wildcardTableNames, string schemaFileSuffix)
        {
            List<string> filesContainingOurTables = new List<string>();
            foreach (var wildcardTableName in wildcardTableNames)
            {
                filesContainingOurTables.AddRange(Directory.GetFiles(folder, wildcardTableName.Replace('%', '*').Replace('_', '?') + "." + schemaFileSuffix));
            }

            return filesContainingOurTables;
        }

        public static byte[] GetAllBytesFromCompressedFile(string fullPath)
        {
            using (MemoryStream allBytes = new MemoryStream())
            {
                using (FileStream file = new FileStream(fullPath, FileMode.Open))
                {
                    using (DeflateStream compress = new DeflateStream(file, CompressionMode.Decompress))
                    {
                        compress.CopyTo(allBytes);
                    }
                }

                return allBytes.ToArray();
            }
        }
    }
}
