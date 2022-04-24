using SekaiTools.Kizuna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayer_Player_Main : KizunaScenePlayerBase_Player_MainBase
    {
        protected KizunaScene kizunaScene;

        public ImageData imageData;

        public void Initialize(ImageData imageData)
        {
            this.imageData = imageData;
        }

        public void SetScene(KizunaScene kizunaScene)
        {
            base.SetScene(kizunaScene);

            this.kizunaScene = kizunaScene;

            ((BondsHonorOrigin)bondsHonorOriLv1).textSprite = imageData.GetValue(kizunaScene.textSpriteLv1);
            ((BondsHonorOrigin)bondsHonorOriLv2).textSprite = imageData.GetValue(kizunaScene.textSpriteLv2);
            ((BondsHonorOrigin)bondsHonorOriLv3).textSprite = imageData.GetValue(kizunaScene.textSpriteLv3);

            ((BondsHonorText)bondsHonorTraLv1).text = kizunaScene.textLv1T;
            ((BondsHonorText)bondsHonorTraLv2).text = kizunaScene.textLv2T;
            ((BondsHonorText)bondsHonorTraLv3).text = kizunaScene.textLv3T;
        }

    }
}
