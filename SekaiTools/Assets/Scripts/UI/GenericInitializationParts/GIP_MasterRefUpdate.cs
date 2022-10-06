using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    [RequireComponent(typeof(MasterRefUpdateCheck))]
    public class GIP_MasterRefUpdate : MonoBehaviour, IGenericInitializationPart
    {
        MasterRefUpdateCheck masterRefUpdateCheck;
        MasterRefUpdateCheck MasterRefUpdateCheck
        {
            get
            {
                if (masterRefUpdateCheck == null)
                    masterRefUpdateCheck = GetComponent<MasterRefUpdateCheck>();
                return masterRefUpdateCheck;
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = MasterRefUpdateCheck.GetErrors();
            if (errors != null && errors.Count != 0)
                return GenericInitializationCheck.GetErrorString("Êý¾Ý±í´íÎó", errors);
            else
                return null;
        }
    }
}