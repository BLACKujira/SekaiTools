using SekaiTools.Live2D;
using SekaiTools.UI.L2DModelSelect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public abstract class NCSScene_CountIntAllBase : NCSScene, IAudioFileReference, ILive2DReferenceCharacter
    {
        [Header("Components")]
        public L2DControllerTypeB l2DController;
        [Header("Settings")]
        public Vector2 modelPosition = new Vector2(-5.7f, -2.9f);
        public float modelScale = 14.5f;

        [System.NonSerialized] protected string audioClipName = null;
        [System.NonSerialized] protected int talkerId = 1;
        [System.NonSerialized] protected float voiceDelay = 1;
        [System.NonSerialized] protected string facialName = string.Empty;
        [System.NonSerialized] protected string motionName = string.Empty;
        [System.NonSerialized] protected AudioData audioData;


        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
        {
            new ConfigUIItem_Float("持续时间","场景",()=>holdTime,(value)=>holdTime = value),
            new ConfigUIItem_Character("角色","场景",()=>talkerId,(value)=>talkerId = value),
            new ConfigUIItem_AudioFile("语音","声音",
                ()=>player.audioData.GetValue(audioClipName),
                (value)=>
                {
                    audioData = value;
                    audioClipName = audioData.ValueArray[0].name;
                    player.audioData.Append(audioData);
                }),
            new ConfigUIItem_Float("语音起始时间","声音",()=>voiceDelay,(value)=>voiceDelay = value),
            new ConfigUIItem_Live2dModel("模型","live2d",
                ()=>l2DController.live2DModels[talkerId]==null?SelectedModelInfo.Empty:l2DController.live2DModels[talkerId].selectedModelInfo,
                (value)=>L2DModelLoader.LoadModel(value,(model)=> l2DController.SetModel(model,talkerId))),
            new ConfigUIItem_Live2dAnimation("Live2d动画","live2d动画",
                ()=>l2DController.live2DModels[talkerId] == null ? null : l2DController.live2DModels[talkerId].AnimationSet,
                ()=>facialName,
                (value)=>facialName = value,
                ()=>motionName,
                (value)=>motionName = value)
        };

        public override string Information => $@"角色 {ConstData.characters[talkerId].Name} , 持续时间 {holdTime.ToString("0.00")} , 
表情 {(string.IsNullOrEmpty(facialName) ? "未设置" : facialName)} , 动作 {(string.IsNullOrEmpty(motionName) ? "未设置" : motionName)} , 语音 {(audioClipName)}";

        public HashSet<string> RequireAudioKeys
        {
            get
            {
                if (string.IsNullOrEmpty(audioClipName)) return new HashSet<string>();
                HashSet<string> requireAudioKeys = new HashSet<string>();
                if (audioData != null) requireAudioKeys.Add(audioData.ValuePathPairArray[0].Key.name);
                else requireAudioKeys.Add(audioClipName);
                return requireAudioKeys;
            }
        }

        public int l2dReferenceCharacterID => talkerId;

        public override void Initialize(NCSPlayerBase player)
        {
            base.Initialize(player);
            l2DController.live2DModels = player.live2DModels;
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            audioClipName = settings.voiceName;
            talkerId = settings.talkerId;
            voiceDelay = settings.voiceDelay;
            facialName = settings.facialName;
            motionName = settings.motionName;
        }

        protected IEnumerator IPlayVoice(AudioClip audioClip)
        {
            yield return new WaitForSeconds(voiceDelay);
            l2DController.modelL.PlayVoice(audioClip);
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int talkerId;
            public string facialName;
            public string motionName;
            public string voiceName;
            public float voiceDelay;

            public Settings(NCSScene_CountIntAllBase nCSScene)
            {
                holdTime = nCSScene.holdTime;
                talkerId = nCSScene.talkerId;
                facialName = nCSScene.facialName;
                motionName = nCSScene.motionName;
                voiceName = nCSScene.audioData == null ? nCSScene.audioClipName : nCSScene.audioData.ValueArray[0].name;
                voiceDelay = nCSScene.voiceDelay;
            }

        }
        private void OnDisable()
        {
            if (l2DController) l2DController.HideModelAll();
        }
    }
}