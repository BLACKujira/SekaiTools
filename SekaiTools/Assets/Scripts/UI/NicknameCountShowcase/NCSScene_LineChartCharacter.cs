using SekaiTools.Effect;
using SekaiTools.UI.DynamicBarChart;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_LineChartCharacter : NCSScene
    {
        [Header("Components")]
        public LineChart lineChart;
        public HDRColorGraphic[] charColorGraphics;
        public HDRUIController[] charColorControllers;
        public RectTransform legendParent;
        [Header("Settings")]
        public IconSet smallIconSet;
        public HDRColorSet hDRColorSet;
        public float legendDistance;
        public Vector2 particleInstantiatePosition;
        [Header("Prefab")]
        public NCSScene_LineChartCharacter_Legend legendPrefab;

        int characterId = 1;
        public override string Information => $@"角色 {ConstData.characters[characterId].Name}";

        public override ConfigUIItem[] configUIItems => new ConfigUIItem[]
            {
                new ConfigUIItem_Float("持续时间","场景",()=>holdTime,(value)=>holdTime = value),
                new ConfigUIItem_Character("角色","场景",()=>characterId,(value)=>characterId = value)
            };

        List<NCSScene_LineChartCharacter_Legend> legends = new List<NCSScene_LineChartCharacter_Legend>();

        public override void Refresh()
        {
            List<DataFrameCharacter> dataFrameCharacters = DynamicBarChartCharacter.GetDataFrameCharacter(countData, characterId, player);
            int itemNumber = lineChart.series.Count;

            DataFrameCharacter lastFrame = dataFrameCharacters[dataFrameCharacters.Count - 1];
            KeyValuePair<string, float>[] selectedData = lastFrame.data
                .OrderBy(kvp => -kvp.Value)
                .Take(Mathf.Min(lastFrame.data.Count, itemNumber))
                .ToArray();

            XAxis xAxis = lineChart.GetChartComponent<XAxis>();
            xAxis.max = dataFrameCharacters.Count;

            YAxis yAxis = lineChart.GetChartComponent<YAxis>();
            yAxis.max = selectedData[0].Value;

            for (int i = 0; i < lineChart.series.Count; i++)
            {
                lineChart.series[i].data.Clear();
                int charBId = int.Parse(selectedData[i].Key.Split('_')[1]);
                lineChart.series[i].lineStyle.color = ConstData.characters[charBId].imageColor;
                lineChart.series[i].endLabel.icon.sprite = smallIconSet.icons[charBId];
            }

            for (int dataFrameIndex = 0; dataFrameIndex < dataFrameCharacters.Count; dataFrameIndex++)
            {
                DataFrameCharacter dataFrameCharacter = dataFrameCharacters[dataFrameIndex];
                for (int i = 0; i < lineChart.series.Count; i++)
                {
                    float dataY = 0;
                    if (dataFrameCharacters[dataFrameIndex].data.ContainsKey(selectedData[i].Key))
                    {
                        dataY = dataFrameCharacters[dataFrameIndex].data[selectedData[i].Key];
                    }
                    lineChart.series[i].AddXYData(dataFrameIndex, dataY);

                    if (i == 0) lineChart.series[0].AddXYData(dataFrameIndex, dataY);
                }
            }

            lineChart.RebuildChartObject();

            //添加图示
            foreach (var legend in legends)
            {
                Destroy(legend.gameObject);
            }
            legends = new List<NCSScene_LineChartCharacter_Legend>();
            float legendStartPos = (selectedData.Length - 1) * legendDistance;
            legendStartPos /= 2;
            legendStartPos = -legendStartPos;
            for (int i = 0; i < selectedData.Length; i++)
            {
                int charId = int.Parse(selectedData[i].Key.Split('_')[1]);
                legendPrefab.particleController.instantiatePosition = particleInstantiatePosition + new Vector2(i * legendDistance + legendStartPos, 0);
                NCSScene_LineChartCharacter_Legend legend = Instantiate(legendPrefab, legendParent);
                legend.RectTransform.anchoredPosition =new Vector2(i * legendDistance + legendStartPos, legend.RectTransform.anchoredPosition.y);
                legend.SetCharacter(charId, characterId);
                legends.Add(legend);
            }

            //改变颜色
            foreach (var hDRColorParticle in charColorGraphics)
            {
                hDRColorParticle.hDRColor = hDRColorSet.colors[characterId];
            }
            foreach (var hDRUIController in charColorControllers)
            {
                HDRColorParticle hDRColorParticle = hDRUIController.InstantiateObject.GetComponent<HDRColorParticle>();
                hDRColorParticle.hDRColor = hDRColorSet.colors[characterId];
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
            characterId = settings.characterId;
        }

        [System.Serializable]
        public class Settings
        {
            public float holdTime;
            public int characterId;

            public Settings(NCSScene_LineChartCharacter nCSScene_LineChartCharacter)
            {
                holdTime = nCSScene_LineChartCharacter.holdTime;
                characterId = nCSScene_LineChartCharacter.characterId;
            }
        }
    }
}