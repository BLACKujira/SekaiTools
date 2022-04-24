using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Live2D;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class L2DModelSelect : UniversalSelector
    {
        public IconSet iconSet;
        ModelLoader modelLoader;

        public ModelLoader ModelLoader { get { if (!modelLoader) modelLoader = ModelLoader.modelLoader; return modelLoader; } set => modelLoader = value; }

        private void Awake()
        {
            Title = "选择Live2D模型";
        }
        public void Generate(Action<SekaiLive2DModel> onClick)
        {
            Generate(ModelLoader.models.Count, (Button button, int id) =>
             {
                 ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                 buttonWithIconAndText.Label = ModelLoader.models[id].name;
                 int charID = ConstData.IsLive2DModelOfCharacter(ModelLoader.models[id].name);
                 buttonWithIconAndText.Icon = iconSet.icons[charID];
             },
            (int id) =>
            {
                onClick(ModelLoader.models[id]);
            }
            );
        }
    }
}