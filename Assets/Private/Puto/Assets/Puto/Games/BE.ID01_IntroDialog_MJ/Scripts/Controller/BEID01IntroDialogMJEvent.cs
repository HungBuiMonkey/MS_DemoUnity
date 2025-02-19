using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public struct BEID01IntroDialogMJEvent : EventListener<BEID01IntroDialogMJEvent>
    {
        public string EventName;
        public object Data;
        public BEID01IntroDialogMJEvent(string nameEvent, object data)
        {
            this.EventName = nameEvent;
            this.Data = data;
        }
        public void OnMMEvent(BEID01IntroDialogMJEvent eventType)
        {

        }
        public const string INIT_STATE_START = "INIT_STATE_START";
        public const string INIT_STATE_FINISH = "INIT_STATE_FINISH";

        public const string INTRO_STATE_START = "INTRO_STATE_START";
        public const string INTRO_STATE_FINISH = "INTRO_STATE_FINISH";

        public const string REVIEW_STATE_START = "REVIEW_STATE_START";
        public const string REVIEW_STATE_FINISH = "REVIEW_STATE_FINISH";

        public const string LISTEN_AGAIN_STATE_START = "LISTEN_AGAIN_STATE_START";
        public const string LISTEN_AGAIN_STATE_FINISH = "LISTEN_AGAIN_STATE_FINISH";

        public const string LISTEN_STATE_START = "LISTEN_STATE_START";
        public const string LISTEN_STATE_FINISH = "LISTEN_STATE_FINISH";

        public const string NEXT_TURN_STATE_START = "NEXT_TURN_STATE_START";
        public const string NEXT_TURN_STATE_FINISH = "NEXT_TURN_STATE_FINISH";

        public const string PLAY_STATE_START = "PLAY_STATE_START";
        public const string PLAY_STATE_FINISH = "PLAY_STATE_FINISH";

        public const string ENDING_STATE_START = "ENDING_STATE_START";
        public const string ENDING_STATE_FINISH = "ENDING_STATE_FINISH";

        public const string GAME_PLAY_END = "GAME_PLAY_END";
    }
}