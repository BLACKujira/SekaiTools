using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public abstract class RadioCommandinput_BilibiliUtilitiesMutiBase : RadioCommandinput_Base
    {
        public RadioCommandinput_BilibiliUtilities_Instance instancePrefab;

        protected RadioCommandinput_BilibiliUtilities_Instance[] instances;
        protected float survivalTimeThreshold = 10;
        protected float autoReconnectTime = 15 * 60;

        protected Dictionary<Danmaku, DanmakuMeta> danamkuDictionary = new Dictionary<Danmaku, DanmakuMeta>();
        protected List<int> sourceList;

        protected bool ready = false;

        public struct Danmaku
        {
            public string userName;
            public string content;

            public Danmaku(string userName, string content)
            {
                this.userName = userName;
                this.content = content;
            }

            public static bool operator ==(Danmaku a, Danmaku b)
            {
                return a.userName.Equals(b.userName) && a.content.Equals(b.content);
            }

            public static bool operator !=(Danmaku a, Danmaku b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                if (obj is Danmaku)
                {
                    Danmaku danmaku = (Danmaku)obj;
                    if (danmaku.userName.Equals(userName) && danmaku.content.Equals(content))
                        return true;
                    return false;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return $"{userName}\n{content}".GetHashCode();
            }
        }

        public class DanmakuMeta
        {
            public HashSet<int> sourcesNotReceived;
            public float receivedTime;

            public float survivalTime => Time.time - receivedTime;
            public bool allSourcesReceived => sourcesNotReceived.Count == 0;

            public DanmakuMeta(List<int> sources, int source)
            {
                sourcesNotReceived = new HashSet<int>(sources);
                sourcesNotReceived.Remove(source);
            }
        }

        public void ReceiveDanmaku(Danmaku danmaku, int sourceId)
        {
            if (!ready)
                return;

            if (danamkuDictionary.ContainsKey(danmaku))
            {
                DanmakuMeta danmakuMeta = danamkuDictionary[danmaku];
                if (danmakuMeta.sourcesNotReceived.Contains(sourceId))
                    danmakuMeta.sourcesNotReceived.Remove(sourceId);
            }
            else
            {
                danamkuDictionary[danmaku] = new DanmakuMeta(sourceList, sourceId);
                if (danmaku.content.StartsWith("/"))
                    radio.ProcessRequest(danmaku.content, danmaku.userName);
            }
            if (danamkuDictionary[danmaku].allSourcesReceived)
            {
                danamkuDictionary.Remove(danmaku);
            }
        }

    }
}