using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundClickAnswerState : FSMState
    {
        private float fadeTime = 0.5f;
        private BEPP01BlendingSoundClickAnswerStateDataObjectDependency dependency;
        private CancellationTokenSource cancellationTokenSource;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData = (BEPP01BlendingSoundClickAnswerStateData)data;
            cancellationTokenSource = new();
            CancellationToken token = cancellationTokenSource.Token;
            DoWork(clickAnswerStateData, token);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundClickAnswerStateDataObjectDependency)data;
        }

        private void DoWork(BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData, CancellationToken token)
        {
            // stop sound
            SoundChannel stopSoundData = new(SoundChannel.STOP_SOUND, null);
            ObserverManager.TriggerEvent<SoundChannel>(stopSoundData);
            string answerClick = clickAnswerStateData.clickAnswerStateEventData.strAnswerClick;
            List<int> indexesAnswerClick = clickAnswerStateData.clickAnswerStateEventData.indexesAnswerClick;
          
            int currentTurn = clickAnswerStateData.clickAnswerStateDataPlay.Round;
            BEPP01AnswerData ansCorrect = clickAnswerStateData.clickAnswerStateDataPlay.listAnswer.Find((item) => item.turn == currentTurn);
            string answerCorrect = ansCorrect.strAnswer;
            EnableButton(false, clickAnswerStateData, answerCorrect);

            if (answerClick == answerCorrect && AreListsEqual(indexesAnswerClick, ansCorrect.indexes))
            {
                PlayActionCorrect(clickAnswerStateData.clickAnswerStateEventData.OClick, clickAnswerStateData, token);
            }
            else
            {
                PlayActionWrong(clickAnswerStateData.clickAnswerStateEventData.OClick, answerCorrect, clickAnswerStateData, token);
            }
        }

        private async void PlayActionCorrect(GameObject currentAns, BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData, CancellationToken token)
        {

            try
            {
                BEPP01BlendingSoundEvent eventCorrect = new(BEPP01BlendingSoundEvent.CLICK_CORRECT, currentAns.GetComponentInChildren<BEPP01Text>().GetData());
                ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(eventCorrect);
                // change Color correct
                BEPP01Image answerCorrectImage = currentAns.GetComponent<BEPP01Image>();
                answerCorrectImage.SetColorCorrect();

                SoundChannel soundDataEffectCorrect = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.ClickAnswerDataConfig.audioCorrect, null, 0.5f);
                ObserverManager.TriggerEvent<SoundChannel>(soundDataEffectCorrect);
                // Audio Answer
                AudioClip audioAnswerCorrect = currentAns.GetComponentInChildren<BEPP01Text>().GetData().audioAnswer;
                SoundChannel soundDataAnswer = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, audioAnswerCorrect);
                ObserverManager.TriggerEvent<SoundChannel>(soundDataAnswer);
                bool isAnimationDone = false;
                dependency.EllieSkeleton.AnimationState.SetAnimation(0, dependency.ellieCorrect, false).Complete += (trackEntry) =>
                {
                    isAnimationDone = true;
                    dependency.EllieSkeleton.AnimationState.SetAnimation(0, dependency.ellieCorrectToNormal, false).Complete += (trackEntry) =>
                    {
                        dependency.EllieSkeleton.AnimationState.SetAnimation(0, dependency.ellieNormal, true);
                    };
                };
                List<int> listIndexUnderline = answerCorrectImage.GetComponentInChildren<BEPP01Text>().GetIndexes();

                foreach (var ind in listIndexUnderline)
                {
                    GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                    BaseImage imageUnderline = underLine.GetComponentInChildren<BEPP01Image>();
                    imageUnderline.SetColor(Color.white);
                    imageUnderline.GetComponentInParent<BEPP01Text>().FadeText(1, 1);
                }

                await UniTask.Delay(500, cancellationToken: token);

                bool isPopup = false;
                RectTransform rectAnswer = currentAns.GetComponent<RectTransform>();
                rectAnswer.DOAnchorPosY(dependency.pointAnswerInit.anchoredPosition.y, 0.5f).OnComplete(() => { isPopup = true; });
                await UniTask.WaitUntil(() => isPopup, cancellationToken: token);
                rectAnswer.transform.localScale = Vector3.zero;
                //delay 0.3s
                await UniTask.Delay(250, cancellationToken: token);
                TriggerFinishClickCorrectAnswer();
            }
            catch (OperationCanceledException ex)
            {
            }

        }
        private async void PlayActionWrong(GameObject currentAns, string answerCorrect, BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData, CancellationToken token)
        {
            try
            {
                BEPP01BlendingSoundEvent eventWrong = new(BEPP01BlendingSoundEvent.CLICK_WRONG, currentAns.GetComponentInChildren<BEPP01Text>().GetData());
                ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(eventWrong);

                // change Color wrong
                BEPP01Image answerClick = currentAns.GetComponent<BEPP01Image>();
                answerClick.SetColorWrong();

                //// SFX wrong
                SoundChannel soundDataEffectWrong = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.ClickAnswerDataConfig.audioWrong, null, 0.5f);
                ObserverManager.TriggerEvent<SoundChannel>(soundDataEffectWrong);

                // Audio Answer
                bool isPlayingSound = false;
                AudioClip audioAnswerWrong = currentAns.GetComponentInChildren<BEPP01Text>().GetData().audioAnswer;
                SoundChannel soundDataAnswer = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, audioAnswerWrong, () => { isPlayingSound = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundDataAnswer);
                bool isEllieWrong = false;
                dependency.EllieSkeleton.AnimationState.SetAnimation(0, dependency.ellieWrong, false).Complete += (trackEntry) =>
                {
                    isEllieWrong = true;
                    dependency.EllieSkeleton.AnimationState.SetAnimation(0, dependency.ellieNormal, true);
                };
                await UniTask.WaitUntil(() => isPlayingSound && isEllieWrong, cancellationToken: token);

                // fade Color Answer to normal
                answerClick.FadeColorNormal(fadeTime);
                await UniTask.Delay(500, cancellationToken: token); // [Spam]

                EnableButton(true, clickAnswerStateData, answerCorrect);

                TriggerFinishClickAnswerWrong();
            }
            catch (OperationCanceledException ex)
            {
            }
        }

        private void EnableButton(bool isEnable, BEPP01BlendingSoundClickAnswerStateData clickAnswerStateData, string answerCorrect)
        {
            for (int j = 1; j <= answerCorrect.Length; j++)
            {
                Button button = dependency.BoxUnderline.GetChild(clickAnswerStateData.clickAnswerStateDataPlay.numberPhonicFilled).GetComponent<Button>();
                button.enabled = isEnable;
            }

            dependency.ButtonEllie.enabled = isEnable;
            for (int i = 0; i < dependency.ListButtonAnswer.Count; i++)
            {
                dependency.ListButtonAnswer[i].enabled = isEnable;
            }
        }
        private void TriggerFinishClickAnswerWrong()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.GUIDING_STATE, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        private void TriggerFinishClickCorrectAnswer()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.CLICK_ANSWER_CORRECT_STATE_FINISH, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        public bool AreListsEqual(List<int> list1, List<int> list2)
        {
            return list1.SequenceEqual(list2);
        }
        public override void OnExit()
        {
            base.OnExit();
            cancellationTokenSource?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
    public class BEPP01BlendingSoundClickAnswerStateData
    {
        public BEPP01BlendingSoundClickAnswerStateDataPlay clickAnswerStateDataPlay { get; set; }
        public BEPP01BlendingSoundClickAnswerStateEventData clickAnswerStateEventData { get; set; }
    }
    public class BEPP01BlendingSoundClickAnswerStateDataPlay
    {
        public List<BEPP01AnswerData> listAnswer { get; set; }
        public int numberPhonicFilled { get; set; }
        public int Round { get; set; }
    }
    public class BEPP01BlendingSoundClickAnswerStateEventData
    {
        public GameObject OClick { get; set; }
        public string strAnswerClick { get; set; }
        public List<int> indexesAnswerClick { get; set; }
    }
    public class BEPP01BlendingSoundClickAnswerStateDataObjectDependency
    {    
        public BEPP01BlendingSoundClickAnswerConfig ClickAnswerDataConfig { get; set; }
        public RectTransform ellie { get; set; }
        public List<GameObject> listLine { get; set; }
        public SkeletonGraphic EllieSkeleton { get; set; }
        public Button ButtonEllie { get; set; }
        public RectTransform BoxUnderline { get; set; }
        public RectTransform BoxAnswer { get; set; }
        public List<Button> ListButtonAnswer { get; set; }
        public RectTransform pointAnswerInit { get; set; }
        public string ellieCorrect { set; get; }
        public string ellieCorrectToNormal { set; get; }
        public string ellieWrong { set; get; }
        public string ellieNormal { set; get; }
    }
}