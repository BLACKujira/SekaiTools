using SekaiTools.DecompiledClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    [RequireComponent(typeof(RectTransform))]
    public class Radio_MusicListLayer_Item_VocalItem : MonoBehaviour
    {
        public Text text_VocalType;
        public Text text_VocalSinger;
        public Text text_VocalSize;

        RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (!_rectTransform)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }


        public void Initialize(MusicVocalData musicVocalData)
        {
            string vocalTypeStr;
            switch (musicVocalData.vocalType)
            {
                case MusicVocalType.sekai:
                    vocalTypeStr = "Sekai";
                    break;
                case MusicVocalType.original_song:
                case MusicVocalType.virtual_singer:
                    vocalTypeStr = "Virtual Singer";
                    break;
                case MusicVocalType.another_vocal:
                    vocalTypeStr = "Another Vocal";
                    break;
                case MusicVocalType.instrumental:
                    vocalTypeStr = "Instrumental";
                    break;
                case MusicVocalType.april_fool_2022:
                    vocalTypeStr = "April Fool 2022";
                    break;
                default:
                    vocalTypeStr = "unknown";
                    break;
            }
            text_VocalType.text = vocalTypeStr;
            if (musicVocalData.singers.Length != 0)
            {
                List<string> singerStrs = new List<string>();
                foreach (var singer in musicVocalData.singers)
                {
                    singerStrs.Add(singer.Replace(" ", string.Empty));
                }
                text_VocalSinger.text = $"Vo. {string.Join("¡¢", singerStrs)}";
            }
            else
            {
                text_VocalSinger.text = string.Empty;
            }

            string sizeStr = musicVocalData.musicSize;
            switch (sizeStr)
            {
                case "full":
                    sizeStr = "Full Version";
                    break;
                case "game":
                    sizeStr = "Game Size";
                    break;
                default:
                    break;
            }
            text_VocalSize.text = sizeStr;
        }
    }
}