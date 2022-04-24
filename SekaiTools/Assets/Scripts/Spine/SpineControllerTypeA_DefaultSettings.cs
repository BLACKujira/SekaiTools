using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Spine
{
    [CreateAssetMenu(menuName = "SekaiTools/Spine/DefaultSettings")]
    public class SpineControllerTypeA_DefaultSettings : ScriptableObject
    {
        public DefaultSettingItem[] defaultPosition_1 = new DefaultSettingItem[1];
        public DefaultSettingItem[] defaultPosition_2 = new DefaultSettingItem[2];
        public DefaultSettingItem[] defaultPosition_3 = new DefaultSettingItem[3];
        public DefaultSettingItem[] defaultPosition_4 = new DefaultSettingItem[4];
        public DefaultSettingItem[] defaultPosition_5 = new DefaultSettingItem[5];
        public DefaultSettingItem[] defaultPosition_6 = new DefaultSettingItem[6];

        public DefaultSettingItem GetDefaultSetting(int characterOrder, int modelCount)
        {
            switch (modelCount)
            {
                case 1: return defaultPosition_1[characterOrder];
                case 2: return defaultPosition_2[characterOrder];
                case 3: return defaultPosition_3[characterOrder];
                case 4: return defaultPosition_4[characterOrder];
                case 5: return defaultPosition_5[characterOrder];
                case 6: return defaultPosition_6[characterOrder];
                default: return characterOrder < 6 ? defaultPosition_6[characterOrder] : new DefaultSettingItem();
            }
        }

        [System.Serializable]
        public class DefaultSettingItem
        {
            public Vector2 position;
            public bool flipX;

            public DefaultSettingItem()
            {
                position = Vector2.zero;
                flipX = false;
            }

            public DefaultSettingItem(Vector2 position, bool flipX)
            {
                this.position = position;
                this.flipX = flipX;
            }
        }
    }
}