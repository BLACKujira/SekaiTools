using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Windows.Forms;
using SekaiTools.DecompiledClass;
using System;
using System.Linq;
using SekaiTools.SekaiViewerInterface;
using SekaiTools.Count;
using System.Text.RegularExpressions;

namespace SekaiTools
{
    public static class ExtensionTools
    {
        public static void CopyComponentValues(this Transform to,Transform from)
        {
            to.position = from.position;
            to.rotation = from.rotation;
            to.localScale = from.localScale;
        }

        public static Texture2D ApplyMask(Texture2D texture, Texture2D mask)
        {
            if (texture.width != mask.width || texture.height != mask.height) { throw new System.Exception("Size error"); }
            Texture2D newTex = new Texture2D(texture.width, texture.height);
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    UnityEngine.Color color = texture.GetPixel(x, y);
                    color.a = mask.GetPixel(x, y).a;
                    newTex.SetPixel(x, y, color);
                }
            }
            return newTex;
        }

        public static Texture2D Capture(RectTransform rectTransform,Canvas targetCanvas)
        {
            int width = (int)(rectTransform.rect.width * targetCanvas.scaleFactor);
            int height = (int)(rectTransform.rect.height * targetCanvas.scaleFactor);

            float x = rectTransform.position.x + rectTransform.rect.xMin * targetCanvas.scaleFactor;
            float y = rectTransform.position.y + rectTransform.rect.yMin * targetCanvas.scaleFactor;

            Texture2D texture2D = new Texture2D((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
            texture2D.ReadPixels(new Rect(x, y, width, height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }

        /// <summary>
        /// 注意时区问题
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMSToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
        }

        /// <summary>
        /// 以东京时间计算
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMSToDateTimeTST(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo).DateTime;
        }

        public static bool IsAllElemTrue(this bool[] array)
        {
            foreach (var elem in array)
            {
                if (elem == false)
                    return false;
            }
            return true;
        }

        public static bool IsAllElemFalse(this bool[] array)
        {
            foreach (var elem in array)
            {
                if (elem == true)
                    return false;
            }
            return true;
        }

        public static bool IsAudioFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension.Equals(".ogg")
                || extension.Equals(".wav")
                || extension.Equals(".mp3"))
                return true;
            return false;
        }

        public static bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            if (extension.Equals(".png"))
                return true;
            return false;
        }

        public static Dictionary<string, string[]> GetFilesInSubFolder(string folderPath)
        {
            string[] directories = Directory.GetDirectories(folderPath).Select((dir) => Path.GetFileName(dir)).ToArray();
            Dictionary<string, string[]> files = new Dictionary<string, string[]>();
            foreach (var folder in directories)
            {
                files[folder] = Directory.GetFiles(Path.Combine(folderPath, folder)).Select((file) => Path.GetFileName(file)).ToArray();
            }
            return files;
        }

        public static string GetRelativePath(string relativeTo, string path)
        {
            relativeTo = Path.GetFullPath(relativeTo);
            path = Path.GetFullPath(path);
            if (!path.StartsWith(relativeTo)) throw new NotImplementedException();
            return $".{path.Substring(relativeTo.Length,path.Length-relativeTo.Length)}";
        }

        public static string GetFullPath(string path, string basePath)
        {
            basePath = Path.GetFullPath(basePath);
            path.Replace("\\\\", "\\");
            path.Replace("/", "\\");
            path.Replace("//", "\\");
            string[] pathArray = path.Split('\\');
            if (pathArray.Length <= 0 || !pathArray[0].Equals(".")) throw new NotImplementedException();
            return $"{basePath}{path.Substring(1, path.Length - 1)}";
        }

        public static string ChangeFolder(string fromFolder,string toFolder,string path)
        {
            string relativePath = GetRelativePath(fromFolder, path);
            return GetFullPath(relativePath, toFolder);
        }

        public static string[] GetAllFiles(string folder)
        {
            List<string> files = new List<string>();
            GetAllFiles_SingleFolder(folder,files);
            return files.ToArray();
        }

        static void GetAllFiles_SingleFolder(string folder,List<string> fileList)
        {
            string[] files = Directory.GetFiles(folder);
            fileList.AddRange(files);
            string[] folders = Directory.GetDirectories(folder);
            foreach (var folderSub in folders)
            {
                GetAllFiles_SingleFolder(folderSub, fileList);
            }
        }

        public static string GetUrlInSV(string fileInAssetFolder)
        {
            string relativePath = GetRelativePath(EnvPath.Assets, fileInAssetFolder);
            return SekaiViewer.AssetUrl + relativePath.Substring(1,relativePath.Length-1);
        }

        public static int CountNicknameCountMatrices(this NicknameCountMatrix[] nicknameCountMatrices,int talkerId,int nameId)
        {
            return nicknameCountMatrices.Sum(ncs => ncs[talkerId, nameId].Times);
        }

        public static bool RegexCheck(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;
            try
            {
                new Regex(pattern);
            }
            catch { return false; }
            return true;
        }
    }
}