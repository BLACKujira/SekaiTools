using SekaiTools.Count;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    public abstract class StoryManager : MonoBehaviour
    {
        public string filePath;
        public string fileName;
        public StoryType storyType;
        public long publishedAt;
        public string description;
        public string sekaiViewerUrl;

        bool initialized = false;
        public bool Initialized => initialized;

        public HashSet<int> highLightRefIdx = new HashSet<int>();

        public void InitInfo(StoryDescriptionGetter descriptionGetter, StoryPublishTimeGetter publishTimeGetter, SVStoryUrlGetter urlGetter)
        {
            if (Initialized) return;

            publishedAt = publishTimeGetter.GetStoryPublishTime(storyType, fileName);
            description = descriptionGetter.GetStroyDescription(storyType, fileName);
            sekaiViewerUrl = urlGetter.GetUrl(storyType, fileName);

            initialized = true;
        }

        public DateTime PublishedAt => ExtensionTools.UnixTimeMSToDateTimeTST(publishedAt);
        public abstract BaseTalkData[] GetTalkDatas();
    }
}