using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using UnityEngine.UI;
using System;

namespace SekaiTools.UI
{
    public class PlayerInitialize_ModelAreaBase : MonoBehaviour
    {
        public virtual Window window{ get ;}

        [Header("Components")]
        public ButtonGenerator buttonGenerator;
        [Header("Settings")]
        public IconSet iconSet;
        [Header("Prefabs")]
        public Window l2DModelSelectWindow;

        [NonSerialized] public SekaiLive2DModel[] sekaiLive2DModels = new SekaiLive2DModel[57];
        int[] appearCharacters;

        public void Initialize(int[] appearCharacters)
        {
            if (appearCharacters != null)
            {
                this.appearCharacters = appearCharacters;
                AutoSetModel();
            }
            else
            {
                buttonGenerator.ClearButtons();
            }
        }

        public void AutoSetModel()
        {
            if (appearCharacters == null) return;
            List<SekaiLive2DModel> models = new List<SekaiLive2DModel>();
            foreach (var charID in appearCharacters)
            {
                foreach (var model in models)
                {
                    if (model.name.Contains(((Character)charID).ToString()))
                    {
                        sekaiLive2DModels[charID] = model;
                        break;
                    }
                }
            }
            Refresh();
        }

        void GenerateButton()
        {
            buttonGenerator.Generate(appearCharacters.Length, (Button button, int id) =>
            {
                ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                buttonWithIconAndText.Icon = iconSet.icons[appearCharacters[id]];
                buttonWithIconAndText.Label = sekaiLive2DModels[appearCharacters[id]] == null ? "无模型" : sekaiLive2DModels[appearCharacters[id]].name;
            },
            (int id) =>
            {
                //L2DModelSelect l2DModelSelect = window.OpenWindow<L2DModelSelect>(l2DModelSelectWindow);
                //l2DModelSelect.Generate((SekaiLive2DModel model) => { sekaiLive2DModels[appearCharacters[id]] = model; Refresh(); });
            });
        }

        void Refresh()
        {
            buttonGenerator.ClearButtons();
            GenerateButton();
        }

        public bool IfGetReady()
        {
            if (appearCharacters == null || appearCharacters.Length == 0) return false;
            foreach (var i in appearCharacters)
            {
                if (sekaiLive2DModels[i] == null) return false;
            }
            return true;
        }
    }
}


namespace SekaiTools.UI.CutinScenePlayerInitialize
{
    public class CutinScenePlayerInitialize_ModelArea : PlayerInitialize_ModelAreaBase
    {
        public CutinScenePlayerInitialize cutinScenePlayerInitialize;

        public override Window window => cutinScenePlayerInitialize.window;
    }
}