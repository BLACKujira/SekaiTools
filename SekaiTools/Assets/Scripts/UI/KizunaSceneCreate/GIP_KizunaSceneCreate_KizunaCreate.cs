using SekaiTools.DecompiledClass;
using SekaiTools.UI.CoupleCombiner;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.KizunaSceneCreate
{
    public class GIP_KizunaSceneCreate_KizunaCreate : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public UniversalGenerator2D universalGenerator2D;
        public Text txtInfoDisplay;
        [Header("Prefab")]
        public Window selectorFreePrefab;
        public Window selectorLimPrefab;

        Vector2Int[] selectedCouple = null;
        public Vector2Int[] SelectedCouple => selectedCouple;

        private void Awake()
        {
            RefreshCoupleDisplay();
        }

        int[] SelectedCoupleCharCount()
        {
            HashSet<int> chars = new HashSet<int>(
                selectedCouple.Select((v2) => v2.x)
                .Concat(
                    selectedCouple
                    .Select((v2) => v2.y))
                );
            return chars.ToArray();
        }

        void RefreshCoupleDisplay()
        {
            universalGenerator2D.ClearItems();
            if (selectedCouple != null)
            {
                universalGenerator2D.Generate(selectedCouple.Length,
                    (gobj, id) =>
                    {
                        BondsHonorSub bondsHonorSub = gobj.GetComponent<BondsHonorSub>();
                        bondsHonorSub.SetCharacter(selectedCouple[id].x, selectedCouple[id].y);
                    });
                txtInfoDisplay.text = $"共{selectedCouple.Length}对组合，包含{SelectedCoupleCharCount().Length}名角色";
            }
            else
            {
                txtInfoDisplay.text = $"请选择组合";
            }
        }

        void OnSelectorApply(Vector2Int[] array)
        {
            selectedCouple = array;
            RefreshCoupleDisplay();
        }

        public void SelectFree()
        {
            CoupleCombiner.CoupleCombiner coupleCombiner
                = WindowController.CurrentWindow.OpenWindow<CoupleCombiner.CoupleCombiner>(selectorFreePrefab);

            coupleCombiner.Initialize(selectedCouple,OnSelectorApply);
        }

        const int MATRIX_SIZE = 27;
        public void SelectLim()
        {
            WindowController.ShowMasterRefCheck(
                new string[] { "bondsHonors" },
                ()=>
                {
                    MasterBondsHonor[] masterBondsHonors
                        = EnvPath.GetTable<MasterBondsHonor>("bondsHonors");
                    
                    CoupleAvailableStatus coupleAvailableStatus = new CoupleAvailableStatus(MATRIX_SIZE);
                    foreach (var masterBondsHonor in masterBondsHonors)
                    {
                        coupleAvailableStatus[masterBondsHonor.gameCharacterUnitId1]
                        .Items[masterBondsHonor.gameCharacterUnitId2] = true;
                        coupleAvailableStatus[masterBondsHonor.gameCharacterUnitId2]
                        .Items[masterBondsHonor.gameCharacterUnitId1] = true;
                    }

                    CoupleCombinerLimited coupleCombinerLimited
                        = WindowController.CurrentWindow.OpenWindow<CoupleCombinerLimited>(selectorLimPrefab);
                    coupleCombinerLimited.Initialize(coupleAvailableStatus, selectedCouple, OnSelectorApply);
                });
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if(selectedCouple == null || selectedCouple.Length == 0)
            {
                errors.Add("未选择组合");
            }
            return GenericInitializationCheck.GetErrorString("组合错误", errors);
        }
    }
}
