using SekaiTools.DecompiledClass;
using SekaiTools.UI.CoupleWithIndexSelector;
using SekaiTools.UI.CutinSceneEditorInitialize;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class GIP_KizunaSceneCreate_CutinData : GIP_CSDSaveData
    {
        [Header("Components")]
        public UniversalGenerator2D universalGenerator2D;

        Vector2Int[] selectedCouple = null;

        public void Initialize(Vector2Int[] selectedCouple)
        {
            this.selectedCouple = selectedCouple;
            CoupleWithIndexStatus createdData = CoupleWithIndexSelector.CoupleWithIndexSelector.GetEmptyMatrix(MATRIX_SIZE);
            ApplyStatusMask(createdData);
            ApplyCreatedData(createdData);
            onDataChange.AddListener(() => RefreshCoupleDisplay());
            RefreshCoupleDisplay();
        }

        void RefreshCoupleDisplay()
        {
            universalGenerator2D.ClearItems();
            Dictionary<Vector2Int, int> dictionary = null;
            if (IfNewFile)
            {
                if (CreatedData!=null)
                {
                    dictionary = GetCoupleDisplay_CreatedData();
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(file_LoadData.SelectedPath))
                {
                    dictionary = GetCoupleDisplay_LoadData();
                }
            }
            if(dictionary!=null)
            {
                List<KeyValuePair<Vector2Int, int>> keyValuePairs = new List<KeyValuePair<Vector2Int, int>>();
                foreach (KeyValuePair<Vector2Int, int> keyValuePair in dictionary)
                {
                    keyValuePairs.Add(keyValuePair);
                }
                universalGenerator2D.Generate(keyValuePairs.Count,
                    (gobj,id)=>
                    {
                        BondsHonorSubWithText coupleDisplayItem = gobj.GetComponent<BondsHonorSubWithText>();
                        coupleDisplayItem.bondsHonorSub.SetCharacter(keyValuePairs[id].Key.x, keyValuePairs[id].Key.y);
                        coupleDisplayItem.txtCount.text = $"{keyValuePairs[id].Value} 互动语音";
                    });
            }
        }

        Dictionary<Vector2Int, int> GetCoupleDisplay_CreatedData()
        {
            Dictionary<Vector2Int, int> dictionary = GetEmptyDictionary();
            CoupleWithIndexStatus createdData = CreatedData;
            for (int i = 0; i < createdData.Rows.Length; i++)
            {
                if (createdData.Rows[i] == null) continue;
                for (int j = 0; j < createdData.Rows[i].Items.Length; j++)
                {
                    if (createdData.Rows[i].Items[j] == null) continue;
                    foreach (var selectStatus in createdData.Rows[i].Items[j])
                    {
                        if(selectStatus == SelectStatus.Checked)
                        {
                            Vector2Int keyIJ = new Vector2Int(i, j);
                            if (dictionary.ContainsKey(keyIJ))
                                dictionary[keyIJ] = dictionary[keyIJ] + 1;
                            Vector2Int keyJI = new Vector2Int(j, i);
                            if (dictionary.ContainsKey(keyJI))
                                dictionary[keyJI] = dictionary[keyJI] + 1;
                        }
                    }
                }
            }
            return dictionary;
        }

        Dictionary<Vector2Int, int> GetCoupleDisplay_LoadData()
        {
            Dictionary<Vector2Int, int> dictionary = GetEmptyDictionary();
            string filePath = file_LoadData.SelectedPath;
            string data = File.ReadAllText(filePath);
            Cutin.CutinSceneData cutinSceneData = Cutin.CutinSceneData.LoadData(data, filePath);
            foreach (var cutinScene in cutinSceneData.cutinScenes)
            {
                Vector2Int keyIJ = new Vector2Int(cutinScene.charFirstID, cutinScene.charSecondID);
                if (dictionary.ContainsKey(keyIJ))
                    dictionary[keyIJ] = dictionary[keyIJ] + 1;
                Vector2Int keyJI = new Vector2Int(cutinScene.charSecondID, cutinScene.charFirstID);
                    dictionary[keyJI] = dictionary[keyJI] + 1;
            }
            return dictionary;
        }

        Dictionary<Vector2Int, int> GetEmptyDictionary()
        {
            Dictionary<Vector2Int, int> dictionary = new Dictionary<Vector2Int, int>();
            foreach (var vector2Int in selectedCouple)
            {
                dictionary[vector2Int] = 0;
            }
            return dictionary;
        }

        public void SelectClipAll_Kizuna()
        {
            CoupleWithIndexSelector.CoupleWithIndexSelector coupleWithIndexSelector
                = WindowController.CurrentWindow.OpenWindow<CoupleWithIndexSelector.CoupleWithIndexSelector>(selectorPrefab);
            if (CreatedData == null)
            {
                CoupleWithIndexStatus coupleWithIndexStatus = CoupleWithIndexSelector.CoupleWithIndexSelector.GetEmptyMatrix(MATRIX_SIZE);
                ApplyStatusMask(coupleWithIndexStatus);
                coupleWithIndexSelector.Initialize(coupleWithIndexStatus, ApplyCreatedData);
            }
            else
            {
                CoupleWithIndexStatus tempStatus = CreatedData.DeepClone();
                tempStatus.RemoveMask();
                ApplyStatusMask(tempStatus);
                coupleWithIndexSelector.Initialize(tempStatus, ApplyCreatedData);
            }
        }

        public void SelectClipLimted_Kizuna()
        {
            WindowController.ShowMasterRefCheck(
                new string[] { "ingameCutinCharacters" },
                () =>
                {
                    MasterIngameCutinCharacters[] masterIngameCutinCharacters
                        = EnvPath.GetTable<MasterIngameCutinCharacters>("ingameCutinCharacters");
                    CoupleWithIndexStatus coupleWithIndexStatus = GetCoupleWithIndexStatus(
                        masterIngameCutinCharacters.Select((micc) => micc.assetbundleName1));


                    CoupleWithIndexSelector.CoupleWithIndexSelector coupleWithIndexSelector
                        = WindowController.CurrentWindow.OpenWindow<CoupleWithIndexSelector.CoupleWithIndexSelector>(selectorPrefab);
                    if (CreatedData == null)
                    {
                        ApplyStatusMask(coupleWithIndexStatus);
                        coupleWithIndexSelector.Initialize(coupleWithIndexStatus, ApplyCreatedData);
                    }
                    else
                    {
                        CoupleWithIndexStatus tempStatus = CreatedData.DeepClone();
                        tempStatus.ApplyMask(coupleWithIndexStatus);
                        ApplyStatusMask(coupleWithIndexStatus);
                        coupleWithIndexSelector.Initialize(tempStatus, ApplyCreatedData);
                    }
                });
        }

        void ApplyStatusMask(CoupleWithIndexStatus coupleWithIndexStatus)
        {
            HashSet<Vector2Int> hasCouples = new HashSet<Vector2Int>(selectedCouple);
            for (int i = 0; i < coupleWithIndexStatus.Rows.Length; i++)
            {
                CoupleMatrixRow<SelectStatus[]> coupleMatrixRow = coupleWithIndexStatus.Rows[i];
                if (coupleMatrixRow == null) continue;
                for (int j = 0; j < coupleMatrixRow.Items.Length; j++)
                {
                    SelectStatus[] selectStatuses = coupleMatrixRow.Items[j];
                    if (selectStatuses == null) continue;
                    if(!(selectedCouple.Contains(new Vector2Int(i,j))||selectedCouple.Contains(new Vector2Int(j,i))))
                    {
                        for (int k = 0; k < selectStatuses.Length; k++)
                        {
                            selectStatuses[k] = SelectStatus.Unavailable;
                        }
                    }
                }
            }
        }

        protected override List<string> GetErrorList()
        {
            List<string> errorList = new List<string>();
            if (IfNewFile && CreatedData == null)
            {
                errorList.Add("未选择片段");
            }

            if (!IfNewFile)
            {
                if (string.IsNullOrEmpty(file_LoadData.SelectedPath))
                    errorList.Add("无效的目录");
                if (!File.Exists(file_LoadData.SelectedPath))
                    errorList.Add("文件不存在");
            }
            return errorList;
        }
    }
}
