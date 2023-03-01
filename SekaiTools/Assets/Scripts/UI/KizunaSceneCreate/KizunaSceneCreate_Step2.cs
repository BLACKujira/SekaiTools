using SekaiTools.UI.GenericInitializationParts;
using System;
using UnityEngine;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class KizunaSceneCreate_Step2 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_KizunaSceneCreate_CutinData gIP_KizunaSceneCreate_CutinData;

        Action<CreatedKizunaSceneInfo> onApply;
        Vector2Int[] selectedCouple;

        public void Initialize(Vector2Int[] selectedCouple, Action<CreatedKizunaSceneInfo> onApply)
        {
            this.onApply = onApply;
            this.selectedCouple = selectedCouple;
            gIP_KizunaSceneCreate_CutinData.Initialize(selectedCouple);
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_KizunaSceneCreate_CutinData);
            if(!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }
            window.Close();
            window.parentWindow.Close();
            onApply(new CreatedKizunaSceneInfo(selectedCouple,gIP_KizunaSceneCreate_CutinData.CutinSceneData));
        }
    }
}