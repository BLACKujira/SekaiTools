using SekaiTools.Kizuna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.KizunaScenePlayer
{
    public class KizunaScenePlayerCustom_Player_Main : KizunaScenePlayerBase_Player_MainBase
    {
        protected KizunaSceneCustom kizunaScene;

        public void SetScene(KizunaSceneCustom kizunaScene)
        {
            base.SetScene(kizunaScene);

            this.kizunaScene = kizunaScene;

            ((BondsHonorText)bondsHonorOriLv1).text = kizunaScene.textLv1O;
            ((BondsHonorText)bondsHonorOriLv2).text = kizunaScene.textLv2O;
            ((BondsHonorText)bondsHonorOriLv3).text = kizunaScene.textLv3O;

            ((BondsHonorText)bondsHonorTraLv1).text = kizunaScene.textLv1T;
            ((BondsHonorText)bondsHonorTraLv2).text = kizunaScene.textLv2T;
            ((BondsHonorText)bondsHonorTraLv3).text = kizunaScene.textLv3T;
        }
    }
}