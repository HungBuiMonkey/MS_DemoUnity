using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public struct BEPP01BlendingSoundEvent : EventListener<BEPP01BlendingSoundEvent>
    {
        public string EventName;
        public object Data;

        public BEPP01BlendingSoundEvent(string nameEvent, object data)
        {
            this.EventName = nameEvent;
            this.Data = data;
        }

        public void OnMMEvent(BEPP01BlendingSoundEvent eventType)
        {
            throw new System.NotImplementedException();
        }
        public const string INIT_STATE_START = "INIT_STATE_START";
        public const string INIT_STATE_FINISH = "INIT_STATE_FINISH";

        public const string INTRO_STATE_START = "INTRO_STATE_START";
        public const string INTRO_STATE_FINISH = "INTRO_STATE_FINISH";

        public const string CLICK_ELLIE_STATE = "CLICK_ELLIE_STATE";
        public const string CLICK_UNDERLINE_STATE = "CLICK_UNDERLINE_STATE";

        public const string CLICK_ANSWER_STATE = "CLICK_ANSWER_STATE";
        public const string CLICK_ANSWER_CORRECT_STATE_FINISH = "CLICK_ANSWER_CORRECT_DONE";

        public const string CLICK_CORRECT = "CLICK_CORRECT";
        public const string CLICK_WRONG = "CLICK_WRONG";

        public const string NEXTTURN_STATE_START = "NEXTTURN_STATE_START";
        public const string NEXTTURN_STATE_FINISH = "NEXTTURN_STATE_FINISH";

        public const string GUIDING_STATE = "GUIDING_STATE";

        public const string ENDGAME_STATE_START = "ENDGAME_STATE_START";
        public const string ENDGAME_STATE_FINISH = "ENDGAME_STATE_FINISH";

        public const string NEXT_STATE_START = "NEXT_STATE_START";
        public const string NEXT_STATE_FINISH = "NEXT_STATE_FINISH";

        public const string FINISH_GAME = "FINISH_GAME";
    }
}