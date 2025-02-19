using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSIntroState : FSMState
    {
        private BEPS01RFSIntroStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private bool isSpaceShip = false;
        private float lastProgress = 0f;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSIntroStateData introData = (BEPS01RFSIntroStateData)data;
            DoWork(introData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSIntroStateObjectDependency)data;
        }

        private async void DoWork(BEPS01RFSIntroStateData introData)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            try
            {
                bool tscMoveDone = false;
                if (!introData.IsSkipCTA)
                {
                    await UniTask.Delay(50, cancellationToken: cts.Token);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.LayoutTube.GetComponent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.LayoutDashBox.GetComponent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.LayoutTextTube.GetComponent<RectTransform>());
                    DisableLayoutGroupTube();
                    DisableLayoutGroupDashBox();
                    DisableLayoutGroupTextTube();
                    await UniTask.Delay(dependency.IntroConfig.timeDelay - 50, cancellationToken: cts.Token);
                    if(introData.CurrentTurn == 0)
                    {
                        dependency.Spaceship.GetComponent<CanvasGroup>().alpha = 1f;
                        bool tscAnimDone = false;
                        isSpaceShip = true;
                        dependency.Spaceship.GetSkeleton().AnimationState.Event += HandleEvent;
                        BEPS01RFSHandleData.SetAnimation(dependency.Spaceship.GetSkeleton(), dependency.SkeletonConfig.spaceshipMoveToFirstStation, false, (trackEntry) => {
                            dependency.Spaceship.StartStateIdle();
                            tscAnimDone = true;
                            isSpaceShip = false;
                        });
                        await UniTask.WaitUntil(() => tscAnimDone, cancellationToken: cts.Token);
                    }else
                    {
                        dependency.LayoutTube.transform.DOMoveX(dependency.PointMoveTube.position.x, 1.5f).SetEase(Ease.InOutQuad).onComplete += () => {
                            tscMoveDone = true;
                        };
                        dependency.LayoutDashBox.transform.DOMoveX(dependency.PointMoveTube.position.x, 1.5f).SetEase(Ease.InOutQuad);
                        await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                    }
                   
                    if (introData.CurrentTurn == 0) dependency.IntroConfig.audiosTopic.Shuffle();
                    await UniTask.Delay(dependency.IntroConfig.timeDelay, cancellationToken: cts.Token);
                    bool tscAudio = false;
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, introData.AudioSentence, () => { tscAudio = true; }, 1f);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                    if (introData.CurrentTurn == 0)
                    {
                        tscAudio = false;
                        dependency.ButtonSkipCTA.Enable(true);
                        int randomIndex = UnityEngine.Random.RandomRange(0, dependency.IntroConfig.audiosTopic.Length);
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.IntroConfig.audiosTopic[randomIndex], () => { tscAudio = true; }, 1f);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                        dependency.ButtonSkipCTA.Enable(false);
                        dependency.ButtonSkipCTA.gameObject.SetActive(false);
                    }
                    await UniTask.Delay(dependency.IntroConfig.timeDelay * 2, cancellationToken: cts.Token);
                }
                else {
                    SoundManager.Instance.StopFxOneShot();
                    dependency.ButtonSkipCTA.Enable(false);
                    dependency.ButtonSkipCTA.gameObject.SetActive(false);
                }
                dependency.LayoutDashBox.GetComponent<CanvasGroup>().alpha = 1f;
                List<BEPS01RFSTubeItem> listRandomTube = BEPS01RFSHandleData.RandomSelectItems(dependency.TubeItems, introData.MaxTubeDrag);
                List<BEPS01RFSTubeItem> listExceptTube = dependency.TubeItems.Except(listRandomTube).ToList();
                BEPS01RFSHandleData.SetDragTube(listRandomTube, true);
                BEPS01RFSHandleData.SetDragTube(listExceptTube, false);

                foreach(var itemTube in listExceptTube)
                {
                    dependency.DashBoxItems.Find((item) => BEPS01RFSHandleData.AreIntegersEqual(item.GetId(), itemTube.GetData().id)).Fade(0f);
                }

                List<BEPS01RFSDashBoxItem> filteredDashBox = dependency.DashBoxItems.Where(x => x.GetAlpha() != 0f).ToList();
                List<Transform> pointMoves = BEPS01RFSHandleData.GetRandomPoints(dependency.PointsRandom, introData.MaxTubeDrag);
                int count = Mathf.Min(listRandomTube.Count, pointMoves.Count);
                pointMoves = pointMoves.OrderBy(x => UnityEngine.Random.value).ToList();
                tscMoveDone = false;
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxImpact);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                for (int i = 0; i < count; i++) 
                {
                    Transform item = listRandomTube[i].transform;
                    Transform targetPoint = pointMoves[i];
                    listRandomTube[i].InitData(filteredDashBox, targetPoint.GetComponent<RectTransform>());
                    item.DOMove(targetPoint.position, 0.75f).SetEase(Ease.OutBack).onComplete += () => { 
                        tscMoveDone = true; 
                    
                    }; 
                }
                await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);

                BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, true);

                BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isSpaceShip)
            {
                var trackEntry = dependency.Spaceship.GetSkeleton().AnimationState.GetCurrent(0);
                if (trackEntry != null)
                {
                    float currentTime = trackEntry.AnimationTime;
                    float totalDuration = trackEntry.Animation.Duration;

                    float progress = currentTime / totalDuration;

                    CheckProgress(progress);
                }
            }

        }
        private void CheckProgress(float progress)
        {
            if (progress >= 0.3f && lastProgress < 0.3f)
            {
                lastProgress = 0.3f;
                dependency.LayoutTube.transform.DOMoveX(dependency.PointMoveTube.position.x, 1.5f).SetEase(Ease.InOutQuad).onComplete += () => {
                };
                dependency.LayoutDashBox.transform.DOMoveX(dependency.PointMoveTube.position.x, 1.5f).SetEase(Ease.InOutQuad);
            }
        }


        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            SoundChannel soundData;
            if(e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_FLY_1, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipFly_1);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_OFF, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipOff);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
        }

        private void DisableLayoutGroupDashBox()
        {
            int countDashbox = dependency.DashBoxItems.Count;
            Vector3[] arrPosDashBox = new Vector3[countDashbox];
            for (int i = 0; i < countDashbox; i++)
            {
                Vector3 vector = dependency.DashBoxItems[i].GetComponent<RectTransform>().anchoredPosition;
                arrPosDashBox[i] = vector;
            }

            RectTransform parentLayout = dependency.LayoutDashBox.GetComponent<RectTransform>();
            float widthParent = parentLayout.sizeDelta.x;

            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutDashBox, false);

            parentLayout.sizeDelta = new Vector2(widthParent, parentLayout.sizeDelta.y);

            for (int i = 0; i < countDashbox; i++)
            {
                RectTransform phonicRectTransform = dependency.DashBoxItems[i].GetComponent<RectTransform>();
                phonicRectTransform.anchoredPosition = arrPosDashBox[i];
            }

            for (int i = 0; i < dependency.DashBoxItems.Count; i++)
            {
                dependency.DashBoxItems[i].transform.position = new Vector3(dependency.DashBoxItems[i].transform.position.x,
                    dependency.TransformPlaceTube.position.y,
                    dependency.DashBoxItems[i].transform.position.z);
            }
        }
        private void DisableLayoutGroupTube()
        {
            int countTube = dependency.TubeItems.Count;
            Vector3[] arrPosTube = new Vector3[countTube];
            for (int i = 0; i < countTube; i++)
            {
                Vector3 vector = dependency.TubeItems[i].GetComponent<RectTransform>().anchoredPosition;
                arrPosTube[i] = vector;
            }

            int countTubeEmpty = dependency.TubeEmptyItems.Count;
            Vector3[] arrPosTubeEmpty = new Vector3[countTubeEmpty];

            for (int i = 0; i < countTubeEmpty; i++)
            {
                Vector3 vector = dependency.TubeEmptyItems[i].GetComponent<RectTransform>().anchoredPosition;
                arrPosTubeEmpty[i] = vector;
            }

            RectTransform parentLayout = dependency.LayoutTube.GetComponent<RectTransform>();
            float widthParent = parentLayout.sizeDelta.x;

            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutTube, false);

            parentLayout.sizeDelta = new Vector2(widthParent, parentLayout.sizeDelta.y);

            for (int i = 0; i < countTube; i++)
            {
                RectTransform phonicRectTransform = dependency.TubeItems[i].GetComponent<RectTransform>();
                phonicRectTransform.anchoredPosition = arrPosTube[i];
            }

            for (int i = 0; i < dependency.TubeItems.Count; i++)
            {
                dependency.TubeItems[i].transform.position = new Vector3(dependency.TubeItems[i].transform.position.x,
                    dependency.TransformPlaceTube.position.y,
                    dependency.TubeItems[i].transform.position.z);
            }

            for (int i = 0; i < countTubeEmpty; i++)
            {
                RectTransform phonicRectTransform = dependency.TubeEmptyItems[i].GetComponent<RectTransform>();
                phonicRectTransform.anchoredPosition = arrPosTubeEmpty[i];
            }

            for (int i = 0; i < dependency.TubeEmptyItems.Count; i++)
            {
                dependency.TubeEmptyItems[i].transform.position = new Vector3(dependency.TubeEmptyItems[i].transform.position.x,
                    dependency.TransformPlaceTube.position.y,
                    dependency.TubeEmptyItems[i].transform.position.z);
            }


        }

        private void DisableLayoutGroupTextTube()
        {
            int countTextTube = dependency.TextsTubePoint.Count;
            Vector3[] arrPosTextTube = new Vector3[countTextTube];
            for (int i = 0; i < countTextTube; i++)
            {
                Vector3 vector = dependency.TextsTubePoint[i].GetComponent<RectTransform>().anchoredPosition;
                arrPosTextTube[i] = vector;
            }

            RectTransform parentLayout = dependency.LayoutTextTube.GetComponent<RectTransform>();
            float widthParent = parentLayout.sizeDelta.x;

            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutTextTube, false);

            parentLayout.sizeDelta = new Vector2(widthParent, parentLayout.sizeDelta.y);

            for (int i = 0; i < countTextTube; i++)
            {
                RectTransform phonicRectTransform = dependency.TextsTubePoint[i].GetComponent<RectTransform>();
                phonicRectTransform.anchoredPosition = arrPosTextTube[i];
            }

            for (int i = 0; i < dependency.TextsTubePoint.Count; i++)
            {
                dependency.TextsTubePoint[i].transform.position = new Vector3(dependency.TextsTubePoint[i].transform.position.x,
                    dependency.TransformPlaceText.position.y,
                    dependency.TextsTubePoint[i].transform.position.z);
            }

        }
        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
            isSpaceShip = false;
            lastProgress = 0f;
            dependency.Spaceship.GetSkeleton().AnimationState.Event -= HandleEvent;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }
    }

    public class BEPS01RFSIntroStateData
    {
        public AudioClip AudioSentence { get; set; }
        public bool IsSkipCTA { get; set; }
        public int CurrentTurn { get; set; }
        public int MaxTubeDrag { get; set; }
    }

    public class BEPS01RFSIntroStateObjectDependency
    {
        public BEPS01RFSIntroConfig IntroConfig { get; set; }
        public BEPS01RFSSkeletonConfig SkeletonConfig { get; set; }
        public Transform PointMoveTube { get; set; }
        public Transform TransformPlaceTube { get; set; }
        public Transform TransformPlaceText { get; set; }
        public HorizontalLayoutGroup LayoutTube { get; set; }
        public HorizontalLayoutGroup LayoutDashBox { get; set; }
        public HorizontalLayoutGroup LayoutTextTube{ get; set; }
        public BEPS01RFSSpaceship Spaceship { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public List<BEPS01RFSTubeItem> TubeEmptyItems { get; set; }
        public List<BEPS01RFSDashBoxItem> DashBoxItems { get; set; }
        public List<Transform> TextsTubePoint { get; set; }
        public BaseButton ButtonSkipCTA { get; set; }
        public List<Transform> PointsRandom { get; set; }
    }
}