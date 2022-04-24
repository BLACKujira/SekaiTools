using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Live2D;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text textName;
        public Text textAniSet;
        public Button buttonSetAniSet;
        public Image characterIcon;
        [Header("Prefabs")]
        public Window universalSelectorPrefab;
        [Header("Settings")]
        public IconSet iconSet;

        SekaiLive2DModel model;

        public void Initialize(SekaiLive2DModel model,InbuiltAnimationSet animationSets, Window inWindow)
        {
            this.model = model;
            textName.text = model.name;
            buttonSetAniSet.onClick.AddListener(
                () =>
                {
                    Window window = Instantiate(universalSelectorPrefab);
                    window.Initialize(inWindow);
                    UniversalSelector universalSelector = (UniversalSelector)window.controlScript;
                    window.Show();
                    universalSelector.Generate(animationSets.l2DAnimationSets.Length,
                        (Button button,int id)=>
                        {
                            ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                            if(id<26)
                            {
                                buttonWithIconAndText.Icon = iconSet.icons[id + 1];
                            }
                            else
                                buttonWithIconAndText.HideIcon();
                            buttonWithIconAndText.Label = animationSets.l2DAnimationSets[id].name;
                        },
                        (int id)=>
                        {
                            if(id==-1)
                                model.AnimationSet = null;
                            else    
                                model.AnimationSet = animationSets.l2DAnimationSets[id];
                            RefreshInfo();
                        }
                    );
                });
            RefreshInfo();
        }
        public void RefreshInfo()
        {
            textAniSet.text = model.AnimationSet == null ? "无动画集" : model.AnimationSet.name;
            int id = ConstData.IsLive2DModelOfCharacter(model.name);
            characterIcon.sprite = iconSet.icons[id];
        }
    }
}