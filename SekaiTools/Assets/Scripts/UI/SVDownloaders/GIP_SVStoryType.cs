using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SVDownloaders
{
    public class GIP_SVStoryType : MonoBehaviour
    {
        [Header("Components")]
        public Toggle tog_Unit;
        public Toggle tog_Event;
        public Toggle tog_Card;
        public Toggle tog_Map;
        public Toggle tog_Live;
        public Toggle tog_Other;

        public bool Download_Unit => tog_Unit.isOn;
        public bool Download_Event => tog_Event.isOn;
        public bool Download_Card => tog_Card.isOn;
        public bool Download_Map => tog_Map.isOn;
        public bool Download_Live => tog_Live.isOn;
        public bool Download_Other => tog_Other.isOn;
    }
}