
using DG.Tweening;

using MonkeyBase.Observer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundNavigator : Navigator
    {
        private int numberPhonicFilled = 0;
        private BEPP01Text answerCorrect;
        private int round = 0;
        private List<BEPP01Text> lstBEPP01TextAlphabet;
        public List<BEPP01AnswerData> listAnswer = new();
        private float timeStart = 0f;
        private float timeEnd = 0f;

        private UserChooseAnswerData chooseAnswerDataCorrect = new UserChooseAnswerData();
        private UserChooseAnswerData chooseAnswerDataWrong = new UserChooseAnswerData();
        private string answerTypeCorrect;
        private string answerTypeInCorrect;

        private int isSetValue = 0;
        private bool isFirstClickWrong = false;
        private bool isFirstClickCorrect = false;

        private void Awake()
        {
            timeStart = Time.realtimeSinceStartup;
        }

        private void Start()
        {
            chooseAnswerDataCorrect.ListAnswerData = new List<UserChooseAnswerData.AnswerData>();
            chooseAnswerDataWrong.ListAnswerData = new List<UserChooseAnswerData.AnswerData>();
            answerTypeCorrect = UserChooseAnswerData.AnswerType.Correct.ToString().ToLower();
            answerTypeInCorrect = UserChooseAnswerData.AnswerType.Incorrect.ToString().ToLower();
        }

        public override (string, object) GetData(Adapter adapter, string eventName, object eventData)
        {
            LogMe.Log("Lucanhtai: " + eventName);
            switch (eventName)
            {
                case BEPP01BlendingSoundEvent.INIT_STATE_START:
                    BEPP01BlendingSoundInitStateData iniStateData = adapter.GetData<BEPP01BlendingSoundInitStateData>(turn);
                    listAnswer = iniStateData.listAnswer;
                    iniStateData.numberPhonicFilled = numberPhonicFilled;
                    return (BEPP01BlendingSoundEvent.INIT_STATE_START, iniStateData);

                case BEPP01BlendingSoundEvent.INIT_STATE_FINISH:
                    if (numberPhonicFilled == 0)
                    {
                        BEPP01BlendingSoundInitStateEventData bEPP01BlendingSoundInitStateEventData = new();
                        bEPP01BlendingSoundInitStateEventData.lstBEPP01TextAlphabet = new();

                        bEPP01BlendingSoundInitStateEventData = (BEPP01BlendingSoundInitStateEventData)eventData;
                        lstBEPP01TextAlphabet = bEPP01BlendingSoundInitStateEventData.lstBEPP01TextAlphabet.ToList();
                        answerCorrect = bEPP01BlendingSoundInitStateEventData.answerCorrect;
                    }
                    BEPP01BlendingSoundIntroStateData introStateData = adapter.GetData<BEPP01BlendingSoundIntroStateData>(turn);
                    introStateData.answerCorrect = answerCorrect;
                    introStateData.numberPhonicFilled = numberPhonicFilled;
                    return (BEPP01BlendingSoundEvent.INTRO_STATE_START, introStateData);

                case BEPP01BlendingSoundEvent.CLICK_ELLIE_STATE:
                    return (BEPP01BlendingSoundEvent.CLICK_ELLIE_STATE, null);

                case BEPP01BlendingSoundEvent.NEXTTURN_STATE_FINISH:
                case BEPP01BlendingSoundEvent.INTRO_STATE_FINISH:
                case BEPP01BlendingSoundEvent.GUIDING_STATE:
                    BEPP01BlendingSoundGuidingStateData guidingStateData = adapter.GetData<BEPP01BlendingSoundGuidingStateData>(turn);
                    return (BEPP01BlendingSoundEvent.GUIDING_STATE, guidingStateData);

                case BEPP01BlendingSoundEvent.CLICK_UNDERLINE_STATE:
                    BEPP01BlendingSoundClickUnderlineStateEventData eventDataClickUnderline = new();
                    eventDataClickUnderline = (BEPP01BlendingSoundClickUnderlineStateEventData)eventData;
                    return (BEPP01BlendingSoundEvent.CLICK_UNDERLINE_STATE, eventDataClickUnderline);

                case BEPP01BlendingSoundEvent.CLICK_ANSWER_STATE:
                    BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData = new();
                    clickAnswerStateData.clickAnswerStateDataPlay = new();
                    clickAnswerStateData.clickAnswerStateDataPlay.listAnswer = listAnswer;
                    clickAnswerStateData.clickAnswerStateDataPlay.numberPhonicFilled = numberPhonicFilled;
                    clickAnswerStateData.clickAnswerStateDataPlay.Round = round;
                    clickAnswerStateData.clickAnswerStateEventData = (BEPP01BlendingSoundClickAnswerStateEventData)eventData;
                    return (BEPP01BlendingSoundEvent.CLICK_ANSWER_STATE, clickAnswerStateData);

                case BEPP01BlendingSoundEvent.CLICK_CORRECT:
                    if (isSetValue == 0)
                    {
                        isFirstClickWrong = false;
                        isFirstClickCorrect = true;
                        isSetValue = 1;
                    }
                    BEPP01AnswerData answerDataCorrect = (BEPP01AnswerData)eventData;
                    UserChooseAnswerData.AnswerData answerItemCorrectData = new UserChooseAnswerData.AnswerData();
                    answerItemCorrectData.Target = answerDataCorrect.strAnswer;
                    answerItemCorrectData.WordID = answerDataCorrect.wordAnswerId;
                    answerItemCorrectData.WordType = answerDataCorrect.wordDataAnswer.Type.ToString();
                    answerItemCorrectData.TypeValue = answerTypeCorrect;
                    chooseAnswerDataCorrect.ListAnswerData.Add(answerItemCorrectData);

                    return ("", null);
                case BEPP01BlendingSoundEvent.CLICK_WRONG:
                    if (isSetValue == 0)
                    {
                        isFirstClickWrong = true;
                        isFirstClickCorrect = false;
                        isSetValue = 1;
                    }
                    BEPP01AnswerData answerDataWrong= (BEPP01AnswerData)eventData;
                    bool exists = chooseAnswerDataWrong.ListAnswerData.Any(item => item.WordID == answerDataWrong.wordAnswerId);
                    if (!exists)
                    {
                        UserChooseAnswerData.AnswerData answerItemWrongData = new UserChooseAnswerData.AnswerData();
                        answerItemWrongData.Target = answerDataWrong.strAnswer;
                        answerItemWrongData.WordID = answerDataWrong.wordAnswerId;
                        answerItemWrongData.WordType = answerDataWrong.wordDataAnswer.Type.ToString();
                        answerItemWrongData.TypeValue = answerTypeInCorrect;
                        chooseAnswerDataWrong.ListAnswerData.Add(answerItemWrongData);
                    }

                    return ("", null);
                case BEPP01BlendingSoundEvent.CLICK_ANSWER_CORRECT_STATE_FINISH:
                    round++;
                    if (round < listAnswer.Count)
                    {
                        BEPP01BlendingSoundPrepareNextTurnStateData prepareNextTurnStateData = new();
                        prepareNextTurnStateData.listAnswer = listAnswer;
                        ++numberPhonicFilled;

                        prepareNextTurnStateData.Turn = turn;
                        prepareNextTurnStateData.numberPhonicFilled = numberPhonicFilled;
                        prepareNextTurnStateData.answerTurn = lstBEPP01TextAlphabet.Find((item) => item.GetData().turn == round);
                        prepareNextTurnStateData.listAnswer = prepareNextTurnStateData.listAnswer;

                        return (BEPP01BlendingSoundEvent.NEXTTURN_STATE_START, prepareNextTurnStateData);
                    }
                    else
                    {
                        //Send event choose answer
                        if (isFirstClickCorrect)
                        {
                            EventUserPlayGameChanel userEventCorrect = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.OtherReport, chooseAnswerDataCorrect);
                            ObserverManager.TriggerEvent(userEventCorrect);
                        }
                        else if (isFirstClickWrong)
                        {
                            EventUserPlayGameChanel userEventWrong = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.OtherReport, chooseAnswerDataWrong);
                            ObserverManager.TriggerEvent(userEventWrong);
                        }
                        chooseAnswerDataCorrect.ListAnswerData.Clear();
                        chooseAnswerDataWrong.ListAnswerData.Clear();
                        isFirstClickWrong = false;
                        isFirstClickCorrect = false;
                        isSetValue = 0;
                        //----------------------------

                        BEPP01BlendingSoundEndGameStateData endgameStateData = adapter.GetData<BEPP01BlendingSoundEndGameStateData>(turn);
                        endgameStateData.listData = listAnswer;
                        endgameStateData.lstBEPP01TextAlphabet = this.lstBEPP01TextAlphabet.ToList();
                        return (BEPP01BlendingSoundEvent.ENDGAME_STATE_START, endgameStateData);
                    }

                case BEPP01BlendingSoundEvent.NEXT_STATE_FINISH:
                    numberPhonicFilled = 0;
                    lstBEPP01TextAlphabet.Clear();
                    listAnswer.Clear();
                    BEPP01BlendingSoundInitStateData iniStateDataNext = adapter.GetData<BEPP01BlendingSoundInitStateData>(turn);
                    iniStateDataNext.numberPhonicFilled = numberPhonicFilled;
                    listAnswer = iniStateDataNext.listAnswer;
                    return (BEPP01BlendingSoundEvent.INIT_STATE_START, iniStateDataNext);

                case BEPP01BlendingSoundEvent.FINISH_GAME:
                    SoundChannel soundData = new SoundChannel(SoundChannel.STOP_ALL_SOUND_BY_DESTROY, null, null, 0, false);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);

                    timeEnd = Time.realtimeSinceStartup;
                    List<UserEndGameData.Word> wordList = (List<UserEndGameData.Word>)eventData;

                    UserEndGameData userEndGameData = new UserEndGameData();
                    userEndGameData.TimeSpent = Mathf.CeilToInt(timeEnd - timeStart);
                    userEndGameData.MaxTurn = adapter.GetMaxTurn();
                    userEndGameData.ScoresList = null;
                    userEndGameData.PhonicList = null;
                    userEndGameData.VideoList = null;
                    userEndGameData.WordList = wordList;

                    EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.FinishGame, userEndGameData);
                    ObserverManager.TriggerEvent(userEvent);
                    return (BEPP01BlendingSoundEvent.FINISH_GAME, null);
                default:
                    return ("", null);
            }
        }
        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}