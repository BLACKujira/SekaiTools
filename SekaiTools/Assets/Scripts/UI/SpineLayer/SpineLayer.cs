using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.BackGround;
using Spine.Unity;

namespace SekaiTools.UI.SpineLayer
{
    public class SpineLayer : BGModifierBase
    {
        public override void Deserialize(string serializedData)
        {
            
        }

        public override string Serialize()
        {
            return "如果看到了这段文字，请联系作者，这不应该出现。";
        }
    }
}