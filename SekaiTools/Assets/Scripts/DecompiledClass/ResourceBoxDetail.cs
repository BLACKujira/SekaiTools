// Sekai.ResourceBoxDetail
using MessagePack;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class ResourceBoxDetail
    {
        public string resourceBoxPurpose;
        public int resourceBoxId;
        public int seq;
        public string resourceType;
        public int resourceId;
        public int resourceLevel;
        public int resourceQuantity;
        [IgnoreMember]
        public int Costume3dId
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public int Costume3dGroupId
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public CostumePartType PartType
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public FigureType Figure
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        [IgnoreMember]
        public int ColorId
        {
            get
            {
                return default;
            }
            private set
            {
            }
        }

        public void CostumeSetup()
        {
        }
    }
}