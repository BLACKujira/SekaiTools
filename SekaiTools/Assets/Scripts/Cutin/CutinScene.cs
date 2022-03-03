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
    }
}