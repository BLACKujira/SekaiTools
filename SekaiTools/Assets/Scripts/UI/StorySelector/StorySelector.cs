using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.StorySelector
{
    public abstract class StorySelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Toggle toggle_Unit;
        public Toggle toggle_Event;
        public Toggle toggle_Card;
        public Toggle toggle_Map;
        public Toggle toggle_Live;
        public Toggle toggle_Other;
        public ButtonGenerator buttonGenerator;

        StoryType currentStoryType = StoryType.UnitStory;
        Dictionary<StoryType, List<StoryManager>> stories = new Dictionary<StoryType, List<StoryManager>>();

        StoryDescriptionGetter storyDescriptionGetter = new StoryDescriptionGetter();
        StoryPublishTimeGetter storyPublishTimeGetter = new StoryPublishTimeGetter();
        SVStoryUrlGetter svStoryUrlGetter = new SVStoryUrlGetter();

        public static string[] RequireMasterTables
        {
            get
            {
                HashSet<string> hashSet = new HashSet<string>();
                hashSet.UnionWith(StoryDescriptionGetter.RequireMasterTables);
                hashSet.UnionWith(SVStoryUrlGetter.RequireMasterTables);
                hashSet.UnionWith(StoryPublishTimeGetter.RequireMasterTables);
                return hashSet.ToArray();
            }
        }

        public void Initialize(List<StoryManager> stories)
        {
            this.stories[StoryType.UnitStory] = new List<StoryManager>();
            this.stories[StoryType.EventStory] = new List<StoryManager>();
            this.stories[StoryType.CardStory] = new List<StoryManager>();
            this.stories[StoryType.MapTalk] = new List<StoryManager>();
            this.stories[StoryType.LiveTalk] = new List<StoryManager>();
            this.stories[StoryType.OtherStory] = new List<StoryManager>();

            foreach (var story in stories)
            {
                if (!story.Initialized)
                {
                    story.InitInfo(storyDescriptionGetter, storyPublishTimeGetter, svStoryUrlGetter);
                }
                this.stories[story.storyType].Add(story);
            }

            toggle_Unit.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.UnitStory;
                Refresh();
            });
            toggle_Event.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.EventStory;
                Refresh();
            });
            toggle_Card.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.CardStory;
                Refresh();
            });
            toggle_Map.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.MapTalk;
                Refresh();
            });
            toggle_Live.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.LiveTalk;
                Refresh();
            });
            toggle_Other.onValueChanged.AddListener((value) =>
            {
                if (!value) return;
                currentStoryType = StoryType.OtherStory;
                Refresh();
            });

            Refresh();
        }

        private void Refresh()
        {
            buttonGenerator.ClearButtons();

            List<StoryManager> storyManagers = stories[currentStoryType];
            buttonGenerator.Generate(storyManagers.Count, (btn, id) =>
            {
                Text text = btn.GetComponentInChildren<Text>();
                text.text = storyManagers[id].description;
            },
            (id) =>
            {
                OnClick(storyManagers[id]);
            });
        }

        protected abstract void OnClick(StoryManager storyManager);
    }
}