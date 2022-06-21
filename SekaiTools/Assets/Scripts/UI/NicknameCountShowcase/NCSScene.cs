using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public abstract class NCSScene : MonoBehaviour
    {
        public string itemName;
        [TextArea]
        public string description;
        public Sprite preview;
        public float holdTime = 10;
        [System.NonSerialized] public NicknameCountData countData;
        [System.NonSerialized] public NCSPlayerBase player;
        RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public virtual string GetSaveData()
        {
            return holdTime.ToString();
        }
        public virtual void LoadData(string serialisedData)
        {
            holdTime = float.Parse(serialisedData);
        }
        public virtual void NewData()
        {

        }
        public virtual ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value)};
        public virtual string information => $"持续时间 {holdTime.ToString("0.00")}";

        public virtual void Initialize(NCSPlayerBase player)
        {
            this.countData = player.countData;
            this.player = player;
        }
        public abstract void Refresh();
    }
}