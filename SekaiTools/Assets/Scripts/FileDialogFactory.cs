using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;

namespace SekaiTools
{
    public static class FileDialogFactory
    {
        public static OpenFileDialog GetOpenFileDialog(string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;
            return openFileDialog;
        }
        public static SaveFileDialog GetSaveFileDialog(string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.RestoreDirectory = true;
            return saveFileDialog;
        }

        public const string FILTER_AUD = "音频资料 (*.aud)|*.aud|Others (*.*)|*.*";
        public const string FILTER_IMD= "图像资料 (*.imd)|*.imd|Others (*.*)|*.*";
        public const string FILTER_CSD = "互动语音场景资料 (*.csd)|*.csd|Others (*.*)|*.*";
        public const string FILTER_KZN = "牵绊场景资料 (*.kzn)|*.kzn|Others (*.*)|*.*";
        public const string FILTER_IMAGE = "Image |*.png;*.jpg|Others (*.*)|*.*";
        public const string FILTER_PNG = "png |*.png";
        public const string FILTER_GIF = "gif |*.gif";
        public const string FILTER_BGS = "背景设置 (*.bgs)|*.bgs|Others (*.*)|*.*";
        public const string FILTER_AUDIO = "Audio |*.wav;*.ogg;*.mp3|Others (*.*)|*.*";
        public const string FILTER_NCS = "昵称统计展示 (*.ncs)|*.ncs|Others (*.*)|*.*";
        public const string FILTER_SS = "Spine场景 (*.ss)|*.ss|Others (*.*)|*.*";
        public const string FILTER_SAS = "Spine动画展示场景 (*.sas)|*.sas|Others (*.*)|*.*";
        public const string FILTER_SLS = "系统Live2D展示场景 (*.sls)|*.sls|Others (*.*)|*.*";
        public const string FILTER_TXT = ".txt|*.txt";
        public const string FILTER_CPF = "组合分类存档 (*.cpf)|*.cpf|Others (*.*)|*.*";

    }
}