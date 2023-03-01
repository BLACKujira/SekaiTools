// Sekai.MasterVirtualLiveSetlist
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [Serializable]
    public class MasterVirtualLiveSetlist
    {
        public int id;
        public int virtualLiveId;
        public int seq;
        public string virtualLiveSetlistType;
        public string assetbundleName;
        public int? virtualLiveStageId;
        public int? musicVocalId;
        public int? character3dId1;
        public int? character3dId2;
        public int? character3dId3;
        public int? character3dId4;
        public int? character3dId5;
        public int? character3dId6;
        [IgnoreMember]
        public VirtualLiveSetlistType VirtualLiveSetlistType => (VirtualLiveSetlistType)Enum.Parse(typeof(VirtualLiveSetlistType), virtualLiveSetlistType);

        [IgnoreMember]
        public int[] Character3dIds
        {
            get
            {
                return null;
            }
        }
    }
}