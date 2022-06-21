using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.GeneralSettingsWindow
{
    public class GSW_Item_AudioFile : GSW_Item
    {
        public Text fileNameText;
        public Button loadFileButton;

        public override void Initialize(ConfigUIItem configUIItem, GeneralSettingsWindow generalSettingsWindow)
        {
            ConfigUIItem_AudioFile configUIItem_AudioFile = configUIItem as ConfigUIItem_AudioFile;
            if (configUIItem_AudioFile == null) throw new ItemTypeMismatchException();
            AudioClip audioClip = configUIItem_AudioFile.getValue();
            fileNameText.text = audioClip == null? "无": audioClip.name;
            loadFileButton.onClick.AddListener(() =>
            {
                FileLoader.LoadAudioData(
                    (audioData) => 
                    {
                        configUIItem_AudioFile.setValue(audioData);
                        fileNameText.text = audioData.valueArray[0].name;
                    });
            });
        }
    }
}