using SekaiTools.UI.L2DModelSelect;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI
{
    public class L2DModelSelectArea : MonoBehaviour
    {
        public enum StatusEnum { Ready, MissingModel, MissingAnimationSet }

        public UniversalGenerator universalGenerator;
        List<L2DModelSelectArea_Item> items = new List<L2DModelSelectArea_Item>();

        public StatusEnum Status
        {
            get
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.SelectedModel.modelName))
                        return StatusEnum.MissingModel;
                }
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.SelectedModel.animationSet))
                        return StatusEnum.MissingAnimationSet;
                }
                return StatusEnum.Ready;
            }
        }

        public Dictionary<string, SelectedModelInfo> KeyValuePairs
        {
            get
            {
                Dictionary<string, SelectedModelInfo> dictionary = new Dictionary<string, SelectedModelInfo>();
                foreach (var item in items)
                {
                    dictionary[item.Key] = item.SelectedModel;
                }
                return dictionary;
            }
        }

        public void Initialize(L2DModelSelectArea_ItemSettings[] settings)
        {
            universalGenerator.Generate(settings.Length, (gobj, id) =>
             {
                 L2DModelSelectArea_Item l2DModelSelectArea_Item = gobj.GetComponent<L2DModelSelectArea_Item>();
                 l2DModelSelectArea_Item.Initialize(settings[id]);
                 items.Add(l2DModelSelectArea_Item);
             });
        }

        public void Clear()
        {
            universalGenerator.ClearItems();
            items = new List<L2DModelSelectArea_Item>();
        }
    }

    public class L2DModelSelectArea_ItemSettings
    {
        public int characterId;
        public string key;
        public string keyOverride;
        public string defaultModel;

        public L2DModelSelectArea_ItemSettings(int characterId, string key, string keyOverride = null, string defaultModel = null)
        {
            this.characterId = characterId;
            this.key = key;
            this.keyOverride = keyOverride;
            this.defaultModel = defaultModel;
        }
    }
}