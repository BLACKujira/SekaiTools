using SekaiTools.Spine;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SpineAniShowPlayerInitialize
{
    public class SpineAniShowPlayerInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_PathSelect gIP_PathSelect;
        [Header("Prefab")]
        public Window playerWindowPrefab;

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_PathSelect);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, errors);
                return;
            }

            string saveDataPath = gIP_PathSelect.pathSelectItems[0].SelectedPath;
            SpineAniShowData spineAniShowData;
            try
            {
                spineAniShowData = SpineAniShowData.LoadData(File.ReadAllText(saveDataPath));
            }
            catch
            {
                WindowController.ShowMessage(GenericInitializationCheck.STR_ERROR, "Œﬁ∑®∂¡»°¥Êµµ");
                return;
            }

            SpineAniShowPlayer.SpineAniShowPlayer spineAniShowPlayer = window.OpenWindow<SpineAniShowPlayer.SpineAniShowPlayer>(playerWindowPrefab);
            try
            {
                spineAniShowPlayer.Initialize(spineAniShowData);
            }
            catch(System.Exception ex)
            {
                spineAniShowPlayer.window.Close();
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, ex.ToString());
                return;
            }
        }
    }
}