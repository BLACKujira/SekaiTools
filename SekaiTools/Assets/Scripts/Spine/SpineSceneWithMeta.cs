using SekaiTools.UI.BackGround;
using System.Collections.Generic;
using static SekaiTools.Count.Showcase.NicknameCountShowcase;
using System.Linq;
using SekaiTools.UI.Transition;

namespace SekaiTools.Spine
{
    [System.Serializable]
    public class SpineSceneWithMeta
    {
        public SpineScene spineScene;

        public SpineSceneWithMeta(SpineScene spineScene)
        {
            backGround = new BackGroundController.BackGroundSaveData(BackGroundController.backGroundController);
            this.spineScene = spineScene;
        }

        public float holdTime = 10;

        public bool useTransition = false;
        public SerializedTransition transition = null;
        public bool changeBackGround = false;
        public BackGroundController.BackGroundSaveData backGround = null;

        public string Information
        {
            get
            {
                List<string> infoList = new List<string>();
                
                if (spineScene.spineObjects.Length == 0)
                    infoList.Add("无模型");
                else
                    infoList.Add($"模型：{string.Join(", ", spineScene.spineObjects.Select((so) => so.atlasAssetName))}");

                if (spineScene.HasSameAnimationAndOffset())
                    infoList.Add("存在动画及偏移均相同的模型");
                else if (spineScene.HasSameAnimation())
                    infoList.Add("存在动画相同的模型");
                return string.Join(";\n", infoList);
            }
        }
    }
}