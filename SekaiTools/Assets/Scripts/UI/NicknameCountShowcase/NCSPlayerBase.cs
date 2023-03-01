using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SekaiTools.Count;
using System;
using SekaiTools.DecompiledClass;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSPlayerBase : MonoBehaviour
    {
        [NonSerialized] public NicknameCountData countData;
        [NonSerialized] public Count.Showcase.NicknameCountShowcase showcase;
        [NonSerialized] public SekaiLive2DModel[] live2DModels = new SekaiLive2DModel[57];
        [NonSerialized] public AudioData audioData;
        [NonSerialized] public ImageData imageData;

        [NonSerialized] public MasterEvent[] events;
        [NonSerialized] public MasterCard[] cards;

        public static HashSet<string> RequireMasterTables = new HashSet<string>()
        {
            "events","cards"
        };

        public virtual void Initialize(Settings settings)
        {
            countData = settings.countData;
            audioData = settings.audioData;
            imageData = settings.imageData;
            live2DModels = settings.live2DModels;
            showcase = settings.showcase;
            
            events = EnvPath.GetTable<MasterEvent>("events");
            cards = EnvPath.GetTable<MasterCard>("cards");

            foreach (var scene in showcase.scenes) 
            {
                scene.nCSScene.Initialize(this);
                scene.nCSScene.gameObject.SetActive(true);
                scene.nCSScene.gameObject.SetActive(false);
            }
        }

        public class Settings
        {
            public NicknameCountData countData;
            public Count.Showcase.NicknameCountShowcase showcase;
            public SekaiLive2DModel[] live2DModels = new SekaiLive2DModel[57];
            public AudioData audioData;
            public ImageData imageData;
        }

        private void OnDestroy()
        {
            foreach (var sekaiLive2DModel in live2DModels)
            {
                if(sekaiLive2DModel!=null)
                    Destroy(sekaiLive2DModel.gameObject);
            }
        }
    }
}