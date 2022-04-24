using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Spine;
using System;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineModelSelect
{
    public class SpineModelSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public ButtonGeneratorBase buttonGenerator;
        [Header("Settings")]
        public InbuiltImageData modelPreviewImage;

        List<TagItem> tagItems;

        public class TagItem
        {
            public string tag;

            public TagItem(string tag)
            {
                this.tag = tag;
            }

            public List<AtlasAssetPair> atlasAssetPairs = new List<AtlasAssetPair>();
        }

        public void Initialize(InbuiltSpineModelSet spineModelSet,Action<AtlasAssetPair> onSelect)
        {
            tagItems = new List<TagItem>();
            tagItems.Add(new TagItem("all"));
            tagItems.Add(new TagItem("vs"));
            tagItems.Add(new TagItem("l/n"));
            tagItems.Add(new TagItem("mmj"));
            tagItems.Add(new TagItem("vbs"));
            tagItems.Add(new TagItem("ws"));
            tagItems.Add(new TagItem("25"));
            tagItems.Add(new TagItem("other"));

            for (int i = 0; i <= 56; i++)
            {
                tagItems[0].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);

                if (i == 0) tagItems[7].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else if (i <= 4) tagItems[2].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else if (i <= 8) tagItems[3].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else if (i <= 12) tagItems[4].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else if (i <= 16) tagItems[5].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else if (i <= 20) tagItems[6].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
                else tagItems[1].atlasAssetPairs.AddRange(spineModelSet.characters[i].atlasAssets);
            }

            toggleGenerator.Generate(tagItems.Count,
                (Toggle toggle,int id) =>
                {
                    toggle.GetComponentInChildren<Text>().text = tagItems[id].tag;
                },
                (bool value, int id) =>
                {
                    if(value)
                    {
                        buttonGenerator.ClearButtons();
                        buttonGenerator.Generate(tagItems[id].atlasAssetPairs.Count,
                            (Button button,int i) =>
                            {
                                ButtonWithIconAndText buttonWithIconAndText = button.GetComponent<ButtonWithIconAndText>();
                                string itemName = tagItems[id].atlasAssetPairs[i].name;
                                buttonWithIconAndText.Label = itemName;
                                buttonWithIconAndText.Icon = modelPreviewImage.GetValue(itemName);
                            },
                            (int i) =>
                            {
                                onSelect(tagItems[id].atlasAssetPairs[i]);
                                window.Close();
                            });
                    }
                });

            toggleGenerator.toggles[0].isOn = true;
        }
    }
}