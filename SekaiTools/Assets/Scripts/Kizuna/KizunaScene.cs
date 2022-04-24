using SekaiTools.Cutin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Kizuna
{
    [System.Serializable]
    public class KizunaSceneBase
    {
        public int charAID;
        public int charBID;
        public List<CutinScene> cutinScenes = new List<CutinScene>();

        public string facialA;
        public string motionA;
        public string facialB;
        public string motionB;

        public KizunaSceneBase(int charAID, int charBID)
        {
            this.charAID = charAID;
            this.charBID = charBID;
        }

        public bool IsKizunaOf(CutinScene cutinScene)
        {
            return cutinScene.IsConversationOf(charAID, charBID);
        }

        public bool IsKizunaOf(int charAID, int charBID)
        {
            if (ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(this.charAID)
                && ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(this.charBID)) return true;
            if (ConstData.MergeVirtualSinger(charBID) == ConstData.MergeVirtualSinger(this.charAID) 
                && ConstData.MergeVirtualSinger(charAID) == ConstData.MergeVirtualSinger(this.charBID)) return true;
            return false;
        }
    }

    /// <summary>
    /// 一个牵绊互动场景，含有互动语音场景
    /// </summary>
    [System.Serializable]
    public class KizunaScene : KizunaSceneBase
    {
        public string textSpriteLv1;
        public string textSpriteLv2;
        public string textSpriteLv3;
        
        public string textLv1T;
        public string textLv2T;
        public string textLv3T;

        public KizunaScene(int charAID, int charBID) : base(charAID, charBID)
        {
        }
    }

    /// <summary>
    /// 可自定义原文的牵绊互动场景
    /// </summary>
    [System.Serializable]
    public class KizunaSceneCustom : KizunaSceneBase
    {
        public string textLv1O;
        public string textLv2O;
        public string textLv3O;

        public string textLv1T;
        public string textLv2T;
        public string textLv3T;

        public KizunaSceneCustom(int charAID, int charBID) : base(charAID, charBID)
        {
        }
    }
}