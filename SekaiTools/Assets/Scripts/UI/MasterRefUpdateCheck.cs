using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI
{
    public class MasterRefUpdateCheck : MonoBehaviour
    {
        public List<MasterRefUpdateItem> masterRefUpdateItems;

        public List<string> GetErrors()
        {
            List<string> errors = new List<string>();
            foreach (var masterRefUpdateItem in masterRefUpdateItems)
            {
                if (!File.Exists(masterRefUpdateItem.savePath))
                    errors.Add($"未找到数据表{masterRefUpdateItem.masterName}");
            }
            return errors;
        }
    }
}