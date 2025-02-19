using DG.Tweening;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSNavigator : Navigator
    {

        private int maxTubeDrag = 0;
        private float timeStart = 0f;
        private float timeEnd = 0f;
        private int dragCorrectCount = 0;
        private bool isEnableCatSpaceship = false;
        private bool isDifficult = false;
        private int indDragScreen = 0;
        private int indDragWrong = 0;
        private void Awake()
        {
            timeStart = Time.realtimeSinceStartup;
        }

        public override (string, object) GetData(Adapter adapter, string eventName, object eventData)
        {
            BEPS01RFSState stateReturn = BEPS01RFSState.InitData;
            object ObjectDataReturn = null;
            BEPS01RFSState currentState = (BEPS01RFSState)Enum.Parse(typeof(BEPS01RFSState), eventName);
            LogMe.Log("Lucanhtai currentState: " + currentState.ToString());

            switch (currentState)
            {
                case BEPS01RFSState.InitData:
                    stateReturn = BEPS01RFSState.InitData;
                    BEPS01RFSInitStateData initStateData = adapter.GetData<BEPS01RFSInitStateData>(turn);
                    initStateData.CurrentTurn = turn;
                    if(turn == 0) isDifficult = initStateData.DataTurn.isDifficult;
                    maxTubeDrag = adapter.GetData<int>(turn);
                    ObjectDataReturn = initStateData;
                    break;
                case BEPS01RFSState.IntroGame:
                    stateReturn = BEPS01RFSState.IntroGame;
                    BEPS01RFSIntroStateData introStateData = adapter.GetData<BEPS01RFSIntroStateData>(turn);
                    introStateData.CurrentTurn = turn;
                    introStateData.IsSkipCTA = (bool)eventData;
                    introStateData.MaxTubeDrag = maxTubeDrag;
                    ObjectDataReturn = introStateData;
                    break;
                case BEPS01RFSState.PlayGame:
                    stateReturn = BEPS01RFSState.PlayGame;
                    BEPS01RFSPlayStateData playStateData = new BEPS01RFSPlayStateData();
                    playStateData.DragCorrectCount = dragCorrectCount;
                    playStateData.MaxTubeDrag = maxTubeDrag;
                    playStateData.IsEnableCatSpaceship = isEnableCatSpaceship;
                    playStateData.TurnGame = turn;
                    playStateData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = playStateData;
                    break;
                case BEPS01RFSState.ClickObject:
                    stateReturn = BEPS01RFSState.ClickObject;
                    BEPS01RFSClickStateEventData clickStateEventData = new BEPS01RFSClickStateEventData();
                    clickStateEventData = (BEPS01RFSClickStateEventData)eventData;
                    ObjectDataReturn = clickStateEventData;
                    break;
                case BEPS01RFSState.DraggingObject:
                    stateReturn = BEPS01RFSState.DraggingObject;
                    indDragScreen++;
                    BEPS01RFSDraggingStateEventData draggingStateEventData = new BEPS01RFSDraggingStateEventData();
                    draggingStateEventData = (BEPS01RFSDraggingStateEventData)eventData;
                    ObjectDataReturn = draggingStateEventData;
                    break;
               /* case BEPS01RFSState.DragResult:
                    stateReturn = BEPS01RFSState.DragResult;
                    BEPS01RFSDragResultStateEventData dragResultStateEventData = new BEPS01RFSDragResultStateEventData();
                    dragResultStateEventData = (BEPS01RFSDragResultStateEventData)eventData;
                    ObjectDataReturn = dragResultStateEventData;
                    break; */
                case BEPS01RFSState.DragCorrect:
                    stateReturn = BEPS01RFSState.PlayGame;
                    ++dragCorrectCount;
                    BEPS01RFSPlayStateData playCorrectStateData = new BEPS01RFSPlayStateData();
                    playCorrectStateData.DragCorrectCount = dragCorrectCount;
                    playCorrectStateData.MaxTubeDrag = maxTubeDrag;
                    playCorrectStateData.TurnGame = turn;
                    playCorrectStateData.IsEnableCatSpaceship = isEnableCatSpaceship;
                    playCorrectStateData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = playCorrectStateData;
                    break;
                case BEPS01RFSState.DragWrong:
                    stateReturn = BEPS01RFSState.DragWrong;
                    indDragWrong++;
                    break;
                case BEPS01RFSState.NextTurnGame:
                    stateReturn = BEPS01RFSState.NextTurnGame;
                    dragCorrectCount = 0;
                    maxTubeDrag = 0;
                    isEnableCatSpaceship = true;
                    BEPS01RFSNextTurnStateData nextTurnStateData = adapter.GetData<BEPS01RFSNextTurnStateData>(turn);
                    nextTurnStateData.TurnGame = turn;
                    nextTurnStateData.MaxTurn = adapter.GetMaxTurn();
                    ObjectDataReturn = nextTurnStateData;

                    ++turn;
                    break;

                case BEPS01RFSState.EndGame:
                    stateReturn = BEPS01RFSState.EndGame;

                    break;
                case BEPS01RFSState.FinishGame:
                    stateReturn = BEPS01RFSState.FinishGame;
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
                    if(!isDifficult)
                    {
                        scores.Score = CalculateDragScreenScore(indDragScreen);
                    } else {
                        scores.Score = CalculateDragWrongScore(indDragWrong);
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
            DOTween.KillAll();
        }
    }
}