using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01MJConversationContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private ScrollRect rect;

        private List<BEID01IntroDialogMJBoxChat> listBoxChat = new();
        private int currentBoxChatIndex = 0;
        private int itemInstanceClickId = 0;

        public RectTransform GetContentRectTransform()
        {
            return content;
        }
        public ScrollRect GetScrollRect()
        {
            return rect;
        }
        public void Appear()
        {
            rect.enabled = false;
            listBoxChat[currentBoxChatIndex].Play();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        public void ResetItem(int itemInstanceId)
        {
            if (itemInstanceClickId != 0)
            {
                for (int i = 0; i < listBoxChat.Count; ++i)
                {
                    if (listBoxChat[i].GetInstanceID() == itemInstanceClickId)
                    {
                        listBoxChat[i].SetReviewClick(true);
                    }
                }
            }

            itemInstanceClickId = itemInstanceId;
        }
    }
}
