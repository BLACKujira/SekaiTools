using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class MessageAreaTypeA : MonoBehaviour
    {
        public Text[] textLines;
        public bool printTime = false;

        Queue<string> messages = new Queue<string>();

        public void AddLine(string message)
        {
            if (printTime)
                messages.Enqueue($"[{DateTime.Now:T}] {message}");
            else
                messages.Enqueue(message);
            while (messages.Count > textLines.Length)
            {
                messages.Dequeue();
            }
            Refresh();
        }

        public void ClearMessage()
        {
            messages.Clear();
            Refresh();
        }

        public void Refresh()
        {
            string[] messages = this.messages.ToArray();
            for (int i = 0; i < textLines.Length; i++)
            {
                if (i < this.messages.Count)
                    textLines[i].text = messages[i];
                else
                    textLines[i].text = string.Empty;
            }
        }
    }
}