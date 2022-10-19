using SekaiTools.Live2D;
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

        public void Initialize()
        {
            ResetFacial();
            ResetMotion();
            ResetVoice();
        }

        public void PlayFacial()
        {
            Model.PlayAnimation( null, facialName);
        }

        public void PlayMotion()
        {
            Model.PlayAnimation(motionName, null);
        }

        public void PlayVoice()
        {
            if(audioData!=null)
                Model.PlayVoice(audioData.valueArray[0]);
        }

        public void PlayAll()
        {
            Model.PlayAnimation(motionName, facialName);
            PlayVoice();
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
            txtFacial.text = string.IsNullOrEmpty(facialName)?"无表情":facialName;
            txtMotion.text = string.IsNullOrEmpty(motionName)?"无动作":motionName;
            if(audioData == null)
            {
                txtVoice.text = "无语音";
                btnPlayVoice.interactable = false;
            }
            else
            {
                txtVoice.text = audioData.valueArray[0].name;
                btnPlayVoice.interactable = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PlayAll();
        }
    }
}