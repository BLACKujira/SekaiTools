using SekaiTools.Spine;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SpineAniShowEditorInitialize
{
    public class SpineAniShowEditorInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_SASSaveData gIP_SASSaveData;
        [Header("Prefab")]
        public Window editorWindowPrefab;

        public void Apply()
        {
            string initError = GenericInitializationCheck.CheckIfReady(gIP_SASSaveData);
            if (!string.IsNullOrEmpty(initError))
            {
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, initError);
                return;
            }

            SpineAniShowData spineAniShowData;
            try
            {
                if (gIP_SASSaveData.IfNewFile)
                {
                    if (gIP_SASSaveData.SelectedTemplate == null)
                        spineAniShowData = new SpineAniShowData();
                    else
                        spineAniShowData = SpineAniShowData.LoadData(gIP_SASSaveData.SelectedTemplate.text);
                }
                else
                {
                    spineAniShowData = SpineAniShowData.LoadData(File.ReadAllText(gIP_SASSaveData.SelectedDataPath));
                }
                spineAniShowData.SavePath = gIP_SASSaveData.SelectedDataPath;
            }
            catch
            {
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR, "Œﬁ∑®∂¡»°¥Êµµ");
                return;
            }

            SpineAniShowEditor.SpineAniShowEditor spineAniShowEditor = window.OpenWindow<SpineAniShowEditor.SpineAniShowEditor>(editorWindowPrefab);
            try
            {
                spineAniShowEditor.Initialize(spineAniShowData);
            }
            catch(System.Exception ex)
            {
                spineAniShowEditor.window.Close();
                WindowController.ShowLog(GenericInitializationCheck.STR_ERROR,ex.ToString());
                return;
            }
        }
    }
}