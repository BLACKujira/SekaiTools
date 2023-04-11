using SekaiTools.Count;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public abstract class DataFrame
    {
        public string Name => storyDescriptionGetter.GetStroyDescription(nicknameCountMatrix.storyType, nicknameCountMatrix.fileName);
        public DateTime DateTime => nicknameCountMatrix.PublishedAt;
        public string EventGroup => eventGroup;
        public Dictionary<string, float> data;

        public NicknameCountMatrix nicknameCountMatrix;
        public StoryDescriptionGetter storyDescriptionGetter;
        public string eventGroup;

        public DataFrame(Dictionary<string, float> data, NicknameCountMatrix nicknameCountMatrix, StoryDescriptionGetter storyDescriptionGetter, string eventGroup)
        {
            this.data = data;
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.storyDescriptionGetter = storyDescriptionGetter;
            this.eventGroup = eventGroup;
        }
    }
}