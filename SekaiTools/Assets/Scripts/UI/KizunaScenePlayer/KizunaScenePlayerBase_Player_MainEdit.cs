using SekaiTools.Kizuna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayerBase_Player_MainEdit : KizunaScenePlayerBase_Player_MainBase
    {
        protected KizunaScene kizunaScene;

        public ImageData imageData;

        public void Initialize(ImageData imageData)
        {
            this.imageData = imageData;
            ((BondsHonorTextInput)bondsHonorTraLv1).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv1T = str; });
            ((BondsHonorTextInput)bondsHonorTraLv2).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv2T = str; });
            ((BondsHonorTextInput)bondsHonorTraLv3).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv3T = str; });
        }

        public void SetScene(KizunaScene kizunaScene)
        {
            base.SetScene(kizunaScene);

            this.kizunaScene = kizunaScene;

            ((BondsHonorOrigin)bondsHonorOriLv1).textSprite = imageData.GetValue(kizunaScene.textSpriteLv1);
            ((BondsHonorOrigin)bondsHonorOriLv2).textSprite = imageData.GetValue(kizunaScene.textSpriteLv2);
            ((BondsHonorOrigin)bondsHonorOriLv3).textSprite = imageData.GetValue(kizunaScene.textSpriteLv3);
            
            ((BondsHonorTextInput)bondsHonorTraLv1).text = kizunaScene.textLv1T;
            ((BondsHonorTextInput)bondsHonorTraLv2).text = kizunaScene.textLv2T;
            ((BondsHonorTextInput)bondsHonorTraLv3).text = kizunaScene.textLv3T;
        }
    }
}