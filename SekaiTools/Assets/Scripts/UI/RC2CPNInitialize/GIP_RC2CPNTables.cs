using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.RC2CPNInitialize
{
    public class GIP_RC2CPNTables : MonoBehaviour , IGenericInitializationPart
    {
        public Toggle togUseBondsHonor;
        public Toggle togUseCarnivalPartyName;
        public MasterRefUpdateCheck mrucBondsHonor;
        public MasterRefUpdateCheck mrucCarnivalPartyName;

        public bool UseBondsHonor => togUseBondsHonor.isOn;
        public bool UseCarnivalPartyName => togUseCarnivalPartyName.isOn;

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (togUseBondsHonor.isOn)
                errors.AddRange(mrucBondsHonor.GetErrors());

            if (togUseCarnivalPartyName.isOn)
                errors.AddRange(mrucCarnivalPartyName.GetErrors());

            return GenericInitializationCheck.GetErrorString("Êý¾Ý±í´íÎó", errors);
        }
    }
}