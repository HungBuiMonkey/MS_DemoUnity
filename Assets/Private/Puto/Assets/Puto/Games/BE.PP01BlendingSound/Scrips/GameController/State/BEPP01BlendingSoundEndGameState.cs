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
    public class BEPP01BlendingSoundEndGameState : FSMState
    {
        private BEPP01BlendingSoundEndGameStateDataObjectDependency dependency;
        private CancellationTokenSource cts;
        private float secondTimeZoom = 1.5f;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundEndGameStateData endGameStateData = (BEPP01BlendingSoundEndGameStateData)data;
            cts = new();
            DoWork(endGameStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundEndGameStateDataObjectDependency)data;
        }

        private async void DoWork(BEPP01BlendingSoundEndGameStateData endGameStateData)
        {
            cts = new();
            try
            {
                dependency.ellieSkeleton.GetComponentInChildren<BaseButton>().Enable(false);
                for (int i = 0; i < dependency.listLine.Count; i++)
                {
                    dependency.listLine[i].GetComponent<BaseButton>().Enable(false);
                }

                SoundChannel soundData;
                Color colorFadeOutGame = new(1, 1, 1, 1);
                Vector3 targetZoomIpad = new(1.45f, 1.45f, 1.45f);
                Vector3 targetZoomPhone = new(1.3f, 1.3f, 1.3f);
                int positionReduceIpad = 120;
                int positionReducePhone = 70;
                LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.backGround);
                float ratio = dependency.backGround.rect.x / dependency.backGround.sizeDelta.y;
                bool isPlayZoom = false;
                if (ratio <= 1.6f)
                {
                    dependency.board.DOScale(targetZoomIpad, secondTimeZoom);
                    dependency.ellie.DOAnchorPosY(dependency.ellie.anchoredPosition.y - positionReduceIpad, secondTimeZoom);
                    dependency.BGBottom.DOAnchorPosY(dependency.BGBottom.anchoredPosition.y - positionReduceIpad, secondTimeZoom).OnComplete(() =>
                    {
                        isPlayZoom = true;
                    });
                }
                else
                {
                    dependency.board.DOScale(targetZoomPhone, secondTimeZoom);
                    dependency.ellie.DOAnchorPosY(dependency.ellie.anchoredPosition.y - positionReducePhone, secondTimeZoom);
                    dependency.BGBottom.DOAnchorPosY(dependency.BGBottom.anchoredPosition.y - positionReducePhone, secondTimeZoom).OnComplete(() =>
                    {
                        isPlayZoom = true;
                    });
                }

                await UniTask.WaitUntil(() => isPlayZoom, cancellationToken: cts.Token);
                await UniTask.Delay(250, cancellationToken: cts.Token);
                dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieEndGame, false).Complete += (trackEntry) =>
                {
                    dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieEndGameToNormal, false).Complete += (trackEntry) =>
                    {
                        dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieNormal, true);
                    };
                };

                bool isSyncText = false;
                List<BEPP01Text> listTextAlphabet = endGameStateData.lstBEPP01TextAlphabet.ToList();
                listTextAlphabet = listTextAlphabet.OrderBy(item => item.GetData().turn).ToList();

                for (int i = 0; i < listTextAlphabet.Count; i++)
                {
                    List<int> listIndexUnderline = listTextAlphabet[i].GetIndexes();
                    foreach (var ind in listIndexUnderline)
                    {
                        GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                        BEPP01Text textUnderline = underLine.GetComponent<BEPP01Text>();
                        textUnderline.ChangeColorText();
                    }
                    soundData = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, listTextAlphabet[i].GetData().audioAnswer, () => {
                        isSyncText = true;
                    });
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.WaitUntil(() => isSyncText, cancellationToken: cts.Token);
                    await UniTask.Delay(500, cancellationToken: cts.Token);
                    foreach (var ind in listIndexUnderline)
                    {
                        GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                        BEPP01Text textUnderline = underLine.GetComponent<BEPP01Text>();
                        textUnderline.ResetColorText();
                    }
                    isSyncText = false;

                }
                await UniTask.Delay(500, cancellationToken: cts.Token);
                // SyncText word
                bool isSyncTextWord = false;
                soundData = new(SoundChannel.PLAY_SOUND_NEW_OBJECT, endGameStateData.audioWord, () => { isSyncTextWord = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                for (int i = 0; i < dependency.listLine.Count; i++)
                {
                    dependency.listLine[i].GetComponent<BEPP01Text>().ChangeColorText();
                }
                await UniTask.WaitUntil(() => isSyncTextWord, cancellationToken: cts.Token);
                await UniTask.Delay(500, cancellationToken: cts.Token);
                for (int i = 0; i < dependency.listLine.Count; i++)
                {
                    dependency.listLine[i].GetComponent<BEPP01Text>().ResetColorText();
                }

                await UniTask.Delay(2000, cancellationToken: cts.Token);
                dependency.FadeOutGame.EnableImage(true);
                dependency.FadeOutGame.SetColor(colorFadeOutGame, 1.5f);

                BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.FINISH_GAME, endGameStateData.WordList);
                ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);

            }
            catch (Exception)
            {

            }
        }
       


        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts.Cancel();
            cts.Dispose();
        }
    }
    public class BEPP01BlendingSoundEndGameStateData
    {
        public List<BEPP01Text> lstBEPP01TextAlphabet { get; set; }
        public List<BEPP01AnswerData> listData { get; set; }
        public AudioClip audioWord { get; set; }
        public List<UserEndGameData.Word> WordList { get; set; }
    }

    public class BEPP01BlendingSoundEndGameStateDataObjectDependency
    {
        public List<Button> ListButtonAnswer { get; set; }
        public List<GameObject> listLine { get; set; }
        public RectTransform ellie { get; set; }
        public BEPP01Image FadeOutGame { get; set; }
        public SkeletonGraphic ellieSkeleton { get; set; }
        public RectTransform BGBottom { get; set; }
        public RectTransform backGround { get; set; }
        public RectTransform board { get; set; }
        public string ellieEndGame { set; get; }
        public string ellieEndGameToNormal { set; get; }
        public string ellieNormal { set; get; }
    }
}