
using DG.Tweening;
using MonkeyBase.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsNavigator : Navigator
    {
        private bool isNextTurn = false;
        private int maxOwlDrag = 0;
        private float timeStart = 0f;
        private float timeEnd = 0f;

        private bool isDifficult = false;
   
        private void Awake()
        {
            timeStart = Time.realtimeSinceStartup;
        }

        public override (string, object) GetData(Adapter adapter, string eventName, object eventData)
        {
            BEPS02FlyingOwlsState stateReturn = BEPS02FlyingOwlsState.InitData;
            object ObjectDataReturn = null;
            BEPS02FlyingOwlsState currentState = (BEPS02FlyingOwlsState)Enum.Parse(typeof(BEPS02FlyingOwlsState), eventName);
            LogMe.Log("Lucanhtai currentState: " + currentState.ToString());

            switch (currentState)
            {
                case BEPS02FlyingOwlsState.InitData:
                    if (isNextTurn)++turn;
                    BEPS02HandleData.DragCorrectCount = 0;
                    BEPS02HandleData.DragWrongCount = 0;
                    stateReturn = BEPS02FlyingOwlsState.InitData;
                    BEPS02FlyingOwlsInitStateData initStateData = adapter.GetData<BEPS02FlyingOwlsInitStateData>(turn);
                    initStateData.IsNextTurn = isNextTurn;
                    if (turn == 0) isDifficult = initStateData.IsDifficult;
                    maxOwlDrag = adapter.GetData<int>(turn);
                    ObjectDataReturn = initStateData;
                    break;
                case BEPS02FlyingOwlsState.IntroGame:
                    stateReturn = BEPS02FlyingOwlsState.IntroGame;
                    BEPS02FlyingOwlsIntroStateData introStateData = adapter.GetData<BEPS02FlyingOwlsIntroStateData>(turn);
                    introStateData.IsNextTurn = isNextTurn;
                    introStateData.MaxOwlDrag = maxOwlDrag;
                    ObjectDataReturn = introStateData;
                    break;

                case BEPS02FlyingOwlsState.PlayGame:
                    stateReturn = BEPS02FlyingOwlsState.PlayGame;
                    BEPS02FlyingOwlsPlayStateData playStateData = new BEPS02FlyingOwlsPlayStateData();
                    if(eventData != null) playStateData.EventDataPlay = (BEPS02FlyingOwlsPlayStateEventData)eventData;
                    playStateData.MaxOwlDrag = maxOwlDrag;
                    playStateData.TurnGame = turn;
                    playStateData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = playStateData;
                    break;
                case BEPS02FlyingOwlsState.PlayEffect:
                    stateReturn = BEPS02FlyingOwlsState.PlayEffect;
                    BEPS02FlyingOwlsPlayEffectStateData playEffectStateData = new BEPS02FlyingOwlsPlayEffectStateData();
                    playEffectStateData.IsPlayEffect = (bool)eventData;
                    ObjectDataReturn = playEffectStateData;
                    break;

                case BEPS02FlyingOwlsState.ClickObject:
                    if (BEPS02HandleData.DragWrongCount == 3) BEPS02HandleData.DragWrongCount = 0;
                    stateReturn = BEPS02FlyingOwlsState.ClickObject;
                    BEPS02FlyingOwlsClickStateData clickStateData = adapter.GetData<BEPS02FlyingOwlsClickStateData>(turn);
                    clickStateData.EventClickData = (BEPS02FlyingOwlsClickStateEventData)eventData;
                    ObjectDataReturn = clickStateData;
                    break;

                case BEPS02FlyingOwlsState.DraggingObject:
                    if (BEPS02HandleData.DragWrongCount == 3) BEPS02HandleData.DragWrongCount = 0;
                    stateReturn = BEPS02FlyingOwlsState.DraggingObject;
                    BEPS02FlyingOwlsDraggingStateData draggingStateData = adapter.GetData<BEPS02FlyingOwlsDraggingStateData>(turn);
                    draggingStateData.EventData = (BEPS02FlyingOwlsDraggingStateEventData)eventData;
                    ObjectDataReturn = draggingStateData;
                    break;

                /*case BEPS02FlyingOwlsState.DragResult:
                    stateReturn = BEPS02FlyingOwlsState.DragResult;
                    if (dragWrongCount > 3)
                    {
                        dragWrongCount = 0;
                    }
                    BEPS02FlyingOwlsDragResultStateData dragStateData = adapter.GetData<BEPS02FlyingOwlsDragResultStateData>(turn);
                    dragStateData.EventDragData = (BEPS02FlyingOwlsDragResultStateEventData)eventData;
                    ObjectDataReturn = dragStateData;
                    break;*/
              
                case BEPS02FlyingOwlsState.DragCorrect:
                    stateReturn = BEPS02FlyingOwlsState.PlayGame;
                    BEPS02FlyingOwlsPlayStateData playStateByDragCorrectData = new BEPS02FlyingOwlsPlayStateData();
                    if (eventData != null) playStateByDragCorrectData.EventDataPlay = (BEPS02FlyingOwlsPlayStateEventData)eventData;
                    playStateByDragCorrectData.MaxOwlDrag = maxOwlDrag;
                    playStateByDragCorrectData.TurnGame = turn;
                    playStateByDragCorrectData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = playStateByDragCorrectData;
                    break;
                case BEPS02FlyingOwlsState.DragWrong:
                    stateReturn = BEPS02FlyingOwlsState.PlayGame;
                   
                    BEPS02FlyingOwlsPlayStateData playStateByDragWrongData = new BEPS02FlyingOwlsPlayStateData();
                    if (eventData != null) playStateByDragWrongData.EventDataPlay = (BEPS02FlyingOwlsPlayStateEventData)eventData;
                    playStateByDragWrongData.MaxOwlDrag = maxOwlDrag;
                    playStateByDragWrongData.TurnGame = turn;
                    playStateByDragWrongData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = playStateByDragWrongData;

                    break;
                case BEPS02FlyingOwlsState.NextTurnGame:
                    stateReturn = BEPS02FlyingOwlsState.NextTurnGame;
                    isNextTurn = true;
                    maxOwlDrag = 0;
                    BEPS02FlyingOwlsNextTurnStateData nextTurnStateData = adapter.GetData<BEPS02FlyingOwlsNextTurnStateData>(turn);
                    nextTurnStateData.TurnGame = turn;
                    nextTurnStateData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = nextTurnStateData;
                    break;

                case BEPS02FlyingOwlsState.OutroGame:
                    stateReturn = BEPS02FlyingOwlsState.OutroGame;
                    break;
                case BEPS02FlyingOwlsState.FinishGame:
                    stateReturn = BEPS02FlyingOwlsState.FinishGame;
                    SoundChannel soundData = new SoundChannel(SoundChannel.STOP_ALL_SOUND_BY_DESTROY, null, null, 0, false);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);

                    timeEnd = Time.realtimeSinceStartup;
                    List<UserEndGameData.Word> wordList = adapter.GetData<List<UserEndGameData.Word>>(turn);

                    UserEndGameData userEndGameData = new UserEndGameData();
                    userEndGameData.TimeSpent = Mathf.CeilToInt(timeEnd - timeStart);
                    userEndGameData.MaxTurn = adapter.GetMaxTurn();
                    userEndGameData.ScoresList = new List<UserEndGameData.Scores>();
                    userEndGameData.PhonicList = null;
                    userEndGameData.VideoList = null;
                    userEndGameData.WordList = wordList;
                    //Score
                    UserEndGameData.Scores scores = new UserEndGameData.Scores();
                    if (!isDifficult)
                    {
                        scores.Score = CalculateDragScreenScore(BEPS02HandleData.IndDragScreen);
                    }
                    else
                    {
                        scores.Score = CalculateDragWrongScore(BEPS02HandleData.IndDragWrong);
                    }
                    userEndGameData.ScoresList.Add(scores);

                    EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.FinishGame, userEndGameData);
                    ObserverManager.TriggerEvent(userEvent);
                    break;

                default:
                    ObjectDataReturn = null;
                    break;
            }
            return (stateReturn.ToString(), ObjectDataReturn);
        }
        private int CalculateDragScreenScore(int indDragScreen)
        {
            if (indDragScreen <= 6)
                return 100;
            else if (indDragScreen >= 7 && indDragScreen <= 8)
                return 80;
            else if (indDragScreen >= 9 && indDragScreen <= 10)
                return 60;
            else if (indDragScreen > 10)
                return 40;
            else
                return 0;
        }

        private int CalculateDragWrongScore(int indDragWrong)
        {
            if (indDragWrong == 0)
                return 100; 
            else if (indDragWrong >= 1 && indDragWrong <= 3)
                return 80;
            else if (indDragWrong >= 4 && indDragWrong <= 6)
                return 60; 
            else if (indDragWrong > 6)
                return 40; 
            else
                return 0;
        }
        private void OnDestroy()
        {
            BEPS02HandleData.DragCorrectCount = 0;
            BEPS02HandleData.DragWrongCount = 0;
            BEPS02HandleData.IndDragWrong = 0;
            BEPS02HandleData.IndDragScreen = 0;
            DOTween.KillAll();
        }
    }
}