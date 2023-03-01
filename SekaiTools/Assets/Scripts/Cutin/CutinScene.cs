using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Cutin
{
    /// <summary>
    /// 一个互动语音场景
    /// </summary>
    [System.Serializable]
    public class CutinScene
    {
        public int charFirstID;
        public int charSecondID;
        public int dataID;

        public TalkData talkData_First = new TalkData();
        public TalkData talkData_Second = new TalkData();

        public CutinScene(int charFirstID, int charSecondID, int dataID)
        {
            this.charFirstID = charFirstID;
            this.charSecondID = charSecondID;
            this.dataID = dataID;
            talkData_First = new TalkData();
            talkData_First.talkVoice = new CutinVoiceInfo(CutinVoiceType.bondscp, charFirstID, charSecondID, CutinVoiceOrder.first, dataID).StandardizeName;
            talkData_Second = new TalkData();
            talkData_Second.talkVoice = new CutinVoiceInfo(CutinVoiceType.bondscp, charFirstID, charSecondID, CutinVoiceOrder.second, dataID).StandardizeName;
        }

        [System.Obsolete]
        public CutinScene(CutinSceneData.CutinSceneInfoBase cutinSceneInfo)
        {
            this.charFirstID = cutinSceneInfo.charFirstID;
            this.charSecondID = cutinSceneInfo.charSecondID;
            this.dataID = cutinSceneInfo.dataID;
        }

        [System.Serializable]
        public class TalkData
        {
            public string motionCharFirst;
            public string facialCharFirst;
            public string motionCharSecond;
            public string facialCharSecond;
            public string talkVoice;
            public string talkText;
            public string talkText_Translate;
        }

        public bool IsConversationOf(int charAID, int charBID, bool mergeVirtualSinger = false)
        {
            if (mergeVirtualSinger)
            {
                if (ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(charFirstID)
                    && ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(charSecondID)) return true;
                if (ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(charSecondID)
                    && ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(charFirstID)) return true;
            }
            else
            {
                if (charAID == charFirstID && charBID == charSecondID) return true;
                if (charAID == charSecondID && charBID == charFirstID) return true;
            }
            return false;
        }

        public CutinSceneData.CutinSceneInfoBase GetInfo()
        {
            return new CutinSceneData.CutinSceneInfoBase(charFirstID, charSecondID, dataID);
        }
    }
}