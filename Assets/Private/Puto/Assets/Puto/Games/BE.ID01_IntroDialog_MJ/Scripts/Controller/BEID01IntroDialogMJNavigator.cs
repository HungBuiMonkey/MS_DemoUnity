using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MonkeyBase.Observer;


namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJNavigator : Navigator
    {
        public List<BEID01IntroDialogMJBoxChat> ListBoxChat { get; set; }

        private float timeStart = 0f;
        private float timeEnd = 0f;

        private void Awake()
        {
            timeStart = Time.realtimeSinceStartup;
        }

        public override (string, object) GetData(Adapter adapter, string eventName, object eventData)
        {
            switch (eventName)
            {
                case BEID01IntroDialogMJEvent.INIT_STATE_START:
                    BEID01IntroDialogMJInitStateData initStateData = adapter.GetData<BEID01IntroDialogMJInitStateData>(turn);
                    return (BEID01IntroDialogMJEvent.INIT_STATE_START, initStateData);

                case BEID01IntroDialogMJEvent.INTRO_STATE_START:
                    BEID01IntroDialogMJIntroStateData introStateData = (BEID01IntroDialogMJIntroStateData)eventData;
                    ListBoxChat = new(introStateData.ListBoxChat);
                    return (BEID01IntroDialogMJEvent.INTRO_STATE_START, introStateData);

                case BEID01IntroDialogMJEvent.PLAY_STATE_START:
                    BEID01IntroDialogMJPlayStateData playStateData = adapter.GetData<BEID01IntroDialogMJPlayStateData>(turn);
                    playStateData.ListBoxChat = new(ListBoxChat);
                    return (BEID01IntroDialogMJEvent.PLAY_STATE_START, playStateData);

                case BEID01IntroDialogMJEvent.REVIEW_STATE_START:
                    BEID01IntroDialogMJReviewStateData reviewStateData = new BEID01IntroDialogMJReviewStateData();
                    reviewStateData.ListBoxChat = new(ListBoxChat);
                    return (BEID01IntroDialogMJEvent.REVIEW_STATE_START, reviewStateData);

                case BEID01IntroDialogMJEvent.LISTEN_AGAIN_STATE_START:
                    BEID01IntroDialogMJListenAgainStateData listenAgainStateData = new BEID01IntroDialogMJListenAgainStateData();
                    BEID01MJListenAgainStateEventData listenAgainStateEventData = (BEID01MJListenAgainStateEventData)eventData;
                    listenAgainStateData.EventData = listenAgainStateEventData;
                    listenAgainStateData.ListBoxChat = new(ListBoxChat);
                    return (BEID01IntroDialogMJEvent.LISTEN_AGAIN_STATE_START, listenAgainStateData);

                case BEID01IntroDialogMJEvent.LISTEN_STATE_START:
                    BEID01IntroDialogMJListenStateData listenStateData = new BEID01IntroDialogMJListenStateData();
                    BEID01IntroDialogMJListenStateEventData listenStateEventData = (BEID01IntroDialogMJListenStateEventData)eventData;
                    listenStateData.EventData = listenStateEventData;
                    return (BEID01IntroDialogMJEvent.LISTEN_STATE_START, listenStateData);

                case BEID01IntroDialogMJEvent.GAME_PLAY_END:
                    
                    SoundChannel soundData = new SoundChannel(SoundChannel.STOP_ALL_SOUND_BY_DESTROY, null, null, 0, false);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    timeEnd = Time.realtimeSinceStartup;
                    BEID01IntroDialogMJEndGameStateData endGameStateData = adapter.GetData<BEID01IntroDialogMJEndGameStateData>(turn);

                    UserEndGameData userEndGameData = new UserEndGameData();
                    userEndGameData.TimeSpent = Mathf.CeilToInt(timeEnd - timeStart);
                    userEndGameData.MaxTurn = adapter.GetMaxTurn();
                    userEndGameData.ScoresList = null;
                    userEndGameData.PhonicList = null;
                    userEndGameData.VideoList = null;
                    userEndGameData.WordList = endGameStateData.WordList;
                   
                    EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.FinishGame, userEndGameData);
                    ObserverManager.TriggerEvent(userEvent);
                    return (BEID01IntroDialogMJEvent.GAME_PLAY_END, null);
            }
            return ("", null);
        }
    }
    public class BEID01IntroDialogMJEndGameStateData
    {
        public List<UserEndGameData.Word> WordList { get; set; }

    }


}