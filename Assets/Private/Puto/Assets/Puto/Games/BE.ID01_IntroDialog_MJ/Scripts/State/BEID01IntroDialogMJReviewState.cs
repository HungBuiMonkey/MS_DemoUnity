using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJReviewState : FSMState
    {
        private BEID01IntroDialogMJReviewStateDependency dependency;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEID01IntroDialogMJReviewStateData reviewStateData = (BEID01IntroDialogMJReviewStateData)data;
            DoWork(reviewStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJReviewStateDependency)data;
        }
        private void DoWork(BEID01IntroDialogMJReviewStateData reviewStateData)
        {
            var listBoxChat = reviewStateData.ListBoxChat;
            dependency.ConversationContainer.GetComponent<ScrollRect>().enabled = true;
            foreach (var boxChat in listBoxChat)
            {
                boxChat.SetReviewClick(true);
            }
        }
    }
    public class BEID01IntroDialogMJReviewStateData
    {
        public List<BEID01IntroDialogMJBoxChat> ListBoxChat { get; set; }
    }
    public class BEID01IntroDialogMJReviewStateDependency
    {
        public BEID01MJConversationContainer ConversationContainer { get; set; }
    }
}