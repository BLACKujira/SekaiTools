using SekaiTools.Effect;
using System;
using System.Collections;
using UnityEngine;

namespace SekaiTools.UI.Transition
{
    public class Transition01_CustomColor : Transition01Base
    {
        [ColorUsage(true, true)]
        public Color hdrColor = new Color(2, 2, 2, 1);

        public override string DefaultSerializedTransition => JsonUtility.ToJson(new SaveData(hdrColor));

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
                    {
                        new ConfigUIItem_HDRColor("HDRColor","Color",()=>hdrColor,(value)=>hdrColor = value)
                    };

        public override void LoadSettings(string serialisedSettings)
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(serialisedSettings);
            hdrColor = saveData.hdrColor;
        }

        public override string SaveSettings()
        {
            return JsonUtility.ToJson(new SaveData(hdrColor));
        }

        [System.Serializable]
        public class SaveData
        {
            public Color hdrColor;

            public SaveData(Color hdrColor)
            {
                this.hdrColor = hdrColor;
            }
        }

        protected override IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction, TransitionYieldInstruction transitionYieldInstruction)
        {
            GameObject transitionObject = Instantiate(triBurstPrefab);
            IHDRColor iHDRColor = transitionObject.GetComponent<IHDRColor>();
            iHDRColor.hDRColor = hdrColor;
            yield return TransitionBase(changeCoroutine, changeAction, transitionObject, transitionYieldInstruction);
        }
    }
}