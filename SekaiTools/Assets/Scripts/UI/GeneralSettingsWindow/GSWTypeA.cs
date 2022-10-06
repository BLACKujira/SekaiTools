using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSWTypeA : GeneralSettingsWindow
    {
        [Header("GSWPrefab")]
        public GSW_Item_HDRColor item_HDRColor_Prefab;
        public GSW_Item_Float item_Float_Prefab;
        public GSW_Item_Character item_Character_Prefab;
        public GSW_Item_AudioFile item_AudioFile_Prefab;
        public GSW_Item_Live2dModel item_Live2dModel_Prefab;
        public GSW_Item_Live2dAnimation item_Live2dAnimation_Prefab;
        public GSW_Item_SpineScene item_SpineScene_Prefab;
        public GSW_Item_ResetSpineScene item_ResetSpineScene_Prefab;
        public GSW_Item_StringList item_StringListLong_Prefab;

        public override (Type type, GSW_Item item)[] typeDictionary => 
            new (Type type, GSW_Item item)[]
            {
                (typeof(ConfigUIItem_HDRColor),item_HDRColor_Prefab),
                (typeof(ConfigUIItem_Float),item_Float_Prefab),
                (typeof(ConfigUIItem_Character),item_Character_Prefab),
                (typeof(ConfigUIItem_AudioFile),item_AudioFile_Prefab),
                (typeof(ConfigUIItem_Live2dModel),item_Live2dModel_Prefab),
                (typeof(ConfigUIItem_Live2dAnimation),item_Live2dAnimation_Prefab),
                (typeof(ConfigUIItem_SpineScene),item_SpineScene_Prefab),
                (typeof(ConfigUIItem_ResetSpineScene),item_ResetSpineScene_Prefab),
                (typeof(ConfigUIItem_StringListLong),item_StringListLong_Prefab)
            };
    }
}