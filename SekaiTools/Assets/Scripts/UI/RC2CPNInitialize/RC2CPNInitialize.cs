using SekaiTools.DecompiledClass;
using SekaiTools.DecompiledClass.CheerfulCarnival;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.RC2CPNInitialize
{
    public class RC2CPNInitialize : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_RC2CPNTables gIP_RC2CPNTables;
        [Header("Prefab")]
        public Window rc2cpnPrefab;

        public void Apply()
        {
            string error = GenericInitializationCheck.CheckIfReady(gIP_RC2CPNTables);
            if(!string.IsNullOrEmpty(error))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, error);
                return;
            }

            RandomCombine2CharPartyName.RandomCombine2CharPartyName.Settings settings 
                = new RandomCombine2CharPartyName.RandomCombine2CharPartyName.Settings();

            settings.useBondsHonor = gIP_RC2CPNTables.UseBondsHonor;
            settings.useCarnivalPartyName = gIP_RC2CPNTables.UseCarnivalPartyName;

            settings.bondsHonors = EnvPath.GetTable<MasterBondsHonor>("bondsHonors");
            settings.bondsHonorWords = EnvPath.GetTable<MasterBondsHonorWord>("bondsHonorWords");
            settings.carnivalPartyNames = EnvPath.GetTable<MasterCheerfulCarnivalPartyName>("cheerfulCarnivalPartyNames");

            RandomCombine2CharPartyName.RandomCombine2CharPartyName randomCombine2CharPartyName
                = window.OpenWindow<RandomCombine2CharPartyName.RandomCombine2CharPartyName>(rc2cpnPrefab);
            randomCombine2CharPartyName.Initialize(settings);
        }
    }
}