using SekaiTools.Count;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCounterInitialize
{
    public class GIP_NicknameCounterSample : MonoBehaviour , IGenericInitializationPart
    {
        [Header("Components")]
        public FolderSelectItem folder_Sample;
        public FolderSelectItem folder_PassSample;
        public Toggle tog_PassExistingFile;
        public Toggle tog_PassUnusedFile;

        FileStruct fileStruct = FileStruct.Server;

        public FileStruct FileStruct => fileStruct;
        public string Folder_Sample => folder_Sample.SelectedPath;
        public bool PassExistingFile => tog_PassExistingFile.isOn;
        public string Folder_PassSample => folder_PassSample.SelectedPath;
        public bool PassUnusedFile => tog_PassUnusedFile.isOn;

        readonly string[] checkExtensions = new string[] { ".ncmsce", ".ncmcer" };

        bool IsSampleFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension.Equals(".json") || extension.Equals(".asset");
        }

        string[] RemoveExistingFile(string[] files)
        {
            List<string> outFiles = new List<string>();
            foreach (string file in files) 
            {
                if(!IfExistingFile(file)) outFiles.Add(file);
            }
            return outFiles.ToArray();
        }

        bool IfExistingFile(string filePath)
        {
            string checkPath = ExtensionTools.ChangeFolder(Folder_Sample, Folder_PassSample, filePath);
            foreach (var extension in checkExtensions)
            {
                string checkPathWithNewExtension = Path.ChangeExtension(checkPath, extension);
                if(File.Exists(checkPathWithNewExtension)) return true;
            }
            return false;
        }

        public string[] UnitStoryFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetUnitStoryFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample,NicknameCountData.unitStoriesFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetUnitStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/scenario/unitstory";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }
        public string[] EventStoryFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetEventStoryFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample, NicknameCountData.eventStoriesFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetEventStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/event_story";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder =>
            {
                string[] folderSubs = new string[]
                {
                    Path.Combine(folder, "scenario"),
                    Path.Combine(folder, "scenario_rip")
                };
                foreach (var folderSub in folderSubs)
                {
                    if(Directory.Exists(folderSub))
                        return Directory.GetFiles(folderSub);
                }
                return new string[0];
            }).Where(IsSampleFile).ToArray();
            return files;
        }
        public string[] CardStoryFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetCardStoryFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample, NicknameCountData.cardStoriesFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetCardStoryFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/character/member";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }
        public string[] MapTalkFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetMapTalkFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample, NicknameCountData.mapTalkFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetMapTalkFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/scenario/actionset";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }
        public string[] LiveTalkFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetLiveTalkFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample, NicknameCountData.liveTalkFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetLiveTalkFiles_Server(string folder_Sample)
        {
            string folderBase = $"{folder_Sample}/virtual_live/mc/scenario";
            string[] folders = Directory.GetDirectories(folderBase);
            string[] files = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile).ToArray();
            return files;
        }
        public string[] OtherStoryFiles
        {
            get
            {
                string[] outFiles = null;
                switch (fileStruct)
                {
                    case FileStruct.Server: outFiles = GetOtherStoryFiles_Server(Folder_Sample); break;
                    case FileStruct.Classic: outFiles = GetStroyFiles_Classic(Folder_Sample, NicknameCountData.otherStoriesFolder); break;
                }
                if(PassExistingFile) outFiles = RemoveExistingFile(outFiles);
                return outFiles;
            }
        }
        public string[] GetOtherStoryFiles_Server(string folder_Sample)
        {
            List<string> files = new List<string>();
            for (int i = 1; i < 27; i++)
            {
                files.Add($"{folder_Sample}/scenario/profile_rip/self_{(Character)i}.json");
            }

            string folderBase = $"{folder_Sample}/scenario/special";
            string[] folders = Directory.GetDirectories(folderBase);
            IEnumerable<string> spStories = folders.SelectMany(folder => Directory.GetFiles(folder)).Where(IsSampleFile);

            files.AddRange(spStories);
            return files.ToArray();
        }

        private string[] GetStroyFiles_Classic(string folder_Sample,string typeFolder)
        {
            return Directory.GetFiles($"{folder_Sample}/{typeFolder}").Where(IsSampleFile).ToArray();
        }

        public void Initialize()
        {
            folder_Sample.defaultPath = EnvPath.Assets;
            folder_Sample.ResetPath();
        }

        public void SwitchFileStruct_Server() => fileStruct = FileStruct.Server;
        public void SwitchFileStruct_Classic() => fileStruct = FileStruct.Classic;

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(folder_Sample.SelectedPath))
                errors.Add("没有选择样本文件夹");
            if (PassExistingFile && string.IsNullOrEmpty(folder_PassSample.SelectedPath))
                errors.Add("没有选择跳过样本的文件夹");
            return GenericInitializationCheck.GetErrorString("样本错误", errors);
        }
    }
    public enum FileStruct { Server, Classic }
}