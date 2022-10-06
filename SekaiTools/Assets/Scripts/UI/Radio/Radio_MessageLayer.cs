using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace SekaiTools.UI.Radio
{

    public class Radio_MessageLayer : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public RectTransform targetRectTransform;
        [Header("Settings")]
        public float fadeTime = 0.3f;
        public float messageDistance;
        public int maxMessages = 5;
        [Header("Prefab")]
        public Radio_MessageLayer_Item itemPrefab;

        [System.NonSerialized] Queue<RadioMessage> messageQueue = new Queue<RadioMessage>();
        [System.NonSerialized] List<Radio_MessageLayer_Item> messageItems = new List<Radio_MessageLayer_Item>();

        bool isPopuping = false;

        public void AddMessage(RadioMessage message)
        {
            messageQueue.Enqueue(message);
        }

        public void AddMessage(string userName, MessageType messageType, string messageText)
        {
            AddMessage(new RadioMessage(userName, messageType, messageText));
        }

        private void Update()
        {
            if (!isPopuping && messageQueue.Count > 0)
                PopupMessage(messageQueue.Dequeue());
        }
        void PopupMessage(RadioMessage message)
        {
            isPopuping = true;
            //清除自动淡出的消息
            messageItems = new List<Radio_MessageLayer_Item>(
                    from Radio_MessageLayer_Item item in messageItems
                                                             where item != null
                                                             select item
                                                             );
            //向上移动消息
            foreach (var item in messageItems)
            {
                Vector2 anchoredPosition = item.rectTransform.anchoredPosition;
                anchoredPosition.y += messageDistance;
                item.rectTransform.DOAnchorPos(anchoredPosition, fadeTime);
            }
            //淡出超出最大显示数量的消息
            while (messageItems.Count>=maxMessages-1)
            {
                Radio_MessageLayer_Item radio_MessageLayer_Item = messageItems[0];
                radio_MessageLayer_Item.Fade(0, () => Destroy(radio_MessageLayer_Item.gameObject));
                messageItems.RemoveAt(0);
            }
            //显示新的消息
            Radio_MessageLayer_Item newMessageItem = Instantiate(itemPrefab, targetRectTransform);
            newMessageItem.rectTransform.anchoredPosition = Vector2.zero - new Vector2(0,messageDistance);
            newMessageItem.Initialize($"@{message.userName} {message.messageText}", message.messageType, radio);
            newMessageItem.fadeDuration = fadeTime;
            newMessageItem.Fade(1, () => { });
            newMessageItem.rectTransform.DOAnchorPos(Vector2.zero, fadeTime).OnComplete(() => isPopuping = false);
            messageItems.Add(newMessageItem);
        }
    }
}