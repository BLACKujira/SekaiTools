using UnityEngine;

namespace SekaiTools.UI.StorySelector
{
    public class StorySelector_StoryViewer : StorySelector
    {
        [Header("Prefab")]
        public Window storyViewerPrefab;

        protected override void OnClick(StoryManager storyManager)
        {
            StoryViewer.StoryViewer storyViewer = window.OpenWindow<StoryViewer.StoryViewer>(storyViewerPrefab);
            storyViewer.Initialize(storyManager);
        }
    }
}