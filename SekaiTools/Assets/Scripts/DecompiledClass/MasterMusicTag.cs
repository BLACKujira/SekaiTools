// Sekai.MasterMusicTag
using MessagePack;
using System;

namespace SekaiTools.DecompiledClass
{
    [System.Serializable]
    public class MasterMusicTag : IMessagePackSerializationCallbackReceiver
    {
        public int musicId;
        public string musicTag;
        public int seq;

        [IgnoreMember]
        public MusicTag MusicTag
        {
            get
            {
                return (MusicTag)Enum.Parse(typeof(MusicTag),musicTag);
            }
            private set
            {
                musicTag = value.ToString();
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }
}