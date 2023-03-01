using SekaiTools.UI.NCErrorDisplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountEditor
{
    public class NicknameCountEditor_AmbiguityArea : MonoBehaviour
    {
        public NicknameCountEditor nicknameCountEditor;
        [Header("Components")]
        public ButtonGenerator buttonGenerator;
        public Text txtCountAll;
        [Header("Prefab")]
        public Window nCESelectorPrefab;
        public Window windowSelectorFullPrefab;
        public Window nCErrorDisplayPrefab;

        public void Refresh()
        {
            Dictionary<string, int> ambiguityNicknameCount = new Dictionary<string, int>();
            foreach (var mat in nicknameCountEditor.CountData.NicknameCountMatrices)
            {
                foreach (var set in mat.ambiguitySerifSets)
                {
                    ambiguityNicknameCount[set.ambiguityRegex] = ambiguityNicknameCount.ContainsKey(set.ambiguityRegex) ?
                        ambiguityNicknameCount[set.ambiguityRegex] + set.matchedIndexes.Count : set.matchedIndexes.Count;
                }
            }
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(ambiguityNicknameCount);

            buttonGenerator.ClearButtons();
            buttonGenerator.Generate(list.Count,
                (btn, id) =>
                {
                    NicknameCountEditor_AmbiguityArea_Item nicknameCountEditor_AmbiguityArea_Item = btn.GetComponent<NicknameCountEditor_AmbiguityArea_Item>();
                    nicknameCountEditor_AmbiguityArea_Item.Initialize(list[id].Key, list[id].Value);
                },
                (id) =>
                {
                    NCESelector.NCESelector nCESelector
                        = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(nCESelectorPrefab);
                    nCESelector.Initialize(nicknameCountEditor.CountData, list[id].Key);
                });

            txtCountAll.text = list.Sum(kvp => kvp.Value).ToString() + "剩余";
        }

        public void OpenEditorFull()
        {
            NCESelector.NCESelector nCESelectorMuti = nicknameCountEditor.window.OpenWindow<NCESelector.NCESelector>(windowSelectorFullPrefab);
            nCESelectorMuti.Initialize(nicknameCountEditor.CountData);
        }

        public void CheckErrors()
        {
            List<NCError> nCErrors = new List<NCError>();
            for (int i = 1; i < 27; i++)
            {
                for (int j = i + 1; j < 27; j++)
                {
                    int countAToB = nicknameCountEditor.CountData[i, j].Total;
                    int countBToA = nicknameCountEditor.CountData[j, i].Total;
                    int smallOne = Mathf.Min(countAToB, countBToA);
                    int bigOne = Mathf.Max(countAToB, countBToA);

                    if (smallOne < 3 && bigOne > 10)
                    {
                        nCErrors.Add(new NCError(i, j, countAToB, countBToA, "一方小于3，而另一方大于10"));
                    }
                    else if (smallOne > 10 && smallOne * 3 < bigOne)
                    {
                        nCErrors.Add(new NCError(i, j, countAToB, countBToA, "两方均大于10，但一方大于另一方的三倍"));
                    }
                }
            }

            NCErrorDisplay.NCErrorDisplay nCErrorDisplay
                = nicknameCountEditor.window.OpenWindow<NCErrorDisplay.NCErrorDisplay>(nCErrorDisplayPrefab);
            nCErrorDisplay.Initialize(nCErrors.ToArray());
        }
    }
}