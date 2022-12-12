using SekaiTools.SekaiViewerInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.MutiServerTableGetter
{
    public class MutiServerTableGetter : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public ToggleGroupValueLoader valueLoader;
        public MasterRefUpdateItem[] masterRefUpdateItems;

        string tableName;
        public string TableName=>tableName;
        public string SavePath => Path.Combine(EnvPath.Sekai_master_db_diff[(ServerRegion)valueLoader.Value], tableName + ".json");
        Action<ServerRegion> onApplyWhileTableExist;

        public void Initialize(ServerRegion defaultServerRegion,string tableName, Action<ServerRegion> onApplyWhileTableExist)
        {
            valueLoader.toggles[(int)defaultServerRegion].isOn = true;
            this.tableName = tableName;
            this.onApplyWhileTableExist = onApplyWhileTableExist;
            foreach (var masterRefUpdateItem in masterRefUpdateItems)
            {
                masterRefUpdateItem.masterName = tableName;
                masterRefUpdateItem.Refresh();
            }
        }

        public void Apply()
        {
            string savePath = SavePath;
            if (!File.Exists(savePath))
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, "所需数据表不存在，请尝试更新对应语言的数据表");
            }
            else
            {
                onApplyWhileTableExist((ServerRegion)valueLoader.Value);
                window.Close();
            }
        }
    }
}