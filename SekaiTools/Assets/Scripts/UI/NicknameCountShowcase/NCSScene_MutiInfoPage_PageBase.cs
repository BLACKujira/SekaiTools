using SekaiTools.Count;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public abstract class NCSScene_MutiInfoPage_PageBase : MonoBehaviour
    {
        [Header("Components")]
        public Graphic[] colorCharL;
        public Graphic[] colorCharR;
        public Image iconCharL;
        public Image iconCharR;
        public Text titleText;
        public Text infoText;
        [Header("Settings")]
        public IconSet charIconSet;

        public virtual ConfigUIItem[] ConfigUIItems => null;

        NCSPlayerBase player;
        protected NCSPlayerBase Player => player;

        protected virtual void SetCharLGraphics(int charId)
        {
            foreach (Graphic g in colorCharL) { g.color = ConstData.characters[charId].imageColor; }
            if (iconCharL != null) { iconCharL.sprite = charIconSet.icons[charId]; }
        }

        protected virtual void SetCharRGraphics(int charId)
        {
            if ((talkerId >= 1 && talkerId <= 20) && (charId >= 21 && charId <= 26))
            {
                Unit unit = ConstData.characters[talkerId].unit;
                charId = ConstData.GetUnitVirtualSinger(charId, unit);
            }
            foreach (Graphic g in colorCharR) { g.color = ConstData.characters[charId].imageColor; }
            if (iconCharR != null) { iconCharR.sprite = charIconSet.icons[charId]; }
        }

        int talkerId;
        protected int TalkerId => talkerId;
        NicknameCountData nicknameCountData;
        protected NicknameCountData NicknameCountData => nicknameCountData;

        public virtual void Initialize(NicknameCountData nicknameCountData, int charId, NCSPlayerBase player)
        {
            talkerId = charId;
            this.nicknameCountData = nicknameCountData;
            this.player = player;
            SetCharLGraphics(charId);
        }
    }
}