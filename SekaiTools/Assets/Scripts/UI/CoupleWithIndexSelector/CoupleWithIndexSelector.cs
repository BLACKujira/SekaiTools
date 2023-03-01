using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.CoupleWithIndexSelector
{
    public class CoupleWithIndex : CoupleMatrix<List<int>>
    {
        public CoupleWithIndex(int length) : base(length)
        {
        }
    }

    public enum SelectStatus
    {
        Unchecked,
        Checked,
        Unavailable
    }

    public class CoupleWithIndexSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public ToggleGenerator toggleGenerator;
        public UniversalGenerator universalGeneratorColumn;
        [Header("Settings")]
        public int size = 26;
        public IconSet charIconSet;
        [Header("Prefab")]
        public Window toolSelectPrefab;
        public Window characterSelectorPrefab;

        CoupleWithIndexStatus statusMatrix;
        List<CoupleWithIndexSelector_Item> columnItems;
        List<CoupleWithIndexSelector_Tab> tabItems;
        Action<CoupleWithIndexStatus> onApply;

        int currentCharId = 1;

        public void Initialize(int matrixSize, Action<CoupleWithIndexStatus> onApply = null)
        {
            CoupleWithIndexStatus statusMatrix = GetEmptyMatrix(matrixSize);
            Initialize(statusMatrix, onApply);
        }

        public static CoupleWithIndexStatus GetEmptyMatrix(int matrixSize)
        {
            CoupleWithIndexStatus statusMatrix = new CoupleWithIndexStatus(matrixSize);
            foreach (var row in statusMatrix.Rows)
            {
                for (int i = 0; i < row.Items.Length; i++)
                {
                    row.Items[i] = new SelectStatus[6];
                }
            }

            return statusMatrix;
        }

        bool ifFirstSave = true;
        public void Initialize(CoupleWithIndexStatus statusMatrix, Action<CoupleWithIndexStatus> onApply = null)
        {
            this.statusMatrix = statusMatrix;
            this.onApply = onApply;
            List<int> idsNotEmpty = new List<int>();
            for (int i = 0; i < statusMatrix.Rows.Length; i++)
            {
                CoupleMatrixRow<SelectStatus[]> coupleMatrixRow = statusMatrix.Rows[i];
                if (coupleMatrixRow == null) continue;
                for (int j = 0; j < coupleMatrixRow.Items.Length; j++)
                {
                    bool flag = false;
                    SelectStatus[] selectStatuses = coupleMatrixRow.Items[j];
                    if (selectStatuses == null) continue;
                    foreach (var selectStatus in selectStatuses)
                    {
                        if(selectStatus!=SelectStatus.Unavailable)
                        {
                            idsNotEmpty.Add(i);
                            flag = true;
                            break;
                        }
                    }
                    if (flag) break;
                }
            }
            idsNotEmpty.Remove(0);

            tabItems = new List<CoupleWithIndexSelector_Tab>();
            toggleGenerator.Generate(idsNotEmpty.Count,
                (Toggle toggle, int id) =>
                {
                    CoupleWithIndexSelector_Tab coupleWithIndexSelector_Tab
                        = toggle.GetComponent<CoupleWithIndexSelector_Tab>();
                    int charId = idsNotEmpty[id];
                    coupleWithIndexSelector_Tab.Initialize(charId);
                    coupleWithIndexSelector_Tab.SetCharacter(charId);
                    tabItems.Add(coupleWithIndexSelector_Tab);
                },
                (bool value, int id) =>
                {
                    int charId = idsNotEmpty[id];
                    if (value)
                    {
                        if (ifFirstSave)
                        {
                            ifFirstSave = false;
                        }
                        else
                        {
                            SaveChanges(currentCharId);
                        }
                        currentCharId = charId;
                        RefreshInfo(charId);
                    }
                });
            toggleGenerator.toggles[0].isOn = true;
            RefreshInfo(tabItems[0].Index);
        }

        public void RefreshInfo(int charId)
        {
            columnItems = new List<CoupleWithIndexSelector_Item>();
            universalGeneratorColumn.ClearItems();
            List<int> idsNotEmpty = new List<int>();
            CoupleMatrixRow<SelectStatus[]> coupleMatrixRow = statusMatrix.Rows[charId];
            for (int i = 0; i < coupleMatrixRow.Items.Length; i++)
            {
                if(coupleMatrixRow.Items!=null)
                {
                    SelectStatus[] selectStatuses = coupleMatrixRow.Items[i];
                    foreach (var selectStatus in selectStatuses)
                    {
                        if(selectStatus!=SelectStatus.Unavailable)
                        {
                            idsNotEmpty.Add(i);
                            break;
                        }
                    }
                }
            }
            idsNotEmpty.Remove(0);

            universalGeneratorColumn.Generate(idsNotEmpty.Count,
                (gobj, id) =>
                {
                    CoupleWithIndexSelector_Item coupleWithIndexSelector_Item = gobj.GetComponent<CoupleWithIndexSelector_Item>();
                    int charId2 = idsNotEmpty[id];
                    coupleWithIndexSelector_Item.Initialize(charId2);
                    coupleWithIndexSelector_Item.SetCharacter(charId, charId2);
                    coupleWithIndexSelector_Item.SetStatus(coupleMatrixRow[charId2]);
                    columnItems.Add(coupleWithIndexSelector_Item);
                });
        }
        public void SaveChanges(int charId)
        {
            foreach (var item in columnItems)
            {
                int charId2 = item.Index;
                statusMatrix.Rows[charId].Items[charId2] = item.Status;
            }
        }

        public void Apply()
        {
            SaveChanges(currentCharId);
            onApply?.Invoke(statusMatrix);
            window.Close();
        }

        /// <summary>
        /// 可从外部调用，更新矩阵后会自动刷新
        /// </summary>
        /// <param name="func"></param>
        public void SelectCouple(Func<CoupleWithIndexStatusWithMeta, bool> func)
        {
            statusMatrix.ForEach
                ((item) =>
                {
                    if (item.selectStatus == SelectStatus.Unavailable) return SelectStatus.Unavailable;
                    if (func(item)) return SelectStatus.Checked;
                    else return SelectStatus.Unchecked;
                });
            RefreshInfo(currentCharId);
        }

        public void SelectTools()
        {
            FunctionSelector.FunctionSelector functionSelector
                = window.OpenWindow<FunctionSelector.FunctionSelector>(toolSelectPrefab);
            functionSelector.Initialize(SelectCouple_All, SelectCouple_Character);
        }

        void SelectCouple_All()
        {
            SelectCouple((_) => true);
        }

        void SelectCouple_Character()
        {
            CharacterSelector.CharacterSelector characterSelector
                = window.OpenWindow<CharacterSelector.CharacterSelector>(characterSelectorPrefab);
            characterSelector.Initialize(
                (charId) =>
                {
                    SelectCouple(
                        (item) =>
                        {
                            if (item.charId1 == charId || item.charId2 == charId)
                                return true;
                            return false;
                        });
                });
        }
    }
}