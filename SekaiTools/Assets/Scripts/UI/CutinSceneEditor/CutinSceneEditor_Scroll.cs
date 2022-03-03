using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SekaiTools.Cutin;
using UnityEngine.UI;

namespace SekaiTools.UI.CutinSceneEditor
{
    public class CutinSceneEditor_Scroll : ToggleGenerator
    {
        public List<CutinSelectButton> buttons { get; private set; } = new List<CutinSelectButton>();
        public CutinSelectButton currentButton { get; private set; } = null;

        public void Generate(CutinSceneData cutinSceneData,Action<CutinScene> onButtonClick)
        {
            Generate(cutinSceneData.cutinScenes.Count, (Toggle toggle, int id) =>
             {
                 CutinSelectButton cutinSelectButton = toggle.GetComponent<CutinSelectButton>();
                 CutinScene scene = cutinSceneData.cutinScenes[id];
                 cutinSelectButton.SetCharacter(scene.charFirstID, scene.charSecondID);
                 cutinSelectButton.SetLevel(scene.dataID);
                 toggle.onValueChanged.AddListener((bool value) => { if (value) cutinSelectButton.Select(); else cutinSelectButton.Unselect(); });
             },
            (bool value, int id) =>
            {
                if (value)
                {
                    CutinScene scene = cutinSceneData.cutinScenes[id];
                    onButtonClick(scene);
                }
            }
            );
        }
    }
}