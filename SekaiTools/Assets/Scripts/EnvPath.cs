using SekaiTools.SekaiViewerInterface;
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
        public static string sekai_master_db_tc_diff => Path.Combine(MasterFolder, "sekai_master_db_tc_diff");
        public static string sekai_master_db_cn_diff => Path.Combine(MasterFolder, "sekai_master_db_cn_diff");
        public static string sekai_master_db_en_diff => Path.Combine(MasterFolder, "sekai_master_db_en_diff");
        public static string sekai_master_db_kr_diff => Path.Combine(MasterFolder, "sekai_master_db_kr_diff");
        public static Sekai_master_db_diff Sekai_master_db_diff => new Sekai_master_db_diff();

        public static string Assets => Path.Combine(AssetFolder, "assets");
        public static string Output => Path.Combine(MasterFolder, "output");
        public static string Inbuilt => Path.Combine(MasterFolder, "Inbuilt");

        public static T[] GetTable<T>(string tableName,ServerRegion sr = ServerRegion.jp)
        {
            try
            {
                T[] table = JsonHelper.getJsonArray<T>(
                    File.ReadAllText(
                        Path.Combine(Sekai_master_db_diff[sr], $"{tableName}.json")));
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

    public class Sekai_master_db_diff
    {
        public string this[ServerRegion serverRegion]
        {
            get
            {
                switch (serverRegion)
                {
                    case ServerRegion.jp:
                        return EnvPath.sekai_master_db_diff;
                    case ServerRegion.tw:
                        return EnvPath.sekai_master_db_tc_diff;
                    case ServerRegion.cn:
                        return EnvPath.sekai_master_db_cn_diff;
                    case ServerRegion.en:
                        return EnvPath.sekai_master_db_en_diff;
                    case ServerRegion.kr:
                        return EnvPath.sekai_master_db_kr_diff;
                    default:
                        return EnvPath.sekai_master_db_diff;
                }
            }
        }
    }
}