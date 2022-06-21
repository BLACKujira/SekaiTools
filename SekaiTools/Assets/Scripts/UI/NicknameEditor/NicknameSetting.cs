using SekaiTools.Count;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameSetting
{
    public class NicknameSetting : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public NicknameSetting_Block[] blocks = new NicknameSetting_Block[27];

        [HideInInspector] public NicknameSet targetSet;
        [HideInInspector] public NicknameSet cloneSet;
        [HideInInspector] public event Action onApply;

        public void Initialize(NicknameSet nicknameSetGlobal)
        {
            targetSet = nicknameSetGlobal;
            cloneSet = targetSet.Clone();
            for (int i = 1; i < blocks.Length; i++)
            {
                blocks[i].Initialize(cloneSet.nicknameItems[i]);
            }
        }

        public void Initialize(NicknameSet nicknameSetGlobal, NicknameSet nicknameSetCharacter)
        {
            targetSet = nicknameSetCharacter;
            cloneSet = targetSet.Clone();
            for (int i = 1; i < blocks.Length; i++)
            {
                blocks[i].Initialize(nicknameSetGlobal.nicknameItems[i],cloneSet.nicknameItems[i]);
            }
        }

        public void Apply()
        {
            targetSet.ReplaceValue(cloneSet);
            targetSet.SaveData();
            window.Close();
            onApply();
        }
    }
}