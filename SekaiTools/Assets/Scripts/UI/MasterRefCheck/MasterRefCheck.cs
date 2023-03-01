using SekaiTools.UI.GenericInitializationParts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.MasterRefCheck
{
    public class MasterRefCheck : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public GIP_MasterRefUpdate gIP_MasterRefUpdate;
        public UniversalGenerator universalGenerator;

        Action onApply;

        public void Initialize(string[] requireTableNames,Action onApply = null)
        {
            List<MasterRefUpdateItem> masterRefUpdateItems = new List<MasterRefUpdateItem>();
            universalGenerator.Generate(requireTableNames.Length,
                (gobj, id) =>
                {
                    MasterRefUpdateItem masterRefUpdateItem = gobj.GetComponent<MasterRefUpdateItem>();
                    masterRefUpdateItem.TableName = requireTableNames[id];
                    masterRefUpdateItems.Add(masterRefUpdateItem);
                });
            gIP_MasterRefUpdate.MasterRefUpdateCheck.masterRefUpdateItems = masterRefUpdateItems;
            this.onApply = onApply;
        }

        public void Apply()
        {
            string errors = GenericInitializationCheck.CheckIfReady(gIP_MasterRefUpdate);
            if (!string.IsNullOrEmpty(errors))
            {
                WindowController.ShowLog(Message.Error.STR_ERROR, errors);
                return;
            }
            window.Close();
            if (onApply != null) onApply();
        }
    }
}