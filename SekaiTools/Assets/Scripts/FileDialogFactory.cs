using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;

namespace SekaiTools
{
    public static class FileDialogFactory
    {
        private const string audioDataFile = "音频资料 (*.aud)|*.aud|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_AudioData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择音频资料";
            openFileDialog.Filter = audioDataFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_AudioData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存音频资料";
            saveFileDialog.Filter = audioDataFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        private const string imageDataFile = "图像资料 (*.imd)|*.imd|Others (*.*)|*.*";

        public static OpenFileDialog GetOpenFileDialog_ImageData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择图像资料";
            openFileDialog.Filter = imageDataFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_ImageData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存图像资料";
            saveFileDialog.Filter = imageDataFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        private const string cutinSceneDataFile = "互动语音场景资料 (*.csd)|*.csd|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_CutinSceneData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择互动语音场景资料";
            openFileDialog.Filter = cutinSceneDataFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_CutinSceneData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存互动语音场景资料";
            saveFileDialog.Filter = cutinSceneDataFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        private const string kizunaSceneDataFile = "牵绊场景资料 (*.kzn)|*.kzn|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_KizunaSceneData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择牵绊场景资料";
            openFileDialog.Filter = kizunaSceneDataFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_KizunaSceneData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存牵绊场景资料";
            saveFileDialog.Filter = kizunaSceneDataFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }
  
        private const string imageFile = "Image |*.png;*.jpg|Others (*.*)|*.*";
        private const string pngFile = "png |*.png";
        private const string gifFile = "gif |*.gif";
        public static OpenFileDialog GetOpenFileDialog_Image()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择图像";
            openFileDialog.Filter = imageFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_PNG()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存图像";
            saveFileDialog.Filter = pngFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_GIF()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存图像";
            saveFileDialog.Filter = gifFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        private const string backGroundSettingsFile = "背景设置 (*.bgs)|*.bgs|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_BGSettings()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择背景设置";
            openFileDialog.Filter = backGroundSettingsFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog_BGSettings()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存背景设置";
            saveFileDialog.Filter = backGroundSettingsFile;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        private const string audioFile = "Audio |*.wav;*.ogg;*.mp3|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_Audio()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择音频文件";
            openFileDialog.Filter = audioFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }

        private const string nicknameCountShowcaseFile = "NicknameCountShowcase (*.ncs)|*.ncs|Others (*.*)|*.*";
        public static OpenFileDialog GetOpenFileDialog_NCS()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = nicknameCountShowcaseFile;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
    }
}