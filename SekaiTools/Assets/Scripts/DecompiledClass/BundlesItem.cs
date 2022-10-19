using System.Collections.Generic;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    [MessagePackObject]
    public class BundlesItem
    {
        [Key(0)] public string bundleName;
        [Key(1)] public string cacheFileName;
        [Key(2)] public string cacheDirectoryName;
        [Key(3)] public string hash;
        [Key(4)] public string category;
        [Key(5)] public int crc;
        [Key(6)] public int fileSize;
        [Key(7)] public List<string> dependencies;
        [Key(8)] public List<string> paths;
        [Key(9)] public string isBuiltin;
    }
}