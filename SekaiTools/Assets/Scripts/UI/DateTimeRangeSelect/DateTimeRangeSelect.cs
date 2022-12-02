using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.DateTimeRangeSelect
{
    public class DateTimeRangeSelect : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public InputField inputFieldStartTime;
        public InputField inputFieldEndTime;

        Action<DateTimeRange> onApply = null;

        public void Initialize(DateTimeRange defaultTime, Action<DateTimeRange> onApply)
        {
            inputFieldStartTime.text = defaultTime.startTime.ToString("D");
            inputFieldEndTime.text = defaultTime.endTime.ToString("D");
            this.onApply = onApply;
        }

        public void Apply()
        {
            DateTime startTime;
            DateTime endTime;
            if(!DateTime.TryParse(inputFieldStartTime.text,out startTime))
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, "无法识别起始日期");
                return;
            }
            if (!DateTime.TryParse(inputFieldEndTime.text, out endTime))
            {
                WindowController.ShowMessage(Message.Error.STR_ERROR, "无法识别终止日期");
                return;
            }
            if (onApply != null)
                onApply(new DateTimeRange(startTime,endTime));
            window.Close();
        }
    }
}