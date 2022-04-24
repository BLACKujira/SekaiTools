using SekaiTools.Kizuna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayerBase_Player_MainEditCustom : KizunaScenePlayerBase_Player_MainBase
    {
        protected KizunaSceneCustom kizunaScene;

        public void Initialize()
        {
            ((BondsHonorTextInput)bondsHonorOriLv1).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv1O = str; });
            ((BondsHonorTextInput)bondsHonorOriLv2).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv2O = str; });
            ((BondsHonorTextInput)bondsHonorOriLv3).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv3O = str; });

            ((BondsHonorTextInput)bondsHonorTraLv1).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv1T = str; });
            ((BondsHonorTextInput)bondsHonorTraLv2).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv2T = str; });
            ((BondsHonorTextInput)bondsHonorTraLv3).inputField.onValueChanged.AddListener((str) => { kizunaScene.textLv3T = str; });
        }

        public void SetScene(KizunaSceneCustom kizunaScene)
        {
            base.SetScene(kizunaScene);

            this.kizunaScene = kizunaScene;

            ((BondsHonorTextInput)bondsHonorOriLv1).text = kizunaScene.textLv1O;
            ((BondsHonorTextInput)bondsHonorOriLv2).text = kizunaScene.textLv2O;
            ((BondsHonorTextInput)bondsHonorOriLv3).text = kizunaScene.textLv3O;

            ((BondsHonorTextInput)bondsHonorTraLv1).text = kizunaScene.textLv1T;
            ((BondsHonorTextInput)bondsHonorTraLv2).text = kizunaScene.textLv2T;
            ((BondsHonorTextInput)bondsHonorTraLv3).text = kizunaScene.textLv3T;
        }
    }
}