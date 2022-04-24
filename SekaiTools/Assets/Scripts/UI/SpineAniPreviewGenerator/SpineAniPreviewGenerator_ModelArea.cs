using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniPreviewGenerator
{
    public class SpineAniPreviewGenerator_ModelArea : MonoBehaviour
    {
        public SpineAniPreviewGenerator spineAniPreviewGenerator;
        [Header("Components")]
        public ButtonGenerator buttonGenerator;
        [Header("Settings")]
        public InbuiltImageData modelPreviewImage;
        public InbuiltSpineModelSet spineModelSet;
        public string defaultModel = "sd_21miku_normal";
        [Header("Prefab")]
        public Window modelSelectWindowPrefab;


        [System.NonSerialized] public List<ClassifiedAnimation> classifiedAnimations = new List<ClassifiedAnimation>();

        public class ClassifiedAnimation
        {
            public string typeName;
            public AtlasAssetPair atlasAssetPair;
            public List<AniamtionPack> animations = new List<AniamtionPack>();

            public ClassifiedAnimation(string typeName)
            {
                this.typeName = typeName;
            }
        }

        public struct AniamtionPack
        {
            public string animation;
            public string animationName;

            public AniamtionPack(string animation, string animationName)
            {
                this.animation = animation;
                this.animationName = animationName;
            }
        }

        public void Awake()
        {
            ClassifyAnimation();

            AtlasAssetPair atlasAssetPair = spineModelSet.GetValue(defaultModel);
            foreach (var classifiedAnimation in classifiedAnimations)
            {
                classifiedAnimation.atlasAssetPair = atlasAssetPair;
            }

            GenerateButtons();
        }

        void ClassifyAnimation()
        {
            global::Spine.SkeletonData skeletonData = spineAniPreviewGenerator.baseModel.GetSkeletonData(false);
            foreach (var animation in skeletonData.Animations)
            {
                string[] nameArray = animation.name.Split('_');
                if(nameArray.Length<=2)
                {
                    AddAnimation(animation.name, animation.name, animation.name);
                }
                else
                {
                    if (nameArray.Length >= 4 && nameArray[nameArray.Length - 1].Equals("b"))
                    {
                        AddAnimation("back", animation.name,string.Join("_",nameArray,0,nameArray.Length-1));
                    }
                    else
                    {
                        AddAnimation($"{nameArray[0]}_{nameArray[1]}", animation.name, string.Join("_", nameArray, 2, nameArray.Length - 2));
                    }
                }
            }
        }

        void AddAnimation(string typeName,string animation,string animationName)
        {
            AniamtionPack item = new AniamtionPack(animation, animationName);
            foreach (var classifiedAnimation in classifiedAnimations)
            {
                if(classifiedAnimation.typeName.Equals(typeName))
                {
                    classifiedAnimation.animations.Add(item);
                    return;
                }
            }
            ClassifiedAnimation newClassifiedAnimation = new ClassifiedAnimation(typeName);
            newClassifiedAnimation.animations.Add(item);
            classifiedAnimations.Add(newClassifiedAnimation);
        }

        void GenerateButtons()
        {
            buttonGenerator.Generate(classifiedAnimations.Count,
                (Button button,int id) =>
                {
                    SpineAniPreviewGenerator_ModelArea_Item item = button.GetComponent<SpineAniPreviewGenerator_ModelArea_Item>();
                    item.typeText.text = classifiedAnimations[id].typeName;
                    item.countText.text = classifiedAnimations[id].animations.Count.ToString();
                    item.iconImage.sprite = modelPreviewImage.GetValue(classifiedAnimations[id].atlasAssetPair.name);
                },
                (int id) =>
                {
                    SpineModelSelect.SpineModelSelect spineModelSelect = spineAniPreviewGenerator.window.OpenWindow<SpineModelSelect.SpineModelSelect>(modelSelectWindowPrefab);
                    spineModelSelect.Initialize(spineModelSet,
                        (AtlasAssetPair atlasAssetPair) =>
                        {
                            classifiedAnimations[id].atlasAssetPair = atlasAssetPair;
                            buttonGenerator.ClearButtons();
                            GenerateButtons();
                        });
                });
        }

    }
}