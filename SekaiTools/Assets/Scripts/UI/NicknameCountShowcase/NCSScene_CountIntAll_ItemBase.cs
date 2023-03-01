using UnityEngine;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public abstract class NCSScene_CountIntAll_ItemBase : MonoBehaviour
    {
        public abstract void Initialize(int talkerId, int nameId, int times, int total);
    }
}