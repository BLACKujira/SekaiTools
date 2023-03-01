using SekaiTools.Live2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.L2DModelPreview
{
    public class L2DModelPreview_AnimationArea : MonoBehaviour
    {
        public L2DModelPreview l2DModelPreview;
        [Header("Components")]
        public Text txtFacial;
        public Text txtMotion;
        public Text txtVoice;
        public Button btnPlayVoice;
        public GameObject gobjAnimationArea;
        public GameObject gobjTipArea;
        [Header("Prefab")]
        public Window facialSelectWindowPrefab;
        public Window motionSelectWindowPrefab;

        string facialName;
        string motionName;
        AudioData audioData;

        public string FacialName => facialName;
        public string MotionName => motionName;
        public Window window => l2DModelPreview.window;
        public SekaiLive2DModel Model => l2DModelPreview.l2DController.model;

        public event Action<string> OnPlayFacial;
        public event Action<string> OnPlayMotion;
        public event Action<AudioClip> OnPlayVoice;

        public void Initialize()
        {
            ResetFacial();
            ResetMotion();
            ResetVoice();
        }

        public void PlayFacial()
        {
            Model.PlayAnimation(null, facialName);
            if(OnPlayFacial!=null) 
                OnPlayFacial(facialName);
        }

        public void PlayMotion()
        {
            Model.PlayAnimation(motionName, null);
            if (OnPlayMotion != null)
                OnPlayMotion(motionName);
        }

        public void PlayVoice()
        {
            if (audioData != null)
            {
                Model.PlayVoice(audioData.ValueArray[0]);
                if(OnPlayVoice!=null) 
                    OnPlayVoice(audioData.ValueArray[0]);
            }
        }

        public void PlayAll()
        {
            //Model.PlayAnimation(motionName, facialName);
            PlayFacial();
            PlayMotion();
            PlayVoice();
        }

        public void PlayAllSync()
        {
            Model.PlayAnimation(MotionName,null);
            AnimationClip animationClip = Model.AnimationSet.GetAnimation(MotionName);
            if(animationClip)
            {
                StopAllCoroutines();
                l2DModelPreview.StartCoroutine(CoPlayFacial(animationClip.length, FacialName));
            }
        }

        public void SelectFacial()
        {
            Live2DFacialSelect.Live2DFacialSelect live2DFacialSelect = window.OpenWindow<Live2DFacialSelect.Live2DFacialSelect>(facialSelectWindowPrefab);
            live2DFacialSelect.Initialize(Model.AnimationSet, 
                (value) =>
                {
                    facialName = value;
                    Refresh();
                });
        }

        public void SelectMotion()
        {
            Live2DMotionSelect.Live2DMotionSelect live2DMotionSelect = window.OpenWindow<Live2DMotionSelect.Live2DMotionSelect>(motionSelectWindowPrefab);
            live2DMotionSelect.Initialize(Model.AnimationSet,
                (value) =>
                {
                    motionName = value;
                    Refresh();
                });
            live2DMotionSelect.SetDefaultPage(MotionName);
        }

        public void SelectVoice()
        {
            OpenFileDialog openFileDialog = FileDialogFactory.GetOpenFileDialog(FileDialogFactory.FILTER_AUDIO);
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK) return;
            string selectedFile = openFileDialog.FileName;
            AudioData audioData = new AudioData();
            this.audioData = audioData;
            WindowController.ShowNowLoadingCenter("读取音频资料中",audioData.LoadFile(selectedFile)).OnFinish +=
                Refresh;
        }

        public void ResetFacial()
        {
            facialName = null;
            Refresh();
        }

        public void ResetMotion()
        {
            motionName = null;
            Refresh();
        }

        public void ResetVoice()
        {
            if (Model != null)
                Model.PlayVoice(null);
            audioData = null;
            Refresh();
        }

        public void ResetAll()
        {
            ResetFacial();
            ResetMotion();
            ResetVoice();
        }

        void Refresh()
        {
            if (Model == null || Model.AnimationSet == null)
            {
                gobjAnimationArea.SetActive(false);
                gobjTipArea.SetActive(true);
            }
            else
            {
                gobjAnimationArea.SetActive(true);
                gobjTipArea.SetActive(false);
                txtFacial.text = string.IsNullOrEmpty(facialName) ? "无表情" : facialName;
                txtMotion.text = string.IsNullOrEmpty(motionName) ? "无动作" : motionName;
                if (audioData == null)
                {
                    txtVoice.text = "无语音";
                    btnPlayVoice.interactable = false;
                }
                else
                {
                    txtVoice.text = audioData.ValueArray[0].name;
                    btnPlayVoice.interactable = true;
                }
            }
        }

        IEnumerator CoPlayFacial(float motionLength,string facialName)
        {
            yield return new WaitForSeconds(motionLength);
            Model.PlayAnimation(null,facialName);
        }
    }
}