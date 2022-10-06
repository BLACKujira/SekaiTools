using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Radio
{
    public class RadioReturnableWindowController : MonoBehaviour
    {
        [Header("Components")]
        public Text textReturn;
        [Header("Settings")]
        public float protectedTime = 30;
        [Header("Prefab")]
        public TipFade tipFadePrefab;

        ReturnPermission returnPermission;
        TipFade tipFade = null;

        public ReturnPermission ReturnPermission => returnPermission;

        public void ResetReturnPermission()
        {
            StartCoroutine(ChangeReturnPermission());
        }

        IEnumerator ChangeReturnPermission()
        {
            if (tipFade != null)
                Destroy(tipFade.gameObject);
            Instantiate(tipFadePrefab, transform);
            Color color = textReturn.color;
            color.a = 0;
            textReturn.color = color;
            returnPermission = ReturnPermission.sender;
            yield return new WaitForSeconds(protectedTime);
            returnPermission = ReturnPermission.everyone;
            textReturn.DOFade(1, 0.3f);
        }
    }
}