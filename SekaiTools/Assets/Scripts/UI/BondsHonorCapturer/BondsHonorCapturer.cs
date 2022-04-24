using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.UI.ImageCapturer;
using SekaiTools.Kizuna;
using System;

namespace SekaiTools.UI.BondsHonorCapturer
{
    public abstract class BondsHonorCapturerBase : MonoBehaviour
    {
        public ImageCapturer.ImageCapturer imageCapturer;
        [Header("Components_Ori")]
        public BondsHonorBase honor_Lv1_Ori_Ord;
        public BondsHonorBase honor_Lv1_Ori_Inv;

        public BondsHonorBase honor_Lv2_Ori_Ord;
        public BondsHonorBase honor_Lv2_Ori_Inv;

        public BondsHonorBase honor_Lv3_Ori_Ord;
        public BondsHonorBase honor_Lv3_Ori_Inv;

        [Header("Components_Tra")]
        public BondsHonorBase honor_Lv1_Tra_Ord;
        public BondsHonorBase honor_Lv1_Tra_Inv;

        public BondsHonorBase honor_Lv2_Tra_Ord;
        public BondsHonorBase honor_Lv2_Tra_Inv;

        public BondsHonorBase honor_Lv3_Tra_Ord;
        public BondsHonorBase honor_Lv3_Tra_Inv;

        [Header("Components_Sub")]
        public BondsHonorBase honor_Sub_Ord;
        public BondsHonorBase honor_Sub_Inv;

        public BondsHonorBase[] honor_Ord => new BondsHonorBase[]
        {
            honor_Lv1_Ori_Ord,
            honor_Lv2_Ori_Ord,
            honor_Lv3_Ori_Ord,

            honor_Lv1_Tra_Ord,
            honor_Lv2_Tra_Ord,
            honor_Lv3_Tra_Ord,

            honor_Sub_Ord
        };

        public BondsHonorBase[] honor_Inv => new BondsHonorBase[]
        {
            honor_Lv1_Ori_Inv,
            honor_Lv2_Ori_Inv,
            honor_Lv3_Ori_Inv,

            honor_Lv1_Tra_Inv,
            honor_Lv2_Tra_Inv,
            honor_Lv3_Tra_Inv,

            honor_Sub_Inv
        };

        protected void StartCapture(KizunaSceneBase kizunaSceneBase,string saveFolder)
        {
            foreach (var bondsHonorBase in honor_Ord)
            {
                bondsHonorBase.SetCharacter(kizunaSceneBase.charAID, kizunaSceneBase.charBID);
            }
            foreach (var bondsHonorBase in honor_Inv)
            {
                bondsHonorBase.SetCharacter(kizunaSceneBase.charBID, kizunaSceneBase.charAID);
            }

            imageCapturer.StartCapture(saveFolder);
        }
    }
    public class BondsHonorCapturer : BondsHonorCapturerBase
    {
        public ImageData imageData;

        public void StartCapture(KizunaSceneBase kizunaSceneBase, ImageData imageData, string saveFolder)
        {
            this.imageData = imageData;

            KizunaScene kizunaScene = (KizunaScene)kizunaSceneBase;
            ((BondsHonorOrigin)honor_Lv1_Ori_Ord).textSprite = imageData.GetValue(kizunaScene.textSpriteLv1);
            ((BondsHonorOrigin)honor_Lv1_Ori_Inv).textSprite = imageData.GetValue(kizunaScene.textSpriteLv1);

            ((BondsHonorOrigin)honor_Lv2_Ori_Ord).textSprite = imageData.GetValue(kizunaScene.textSpriteLv2);
            ((BondsHonorOrigin)honor_Lv2_Ori_Inv).textSprite = imageData.GetValue(kizunaScene.textSpriteLv2);

            ((BondsHonorOrigin)honor_Lv3_Ori_Ord).textSprite = imageData.GetValue(kizunaScene.textSpriteLv3);
            ((BondsHonorOrigin)honor_Lv3_Ori_Inv).textSprite = imageData.GetValue(kizunaScene.textSpriteLv3);

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