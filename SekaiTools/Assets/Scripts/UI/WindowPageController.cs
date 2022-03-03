using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class WindowPageController : MonoBehaviour
    {
        public List<Page> pages = new List<Page>();
        [System.Serializable]
        public class Page
        {
            public GameObject pageObject;
            public Button prevButton;
            public Button nextButton;
        }

        Page currentPage = null;

        private void Awake()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                int id = i;
                Page page = pages[i];
                if (i > 0&&page.prevButton)
                {
                    page.prevButton.onClick.AddListener(
                        () =>
                        {
                            TurnOnPage(id - 1);
                        });
                }
                if (i < pages.Count - 1&&page.nextButton)
                {
                    page.nextButton.onClick.AddListener(
                        () =>
                        {
                            TurnOnPage(id + 1);
                        });
                }
            }
            foreach (var page in pages)
            {
                if (page.pageObject.activeSelf)
                {
                    currentPage = page;
                    break;
                }
            }
        }

        void TurnOnPage(int id)
        {
            Page page = pages[id];
            if (currentPage == page) return;
            else
            {
                if (currentPage != null) currentPage.pageObject.SetActive(false);
                currentPage = page;
                page.pageObject.SetActive(true);
            }
        }
    }
}