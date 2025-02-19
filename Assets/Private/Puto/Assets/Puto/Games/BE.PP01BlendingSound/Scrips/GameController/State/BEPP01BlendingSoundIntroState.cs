using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundIntroState : FSMState
    {
        private BEPP01BlendingSoundIntroStateDataObjectDependency dependency;
        private CancellationTokenSource cancellationTokenSource;
        private bool isOneTime = true;
        public override void OnEnter(object data)
        {
            base.OnEnter();
            BEPP01BlendingSoundIntroStateData introStateData = (BEPP01BlendingSoundIntroStateData)data;
            cancellationTokenSource = new();
            CancellationToken token = cancellationTokenSource.Token;
            DoWork(introStateData, token);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundIntroStateDataObjectDependency)data;
        }

        private async void DoWork(BEPP01BlendingSoundIntroStateData introStateData, CancellationToken token)
        {
            try
            {
                Vector3 initBoardSizeScaleIpad = new(1.1f, 1.1f, 1.1f);
                Vector3 zoomBoardSizeScaleIpad = new(1.3f, 1.3f, 1.3f);
                Vector3 zoomBoardSizeScalePhone = new(1.2f, 1.2f, 1.2f);
                Vector3 zoomEllieSizeScalePhone = new(0.59f, 0.59f, 0.59f);

                if (introStateData.numberPhonicFilled == 0)
                {
                    if (isOneTime)
                    {
                        isOneTime = false;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.backGround);
                        float ratio = dependency.backGround.rect.x / dependency.backGround.sizeDelta.y;

                        // nếu là ipad
                        if (ratio <= 1.6f)
                        {
                            dependency.board.anchoredPosition = dependency.pointStartBoard.anchoredPosition;
                            dependency.board.localScale = initBoardSizeScaleIpad;
                        }
                        dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieIntro, true);

                        bool isPlayZoom = false;
                        await UniTask.Delay(dependency.introDataConfig.miliSecondStartDelay, cancellationToken: token);
                        if (ratio <= 1.6f)
                        {
                            dependency.board.DOScale(zoomBoardSizeScaleIpad, dependency.introDataConfig.secondTimeZoom);
                        }
                        else
                        {
                            dependency.board.DOScale(zoomBoardSizeScalePhone, dependency.introDataConfig.secondTimeZoom);
                        }
                        dependency.board.DOAnchorPos(dependency.pointBoardMoveIntro.anchoredPosition, dependency.introDataConfig.secondTimeZoom);
                        dependency.studenGroup.DOAnchorPos(dependency.pointStudentGroupDissapear.anchoredPosition, dependency.introDataConfig.secondTimeZoom);
                        dependency.ellie.DOScale(zoomEllieSizeScalePhone, dependency.introDataConfig.secondTimeZoom);
                        dependency.ellie.DOAnchorPosY(dependency.ellie.anchoredPosition.y - 70, dependency.introDataConfig.secondTimeZoom);
                        dependency.BGBottom.DOAnchorPosY(dependency.BGBottom.anchoredPosition.y - 100, dependency.introDataConfig.secondTimeZoom).OnComplete(() =>
                        {
                            isPlayZoom = true;
                        });
                        await UniTask.WaitUntil(() => isPlayZoom, cancellationToken: token);

                        // delay 0.5
                        await UniTask.Delay(500, cancellationToken: token);
                    }

                    PopupAnswer(introStateData, token);
                }
                else
                {
                    PopupAnswer(introStateData, token);
                }
            }
            catch (Exception)
            {

            }

        }
        private async void PopupAnswer(BEPP01BlendingSoundIntroStateData introStateData, CancellationToken token)
        {
            try {
                bool isPopup = false;
                SoundChannel soundData;
                soundData = new(SoundChannel.PLAY_SOUND, dependency.introDataConfig.audioHandup, () =>
                {
                    isPopup = true;
                });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                dependency.boxAnswer.GetComponent<RectTransform>().DOAnchorPosY(dependency.pointBoxAnswerAppear.anchoredPosition.y, 0.5f);

                await UniTask.WaitUntil(() => isPopup, cancellationToken: token);
                bool isAudioCTA = false;
                soundData = new(SoundChannel.PLAY_SOUND, dependency.introDataConfig.audioGuiding, () =>
                {
                    isAudioCTA = true;
                });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                // change color underline
                BEPP01Text answerCorrect = introStateData.answerCorrect;
                List<int> listIndexUnderline = answerCorrect.GetIndexes();

                foreach(var ind in listIndexUnderline)
                {
                    GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                    BaseImage imageUnderline = underLine.GetComponentInChildren<BEPP01Image>();
                    imageUnderline.transform.parent.GetComponent<Button>().enabled = true;
                    imageUnderline.SetColor(Color.red);
                }

                //enable button
                dependency.ButtonEllie.enabled = true;
                for (int i = 0; i < dependency.ListButtonAnswer.Count; i++)
                {
                    dependency.ListButtonAnswer[i].enabled = true;
                    dependency.ListButtonAnswer[i].GetComponent<MKButton>().Enable(false);
                }
                await UniTask.WaitUntil(() => isAudioCTA, cancellationToken: token);
                isAudioCTA = false;
                soundData = new(SoundChannel.PLAY_SOUND, introStateData.audioWord, () =>
                {
                    isAudioCTA = true;
                });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => isAudioCTA, cancellationToken: token);

                TriggerFinishIntro();
            }
            catch (Exception)
            {

            }
           
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
        public override void OnApplicationPause(bool pause)
        {
            base.OnApplicationPause(pause);

            if (pause)
            {
                dependency.boxAnswer.GetComponent<RectTransform>().DOAnchorPosY(dependency.pointBoxAnswerAppear.anchoredPosition.y, 0f);
            }
            else
            {
                dependency.boxAnswer.GetComponent<RectTransform>().DOAnchorPosY(dependency.pointBoxAnswerAppear.anchoredPosition.y, 0f);
            }
        }
        private void TriggerFinishIntro()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.INTRO_STATE_FINISH, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
    }

    public class BEPP01BlendingSoundIntroStateDataObjectDependency
    {
        public RectTransform board { set; get; }
        public RectTransform boxAnswer { set; get; }
        public List<GameObject> listLine { get; set; }
        public List<Button> ListButtonAnswer { get; set; }
        public RectTransform pointBoxAnswerAppear { set; get; }
        public RectTransform ellie { set; get; }
        public Button ButtonEllie { set; get; }
        public SkeletonGraphic ellieSkeleton { set; get; }
        public RectTransform boxUnderline { set; get; }
        public RectTransform studenGroup { set; get; }
        public RectTransform BGBottom { set; get; }
        public RectTransform backGround { set; get; }
        public RectTransform pointAnswerAppear { set; get; }
        public RectTransform pointBoardMoveIntro { set; get; }
        public RectTransform pointStartBoard { set; get; }
        public RectTransform pointStudentGroupDissapear { set; get; }
        public BEPP01BlendingSoundIntroConfig introDataConfig { set; get; }
        public string ellieIntro { set; get; }
    }
    public class BEPP01BlendingSoundIntroStateData
    {
        public int numberPhonicFilled { set; get; }
        public BEPP01Text answerCorrect { set; get; }
        public AudioClip audioWord { set; get; }
    }
}