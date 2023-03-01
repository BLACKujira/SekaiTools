using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    [CreateAssetMenu(menuName = "SekaiTools/NicknameCounterInitialize/InbuiltData")]
    public class NicknameCounterInitialize_InbuiltData : ScriptableObject
    {
        public TextAsset ambiguityNickNameSetData;
        public TextAsset nickNameSetGlobalData;
        public TextAsset[] nickNameSetData = new TextAsset[27];
    }
}