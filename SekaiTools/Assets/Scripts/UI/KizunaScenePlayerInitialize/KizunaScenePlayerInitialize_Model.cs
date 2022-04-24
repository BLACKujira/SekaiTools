using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayerInitialize
{
    public class KizunaScenePlayerInitialize_Model : PlayerInitialize_ModelAreaBase
    {
        public KizunaScenePlayerInitialize kizunaScenePlayerInitialize;

        public override Window window => kizunaScenePlayerInitialize.window;
    }
}