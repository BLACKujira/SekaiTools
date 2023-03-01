using SekaiTools.Cutin;
using SekaiTools.DecompiledClass;
using SekaiTools.UI.CoupleWithIndexSelector;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SekaiTools.UI.CutinSceneEditorInitialize
{
    public class GIP_CSDSaveData : GIP_CreateOrSaveData
    {
        [Header("Components")]
        public Text txt_CreatedDataInfo;
        [Header("Settings")]
        public UnityEvent onDataChange;
        [Header("Prefab")]
        public Window selectorPrefab;

        public const int MATRIX_SIZE = 57;
        public const int MAX_CLIP_INDEX = 6;

        public CutinSceneData CutinSceneData
        {
            get
            {
                CutinSceneData cutinSceneData;
                if (IfNewFile)
                {
                    cutinSceneData = new CutinSceneData(createdData);
                    cutinSceneData.SavePath = file_SaveData.SelectedPath;
                }
                else
                {
                    string data = File.ReadAllText(file_LoadData.SelectedPath);
                    cutinSceneData = CutinSceneData.LoadData(data, file_LoadData.SelectedPath);
                }
                return cutinSceneData;
            }
        }

        CoupleWithIndexStatus createdData = null;
        protected CoupleWithIndexStatus CreatedData => createdData;

        private void Awake()
        {
            file_LoadData.onPathChange.AddListener((_) => onDataChange.Invoke());
            RefreshInfo();
        }

        public void SelectClipAll()
        {
            CoupleWithIndexSelector.CoupleWithIndexSelector coupleWithIndexSelector
                = WindowController.CurrentWindow.OpenWindow<CoupleWithIndexSelector.CoupleWithIndexSelector>(selectorPrefab);
            if(createdData==null)
            {
                coupleWithIndexSelector.Initialize(MATRIX_SIZE, ApplyCreatedData);
            }
            else
            {
                CoupleWithIndexStatus tempStatus = createdData.DeepClone();
                tempStatus.RemoveMask();
                coupleWithIndexSelector.Initialize(tempStatus, ApplyCreatedData);
            }
        }

        public void SelectClipLimted()
        {
            WindowController.ShowMasterRefCheck(
                new string[] { "ingameCutinCharacters" },
                () =>
                {
                    MasterIngameCutinCharacters[] masterIngameCutinCharacters
                        = EnvPath.GetTable<MasterIngameCutinCharacters>("ingameCutinCharacters");
                    CoupleWithIndexStatus coupleWithIndexStatus = GetCoupleWithIndexStatus(
                        masterIngameCutinCharacters.Select((micc)=>micc.assetbundleName1));


                    CoupleWithIndexSelector.CoupleWithIndexSelector coupleWithIndexSelector
                        = WindowController.CurrentWindow.OpenWindow<CoupleWithIndexSelector.CoupleWithIndexSelector>(selectorPrefab);
                    if (createdData == null)
                    {
                        coupleWithIndexSelector.Initialize(coupleWithIndexStatus, ApplyCreatedData);
                    }
                    else
                    {
                        CoupleWithIndexStatus tempStatus = createdData.DeepClone();
                        tempStatus.ApplyMask(coupleWithIndexStatus);
                        coupleWithIndexSelector.Initialize(tempStatus, ApplyCreatedData);
                    }
                });
        }

        protected CoupleWithIndexStatus GetCoupleWithIndexStatus(IEnumerable<string> fileNames,bool checkAvailable = false)
        {
            CoupleWithIndexStatus coupleWithIndexStatus = new CoupleWithIndexStatus(MATRIX_SIZE);
            int maxId = 0;
            List<CutinVoiceInfo> cutinVoiceInfos = new List<CutinVoiceInfo>();
            foreach (var filename in fileNames)
            {
                CutinVoiceInfo cutinVoiceInfo = ConstData.IsCutinVoice(filename);
                if (cutinVoiceInfo != null && cutinVoiceInfo.Type == CutinVoiceType.bondscp)
                {
                    cutinVoiceInfos.Add(cutinVoiceInfo);
                    if (cutinVoiceInfo.index > maxId)
                        maxId = cutinVoiceInfo.index;
                }
            }
            maxId = maxId > MAX_CLIP_INDEX ? maxId : MAX_CLIP_INDEX;
            maxId++;
            foreach (var row in coupleWithIndexStatus.Rows)
            {
                for (int i = 0; i < row.Items.Length; i++)
                {
                    row.Items[i] = new SelectStatus[maxId];
                    for (int j = 0; j < maxId; j++)
                    {
                        row.Items[i][j] = SelectStatus.Unavailable;
                    }
                }
            }
            foreach (var cutinVoiceInfo in cutinVoiceInfos)
            {
                coupleWithIndexStatus[cutinVoiceInfo.charFirstId]
                    .Items[cutinVoiceInfo.charSecondId][cutinVoiceInfo.index] = checkAvailable?SelectStatus.Checked:SelectStatus.Unchecked;
            }
            return coupleWithIndexStatus;
        }

        public void SelectClipFolder()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "选择存放语音的文件夹";
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath);

            CoupleWithIndexStatus coupleWithIndexStatus = GetCoupleWithIndexStatus(
                files.Select((filename) => Path.GetFileNameWithoutExtension(filename)),true);

            ApplyCreatedData(coupleWithIndexStatus);
        }

        public virtual void RefreshInfo()
        {
            if (createdData != null)
                txt_CreatedDataInfo.text = $"存档内有{createdData.ClipCount}个片段，共出现{createdData.AppearCharacters.Length}名角色";
            else
                txt_CreatedDataInfo.text = Message.IO.STR_PLEASECREATEDATA;
        }

        protected void ApplyCreatedData(CoupleWithIndexStatus createdData)
        {
            this.createdData = createdData;
            RefreshInfo();
            onDataChange.Invoke();
        }

        protected override List<string> GetErrorList()
        {
            List<string> list = base.GetErrorList();
            if (IfNewFile && createdData == null)
                list.Add("未选择片段");
            return list;
        }
    }
}