using SekaiTools.Count;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.DynamicBarChart
{
    public abstract class DataFrame
    {
        public DateTime DateTime => nicknameCountMatrix.PublishedAt;
        public string EventGroup => eventGroup;
        public Dictionary<string, float> data;

        public NicknameCountMatrix nicknameCountMatrix;
        public string eventGroup;

        public DataFrame(Dictionary<string, float> data, NicknameCountMatrix nicknameCountMatrix, string eventGroup)
        {
            this.data = data;
            this.nicknameCountMatrix = nicknameCountMatrix;
            this.eventGroup = eventGroup;
        }
    }
}