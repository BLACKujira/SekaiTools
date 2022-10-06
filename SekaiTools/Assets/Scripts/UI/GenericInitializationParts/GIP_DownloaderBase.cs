using SekaiTools.UI.Downloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_DownloaderBase : MonoBehaviour , IGenericInitializationPart
    {
        public InputField retryWaitTimeInput;
        public InputField retryTimesInput;
        public Toggle[] existingFileProcessingModeToggles;

        public float retryWaitTime => float.Parse(retryWaitTimeInput.text);
        public int retryTimes => int.Parse(retryTimesInput.text);
        public ExistingFileProcessingMode existingFileProcessingMode
        {
            get
            {
                for (int i = 0; i < existingFileProcessingModeToggles.Length; i++)
                {
                    if (existingFileProcessingModeToggles[i].isOn)
                        return (ExistingFileProcessingMode)i;
                }
                return ExistingFileProcessingMode.Override;
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            try 
            { 
                if(retryWaitTime<=0)
                    errors.Add("等待时间不能为负数或0");
            }
            catch { errors.Add("等待时间输入格式不正确"); }

            try 
            { 
                if(retryTimes<0)
                    errors.Add("重试次数不能为负数"); 
            }
            catch { errors.Add("重试次数输入格式不正确"); }

            return GenericInitializationCheck.GetErrorString("下载设置错误", errors);
        }
    }
}