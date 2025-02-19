using DG.Tweening;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSNavigator : Navigator
    {

        private float timeStart = 0f;
        private float timeEnd = 0f;
        private void Awake()
        {
            timeStart = Time.realtimeSinceStartup;
        }

        public override (string, object) GetData(Adapter adapter, string eventName, object eventData)
        {
            BERV01State stateReturn = BERV01State.InitData;
            object ObjectDataReturn = null;
            BERV01State currentState = (BERV01State)Enum.Parse(typeof(BERV01State), eventName);
            LogMe.Log("Lucanhtai currentState: " + currentState.ToString());

            switch (currentState)
            {
                case BERV01State.InitData:
                    stateReturn = BERV01State.InitData;
                    BERV01FTSInitStateData initStateData = adapter.GetData<BERV01FTSInitStateData>(turn);
                    ObjectDataReturn = initStateData;
                    break;
                case BERV01State.IntroGame:
                    stateReturn = BERV01State.IntroGame;
                    BERV01FTSIntroStateData introStateData = adapter.GetData<BERV01FTSIntroStateData>(turn);
                    ObjectDataReturn = introStateData;
                    break;
                case BERV01State.PlayGame:
                    stateReturn = BERV01State.PlayGame;
                    BERV01FTSPlayStateData playStateData = new BERV01FTSPlayStateData();
                    if (eventData != null)
                    {
                        (bool isSkip, bool isReplay) valueEvent = ((bool isSkip, bool isReplay))eventData;
                        playStateData.IsSkipGuiding = valueEvent.isSkip;
                        playStateData.IsReplay = valueEvent.isReplay;
                     }
                    else
                    {
                        playStateData.IsSkipGuiding = false;
                        playStateData.IsReplay = false;
                    }  
                    ObjectDataReturn = playStateData;
                    break;
                case BERV01State.OutroGame:
                    stateReturn = BERV01State.OutroGame;

                    break;
                case BERV01State.FinishGame:
                    stateReturn = BERV01State.FinishGame;
                
                    SoundChannel soundData = new SoundChannel(SoundChannel.STOP_ALL_SOUND_BY_DESTROY, null, null, 0, false);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);

                    timeEnd = Time.realtimeSinceStartup;
                    List<UserEndGameData.Word> wordList = adapter.GetData<List<UserEndGameData.Word>>(turn);

                    UserEndGameData userEndGameData = new UserEndGameData();
                    userEndGameData.TimeSpent = Mathf.CeilToInt(timeEnd - timeStart);
                    userEndGameData.MaxTurn = adapter.GetMaxTurn();
                    userEndGameData.ScoresList = null;
                    userEndGameData.PhonicList = null;
                    userEndGameData.VideoList = null;
                    userEndGameData.WordList = wordList;

                    EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.FinishGame, userEndGameData);
                    ObserverManager.TriggerEvent(userEvent);
                    break;
                default:
                    ObjectDataReturn = null;
                    break;
            }
            return (stateReturn.ToString(), ObjectDataReturn);

        }
        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}