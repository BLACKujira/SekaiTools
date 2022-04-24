using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAnimationSelect
{
    public class SpineAnimationSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public ButtonGeneratorBase buttonGenerator;
        [Header("Settings")]
        public InbuiltImageData modelPreviewImage;
        public SkeletonDataAsset baseModel;

        List<TagItem> tagItems = new List<TagItem>();

        public class TagItem
        {
            public string tag;

            public TagItem(string tag)
            {
                this.tag = tag;
            }

            public List<string> animations = new List<string>();
        }

        public void Initialize(Action<string> onSelect)
        {
            List<string> animations = new List<string>();
            ExposedList<global::Spine.Animation> spineAnimations = baseModel.GetSkeletonData(false).animations;
            foreach (var animation in spineAnimations)
            {
                animations.Add(animation.name);
            }

            tagItems.Add(new TagItem("all"));

            foreach (var animation in animations)
            {
                string[] nameArray = animation.Split('_');
                if (nameArray.Length <= 2)
                {
                    AddAnimation(animation, animation);
                }
                else
                {
                    if (nameArray.Length >= 4 && nameArray[nameArray.Length - 1].Equals("b"))
                    {
                        AddAnimation("back", animation);
                    }
                    else
                    {
                        AddAnimation($"{nameArray[0]}_{nameArray[1]}", animation);
                    }
                }
            }

            MergeFewerItems();

            toggleGenerator.Generate(tagItems.Count,
                (Toggle toggle, int id) =>
                {
                     toggle.GetComponentInChildren<Text>().text = tagItems[id].tag;
                },
                (bool value, int id) =>
                {
                if (value)
                {
                    buttonGenerator.ClearButtons();
                    buttonGenerator.Generate(tagItems[id].animations.Count,
                        (Button button, int i) =>
                        {
                            button.image.sprite = modelPreviewImage.GetValue(tagItems[id].animations[i]);
                        },
                         (int i) =>
                        {
                            onSelect(tagItems[id].animations[i]);
                            window.Close();
                        });
                }
        });

            toggleGenerator.toggles[0].isOn = true;
        }

        void AddAnimation(string tag, string animation)
        {
            tagItems[0].animations.Add(animation);
            foreach (var classifiedAnimation in tagItems)
            {
                if (classifiedAnimation.tag.Equals(tag))
                {
                    classifiedAnimation.animations.Add(animation);
                    return;
                }
            }
            TagItem newTagItem = new TagItem(tag);
            newTagItem.animations.Add(animation);
            tagItems.Add(newTagItem);
        }

        void MergeFewerItems()
        {
            TagItem tagItem = new TagItem("other");
            List<TagItem> removeItems = new List<TagItem>();
            foreach (var item in tagItems)
            {
                if (item.animations.Count < 5)
                {
                    removeItems.Add(item);
                    tagItem.animations.AddRange(item.animations);
                }
            }

            foreach (var item in removeItems)
            {
                tagItems.Remove(item);
            }

            tagItems.Add(tagItem);
        }
    }
}