using SekaiTools.Count;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_UnitFriend : NCSScene
    {
        [Header("Components")]
        public NCSScene_UnitFriend_Item[] items;
        public Image imgUnitIcon;
        [Header("Settings")]
        public bool isVirtualSinger = false;
        public IconSet unitIconSet;
        public Unit unit = Unit.Leoneed;
        public Unit Unit => unit;

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Unit("组合","scene",()=>unit,(value)=>unit = value)
            };

        public override string Information => $"组合 {ConstData.units[unit].name} , " + base.Information;

        public override void Refresh()
        {
            if (isVirtualSinger && unit != Unit.VirtualSinger)
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, "此页面只能用于虚拟歌手");
                unit = Unit.VirtualSinger;
            }
            if (!isVirtualSinger && unit == Unit.VirtualSinger)
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, "此页面不能用于虚拟歌手");
                unit = Unit.Leoneed;
            }

            Sprite unitIcon = unitIconSet.icons[(int)unit];
            imgUnitIcon.sprite = unitIcon;
            imgUnitIcon.rectTransform.sizeDelta = new Vector2(unitIcon.texture.width, unitIcon.texture.height);

            List<CountResult> countResults = new List<CountResult>();
            for (int i = 1; i < 21; i++)
            {
                if (ConstData.characters[i].unit == unit) continue;
                countResults.Add(new CountResult(i, unit, countData));

                countResults.Sort((x, y) => x.Total.CompareTo(y.Total));
                countResults.Reverse();
            }

            for (int i = 0; i < items.Length; i++)
            {
                NCSScene_UnitFriend_Item item = items[i];
                CountResult countResult = countResults[i];
                item.Initialize(countResult.charId, countResult.countChars);
            }
        }

        public override void LoadData(string serializedData)
        {
            try
            { 
                SaveData saveData = JsonUtility.FromJson<SaveData>(serializedData);
                holdTime = saveData.holdTime;
                unit = saveData.unit;
            }
            catch { }
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new SaveData(this));
        }

        [System.Serializable]
        public class SaveData
        {
            public float holdTime;
            public Unit unit;

            public SaveData(NCSScene_UnitFriend nCSScene_UnitFriend)
            {
                holdTime = nCSScene_UnitFriend.holdTime;
                unit = nCSScene_UnitFriend.unit;
            }
        }

        public class CountResult
        {
            public readonly int charId;

            public CountResult(int charId, Unit unit, NicknameCountData countData)
            {
                this.charId = charId;
                if (unit != Unit.VirtualSinger)
                {
                    countChars = new Vector2Int[4];
                    int startId = ((int)unit - 2) * 4 + 1;
                    for (int i = 0; i < 4; i++)
                    {
                        countChars[i] = new Vector2Int(startId + i, 0);
                    }
                }
                else
                {
                    countChars = new Vector2Int[6];
                    int startId = 21;
                    for (int i = 0; i < 6; i++)
                    {
                        countChars[i] = new Vector2Int(startId + i, 0);
                    }
                }

                for (int i = 0; i < countChars.Length; i++)
                {
                    Vector2Int vector2Int = countChars[i];
                    NicknameCountItem nicknameCountItem = countData[vector2Int.x, charId];
                    countChars[i].y = nicknameCountItem.Total;
                }
            }

            public readonly Vector2Int[] countChars;
            public int Total => countChars.Sum(v2 => v2.y);
        }
    }
}