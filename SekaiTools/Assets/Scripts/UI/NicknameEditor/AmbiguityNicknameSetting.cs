using SekaiTools.Count;
using System;
using UnityEngine;

namespace SekaiTools.UI.NicknameSetting
{
    public class AmbiguityNicknameSetting : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public AmbiguityNicknameSetting_Block block;

        [HideInInspector] public AmbiguityNicknameSet targetSet;
        [HideInInspector] public AmbiguityNicknameSet cloneSet;
        [HideInInspector] public Action<bool> onApply;

        public void Initialize(AmbiguityNicknameSet ambiguityNicknameSet, Action<bool> onApply)
        {
            this.onApply = onApply;
            targetSet = ambiguityNicknameSet;
            cloneSet = targetSet.Clone();
            block.Initialize(cloneSet);
        }

        public void Apply()
        {
            bool saveSuccess = true;
            try
            {
                targetSet.ReplaceValue(cloneSet);
                targetSet.SaveData();
            }
            catch
            {
                saveSuccess = false;
            }
            window.Close();
            onApply(saveSuccess);
        }
    }
}