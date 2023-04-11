using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.DynamicBarChart
{
    public class DynamicBarChart_Progress_EventItem : MonoBehaviour
    {
        [Header("Components")]
        public Image imgEvIcon;

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