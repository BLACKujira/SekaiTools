using SekaiTools.UI.GenericInitializationParts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class KizunaSceneCreate_Step1 : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_KizunaSceneCreate_KizunaCreate gIP_KizunaSceneCreate_KizunaCreate;
        [Header("Prefab")]
        public Window step2WindowPrefab;

        Action<CreatedKizunaSceneInfo> onApply;

        public void Initialize(Action<CreatedKizunaSceneInfo> onApply)
        {
            this.onApply = onApply;
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_KizunaSceneCreate_KizunaCreate);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR,errors);
                return;
            }

            KizunaSceneCreate_Step2 kizunaSceneCreate_Step2 
                = window.OpenWindow<KizunaSceneCreate_Step2>(step2WindowPrefab);
            Vector2Int[] oriSelectedCouple = gIP_KizunaSceneCreate_KizunaCreate.SelectedCouple;
            Vector2Int[] selectedCouple = new Vector2Int[oriSelectedCouple.Length];
            for (int i = 0; i < selectedCouple.Length; i++)
            {
                int charAID = oriSelectedCouple[i].x;
                int charBID = oriSelectedCouple[i].y;
                ConstData.ConvergeVirtualSingerToCharacter(ref charAID, ref charBID);
                selectedCouple[i] = new Vector2Int(charAID, charBID);
            }

            kizunaSceneCreate_Step2.Initialize(selectedCouple, onApply);
        }
    }
}