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

        public HashSet<int> highLightRefIdx = new HashSet<int>();

        public void InitInfo(StoryDescriptionGetter descriptionGetter, StoryPublishTimeGetter publishTimeGetter, SVStoryUrlGetter urlGetter)
        {
            publishedAt = publishTimeGetter.GetStoryPublishTime(storyType, fileName);
            description = descriptionGetter.GetStroyDescription(storyType, fileName);
            sekaiViewerUrl = urlGetter.GetUrl(storyType, fileName);
        }

        public DateTime PublishedAt => ExtensionTools.UnixTimeMSToDateTimeTST(publishedAt);
        public abstract BaseTalkData[] GetTalkDatas();
    }
}