using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Live2D;
using SekaiTools.Count;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_CountIntAll : NCSScene , IAudioFileReference , ILive2DReferenceCharacter
    {
        [Header("Components")]
        public L2DControllerTypeB l2DController;
        [Header("Text")]
        public Text[] countTextCharacters = new Text[27];
        public Text countTextTotal;
        [Header("Settings")]
        public Vector2 modelPosition = new Vector2(-5.7f,-2.9f);
        public float modelScale = 14.5f;

        [System.NonSerialized] string audioClipName = null;
        [System.NonSerialized] int talkerId = 1;
        [System.NonSerialized] float voiceDelay = 1;
        [System.NonSerialized] string facialName = string.Empty;
        [System.NonSerialized] string motionName = string.Empty;
        [System.NonSerialized] AudioData audioData;


        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
        {
            new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
            new ConfigUIItem_Character("角色","scene",()=>talkerId,(value)=>talkerId = value),
            new ConfigUIItem_AudioFile("语音","audio",
                ()=>player.audioData.GetValue(audioClipName),
                (value)=>
                {
                    audioData = value;
                    audioClipName = audioData.valueArray[0].name;
                }),
            new ConfigUIItem_Float("语音起始时间","audio",()=>voiceDelay,(value)=>voiceDelay = value),
            new ConfigUIItem_Live2dModel("模型","live2d",
                ()=>l2DController.live2DModels[talkerId],
                (value)=>l2DController.SetModel(value,talkerId)),
            new ConfigUIItem_Live2dAnimation("Live2d动画","live2d",
                ()=>l2DController.live2DModels[talkerId] == null ? null : l2DController.live2DModels[talkerId].AnimationSet,
                ()=>facialName,
                (value)=>facialName = value,
                ()=>motionName,
                (value)=>motionName = value)
        };

        public override string information => $@"角色 {ConstData.characters[talkerId].Name} , 持续时间 {holdTime.ToString("0.00")} , 
表情 {(string.IsNullOrEmpty(facialName)?"未设置":facialName)} , 动作 {(string.IsNullOrEmpty(motionName)? "未设置":motionName)} , 语音 {(audioClipName)}";

        KeyValuePair<AudioClip, string>[] IAudioFileReference.audioFiles
        {
            get
            {
                if (string.IsNullOrEmpty(audioClipName)) return new KeyValuePair<AudioClip, string>[0];
                KeyValuePair<AudioClip, string>[] keyValuePairs = new KeyValuePair<AudioClip, string>[1];
                if (audioData != null) keyValuePairs[0] = audioData.valuePathPairArray[0];
                else keyValuePairs[0] = new KeyValuePair<AudioClip, string>(player.audioData.GetValue(audioClipName), player.audioData.GetSavePath(player.audioData.GetValue(audioClipName)));
                return keyValuePairs;
            }
        }

        public int l2dReferenceCharacterID => talkerId;

        public override void Initialize(NCSPlayerBase player)
        {
            base.Initialize(player);
            l2DController.live2DModels = player.live2DModels;
        }

        public override void LoadData(string serialisedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serialisedData);
            audioClipName = settings.voiceName;
            talkerId = settings.talkerId;
            voiceDelay = settings.voiceDelay;
            facialName = settings.facialName;
            motionName = settings.motionName;
        }

        public override void Refresh()
        {
            StopAllCoroutines();
            l2DController.HideModelAll();
            for (int i = 1; i < 27; i++)
            {
                if (i == talkerId)
                {
                    countTextCharacters[i].text = "-";
                }
                else
                {
                    NicknameCountItem nicknameCountItem = countData[talkerId, i];
                    countTextCharacters[i].text = nicknameCountItem.Total.ToString();
                }
            }
            countTextTotal.text = $"共计 {countData.GetCountTotal(talkerId,true)} 次 ，在 {countData.GetSerifCount(talkerId)} 句台词中";
            if (l2DController.live2DModels[talkerId])
            {
                l2DController.ShowModelLeft((Character)talkerId);
                l2DController.SetModelPositionLeft(modelPosition);
                l2DController.modelL.transform.localScale = new Vector3(modelScale,modelScale,1);
                l2DController.FadeInLeft();
                AudioClip audioClip;
                if (audioData != null) audioClip = this.audioData.valueArray[0];
                else audioClip = player.audioData.GetValue(audioClipName);
                if (audioClip)
                    StartCoroutine(IPlayVoice(audioClip));
                l2DController.modelL.PlayAnimation(motionName, facialName);
            }
        }

        IEnumerator IPlayVoice(AudioClip audioClip)
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

            public Settings(NCSScene_CountIntAll nCSScene)
            {
                holdTime = nCSScene.holdTime;
                talkerId = nCSScene.talkerId;
                facialName = nCSScene.facialName;
                motionName = nCSScene.motionName;
                voiceName = nCSScene.audioData == null ?nCSScene.audioClipName: nCSScene.audioData.valueArray[0].name;
                voiceDelay = nCSScene.voiceDelay;
            }

        }
        private void OnDisable()
        {
            if (l2DController) l2DController.HideModelAll();
        }
    }
    public interface IAudioFileReference
    {
        KeyValuePair<AudioClip, string>[] audioFiles { get; }
    }

    public interface ILive2DReferenceCharacter
    {
        int l2dReferenceCharacterID { get; }
    }
}