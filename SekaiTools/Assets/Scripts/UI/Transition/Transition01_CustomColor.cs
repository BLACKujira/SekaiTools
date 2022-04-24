using SekaiTools.Effect;
using SekaiTools.UI.BackGround;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Transition
{
    public class Transition01_CustomColor : Transition01Base
    {
        [ColorUsage(true, true)]
        public Color hdrColor = new Color(2, 2, 2, 1);

        protected override IEnumerator Transition(IEnumerator changeCoroutine, Action changeAction)
        {
            BackGroundPart backGroundPart = BackGroundController.AddDecoration(triBurstPrefab);
            backGroundPart.disableRemove = true;
            IHDRColor iHDRColor = backGroundPart.GetModifier<IHDRColor>();
            iHDRColor.hDRColor = hdrColor;
            yield return TransitionBase(changeCoroutine, changeAction, backGroundPart);
        }
    }
}