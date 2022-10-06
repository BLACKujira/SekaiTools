using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools
{
    public static class EnvPath
    {
        public static string MasterFolder => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "SekaiTools");
        public static string AssetFolder => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "SekaiTools");

        public static string sekai_master_db_diff => Path.Combine(MasterFolder, "sekai_master_db_diff");
        public static string assets => Path.Combine(AssetFolder, "assets");
        public static string output => Path.Combine(MasterFolder, "output");
        public static string Inbuilt => Path.Combine(MasterFolder, "Inbuilt");

        public static T[] GetTable<T>(string tableName)
        {
            try
            {
                T[] table = JsonHelper.getJsonArray<T>(
                    File.ReadAllText(
                        Path.Combine(sekai_master_db_diff, $"{tableName}.json")));
                return table;
            }
            catch
            {
                throw new DataTableCorruptionException($"Êý¾Ý±íËð»µ {tableName}");
            }
        }


        [System.Serializable]
        public class DataTableCorruptionException : System.Exception
        {
            public DataTableCorruptionException() { }
            public DataTableCorruptionException(string message) : base(message) { }
            public DataTableCorruptionException(string message, System.Exception inner) : base(message, inner) { }
            protected DataTableCorruptionException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}