using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress_MonthItem : MonoBehaviour
    {
        [Header("Components")]
        public Text txtMonth;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
    }
}