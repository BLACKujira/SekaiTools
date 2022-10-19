using System.Collections.Generic;
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    [MessagePackObject]
    public class BundleRoot
    {
        [Key(0)] public string version;
        [Key(1)] public string os;
        [Key(2)] public List<BundlesItem> bundles;
    }
}