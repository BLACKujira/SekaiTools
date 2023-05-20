using SekaiTools.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_Calc : NCSScene
    {
        [Header("Components")]
        public Text targetText;
        public HDRColorGraphic hDRColorGraphic;
        [TextArea]
        public string calcText = 
            @"Initializing...

837E 834E 8175 59 82B3 82F1 82CC 82B1 82C6 82CD
82E0 82A4 9659 82EA 82BD 82D9 82A4 82AA
82A2 82A2 82C6 8E76 82A2 82DC 82B7 8176...

838B 834A 8175 918A 95CF 82ED 82E7 82B8
96A2 97FB 82AA 82DC 82B5 82A2 82C8 8176...

8357 837E 8175 92FA 82DF 82AA 88AB 82A2 82E6 8176...

<size=80>Calc.</size>

8357 837E 815B 8175 8163 8163
28 814C 8145
81CD 8145 814D 29
8163 8163 8176...";
        [Header("Settings")]
        public int calcLineIndex = 11;
        public float charWaitTime = 0.03f;
        public float delayBeforeCalcLine = 0.5f;
        public float playDelay = 3;

        public Color TextColor
        {
            get => hDRColorGraphic.hDRColor;
            set => hDRColorGraphic.hDRColor = value;
        }

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","scene",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_HDRColor("文字颜色","scene",()=>hDRColorGraphic.hDRColor,(value)=>hDRColorGraphic.hDRColor = value),
            };

        private void Awake()
        {
            calcText = targetText.text;
        }

        public override void Refresh()
        {
            if (playCoroutine != null) StopCoroutine(playCoroutine);
            if (gameObject.activeSelf) playCoroutine = StartCoroutine(CoPlay());
        }

        Coroutine playCoroutine = null;
        IEnumerator CoPlay()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(charWaitTime);

            string[] lines = calcText.Split('\n');
            List<string> wordsList = new List<string>();

            int calcWordIndex = 0;
            for (int i = 0; i < lines.Length; i++) 
            {
                string line = lines[i];
                line += '\n';
                if(i == calcLineIndex) 
                {
                    calcWordIndex = wordsList.Count;
                    wordsList.Add(line);
                }
                else
                {
                    string[] chars = line.Split(' ');
                    foreach (var charStr in chars)
                    {
                        string addStr = charStr.EndsWith("\n") ? charStr : charStr + ' ';
                        wordsList.Add(addStr);
                    }
                }
            }

            targetText.text = string.Empty;
            targetText.text += wordsList[0];

            yield return new WaitForSeconds(playDelay);

            for (int i = 1; i < wordsList.Count; i++)
            {
                if (i == calcWordIndex - 1) yield return new WaitForSeconds(delayBeforeCalcLine);

                string word = wordsList[i];
                targetText.text += word;
                yield return waitForSeconds;
            }
        }

        public override string GetSaveData()
        {
            return JsonUtility.ToJson(new Settings(this));
        }

        public override void LoadData(string serializedData)
        {
            Settings settings = JsonUtility.FromJson<Settings>(serializedData);
            holdTime = settings.holdTime;
            TextColor = settings.textColor;
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public Color textColor;

            public Settings(NCSScene_Calc nCSScene_Calc)
            {
                holdTime = nCSScene_Calc.holdTime;
                textColor = nCSScene_Calc.TextColor;
            }
        }
    }
}