using SekaiTools.UI.GeneralSettingsWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    [Obsolete]
    public class GIP_GSW : GeneralSettingsWindow.GeneralSettingsWindow
    {


        public override (Type type, GSW_Item item)[] typeDictionary => throw new NotImplementedException();
    }
}