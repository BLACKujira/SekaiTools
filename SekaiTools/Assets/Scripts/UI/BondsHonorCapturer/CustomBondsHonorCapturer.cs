using SekaiTools.Kizuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BondsHonorCapturer
{
    public class CustomBondsHonorCapturer : BondsHonorCapturerBase
    {

        public new void StartCapture(KizunaSceneBase kizunaSceneBase, string saveFolder)
        {
            KizunaSceneCustom kizunaScene = (KizunaSceneCustom)kizunaSceneBase;
            ((BondsHonorText)honor_Lv1_Ori_Ord).text = kizunaScene.textLv1O;
            ((BondsHonorText)honor_Lv1_Ori_Inv).text = kizunaScene.textLv1O;

            ((BondsHonorText)honor_Lv2_Ori_Ord).text = kizunaScene.textLv2O;
            ((BondsHonorText)honor_Lv2_Ori_Inv).text = kizunaScene.textLv2O;

            ((BondsHonorText)honor_Lv3_Ori_Ord).text = kizunaScene.textLv3O;
            ((BondsHonorText)honor_Lv3_Ori_Inv).text = kizunaScene.textLv3O;

            ((BondsHonorText)honor_Lv1_Tra_Ord).text = kizunaScene.textLv1T;
            ((BondsHonorText)honor_Lv1_Tra_Inv).text = kizunaScene.textLv1T;

            ((BondsHonorText)honor_Lv2_Tra_Ord).text = kizunaScene.textLv2T;
            ((BondsHonorText)honor_Lv2_Tra_Inv).text = kizunaScene.textLv2T;

            ((BondsHonorText)honor_Lv3_Tra_Ord).text = kizunaScene.textLv3T;
            ((BondsHonorText)honor_Lv3_Tra_Inv).text = kizunaScene.textLv3T;

            base.StartCapture(kizunaSceneBase, saveFolder);
        }
    }
}