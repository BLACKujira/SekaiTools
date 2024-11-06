using SekaiTools.UI.StorySelector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class MobInfoViewer_Item : MonoBehaviour
    {
        MobInfoViewer mobInfoViewer;
        MobInfo mobInfo;
        [Header("Components")]
        public Text txtId;
        public Text txtNicknames;
        [Header("Prefab")]
        public Window storySelectorPrefab;

        public void SetData(MobInfoViewer mobInfoViewer, MobInfo mobInfo)
        {
            this.mobInfoViewer = mobInfoViewer;
            this.mobInfo = mobInfo;

            txtId.text = $"ID: {mobInfo.characterId}";

            string[] nicknames = mobInfo.nicknames
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToArray();
            txtNicknames.text = $"昵称: {string.Join(" ", nicknames)}";
        }

        public void OpenStorySelector()
        {
            List<StoryManager> storyManagers = mobInfoViewer.MobInfoCounter.GetStories(mobInfo.characterId);
            StorySelector_StoryViewer storySelector_StoryViewer = mobInfoViewer.window.OpenWindow<StorySelector.StorySelector_StoryViewer>(storySelectorPrefab);
            storySelector_StoryViewer.Initialize(storyManagers);
        }

        public void ViewCostumeTypes()
        {
            string costumeTypes = string.Join(" ", mobInfo.costumeTypes);
            WindowController.ShowMessage("此角色的Live2D模型", costumeTypes);
        }
    }
}
