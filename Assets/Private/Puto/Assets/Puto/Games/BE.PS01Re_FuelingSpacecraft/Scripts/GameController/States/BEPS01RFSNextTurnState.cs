using Cysharp.Threading.Tasks;
using DataModel;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSNextTurnState : FSMState
    {
        private BEPS01RFSNextTurnStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private string catAnimation = "";
        private string spaceshipAnimation = "";
        private bool isSpaceShip = false;
        private float lastProgress = 0f;
        private BEPS01RFSNextTurnStateData nextTurnStateData;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            nextTurnStateData = (BEPS01RFSNextTurnStateData)data;
            DoWork(nextTurnStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSNextTurnStateObjectDependency)data;
        }

        private async void DoWork(BEPS01RFSNextTurnStateData nextTurnStateData)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            try
            {
                dependency.Guiding.ResetGuiding();
                BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, false);
                dependency.ButtonCatSpaceship.Enable(false);
                dependency.ButtonCatStation.Enable(false);

                dependency.Spaceship.GetSkeleton().AnimationState.Event += HandleEvent;
                await UniTask.Delay(dependency.NextTurnConfig.timeDelay, cancellationToken: cts.Token);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.NextTurnConfig.sfxCelestialWin);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                bool tscAnimationDone = false;
                bool tscMoveDone = false;
                Transform catTransform = dependency.CatSkeleton.transform;

                catAnimation = (nextTurnStateData.TurnGame == 0) ? dependency.SkeletonConfig.catYellowMoveToStation : dependency.SkeletonConfig.catGrayMoveToStation;
                BEPS01RFSHandleData.SetAnimation(dependency.CatSkeleton, catAnimation, false, (trackEntry) => {
                    tscAnimationDone = true;
                    catTransform.position = new Vector3(dependency.TransformPlaceCatStart.position.x, dependency.TransformPlaceCatStart.position.y, catTransform.position.z);
                });

                for (int i = 0; i < dependency.TubeItems.Count; i++)
                {
                    TMP_Text text = dependency.TubeItems[i].GetText();
                    text.gameObject.AddComponent<Canvas>();
                    string textContent = text.text;
                    if (textContent.Contains("\n"))
                    {
                        textContent = textContent.Replace("\n", " "); 
                        text.text = textContent; 
                    }
                    text.ForceMeshUpdate();
                    text.GetComponent<Canvas>().overrideSorting = true;
                    text.GetComponent<Canvas>().sortingOrder = BEPS01RFSHandleData.SORTING_ORDER_DRAG;
                    text.transform.SetParent(dependency.LayoutTube.transform);
                    text.transform.SetAsLastSibling();
                    text.transform.DOMove(dependency.TextsTubePoint[i].position, 1f).SetEase(Ease.InOutCubic).onComplete += () =>
                    {
                        tscMoveDone = true;
                    };
                }

                await UniTask.WaitUntil(() => tscAnimationDone && tscMoveDone, cancellationToken: cts.Token);
                bool tscAudioSentence = false;
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, nextTurnStateData.AudioSentence, () => { tscAudioSentence = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);

                catAnimation = (nextTurnStateData.TurnGame == 0) ? dependency.SkeletonConfig.catYellowMoveToSpaceship : dependency.SkeletonConfig.catGrayMoveToSpaceship;
                BEPS01RFSHandleData.SetAnimation(dependency.CatSkeleton, catAnimation, true, null);
                bool tscCatMoveDone = false;
                catTransform.DOMoveX(dependency.TransformPlaceCatMove.position.x, 5f).SetEase(Ease.Linear).onComplete += () =>
                {
                    tscCatMoveDone = true;
                    catTransform.localScale = Vector3.zero;
                };
                for (int i = 0; i < dependency.TubeItems.Count; i++)
                {
                    TMP_Text textTube = dependency.TubeItems[i].GetText();
                    string firstContent = textTube.text;
                    string textResult = textTube.text;
                    textResult = BEPS01RFSHandleData.COLOR_SYNC + textTube.text + "</color>";
                    dependency.TubeItems[i].ResetSizeOutline();
                    textTube.text = textResult;
                    await UniTask.Delay(Math.Abs(nextTurnStateData.SyncDatas[i].e - nextTurnStateData.SyncDatas[i].s), cancellationToken: cts.Token);
                    textTube.text = firstContent;
                    dependency.TubeItems[i].SetSizeOutline(0f);
                }
                await UniTask.WaitUntil(() => tscAudioSentence && tscCatMoveDone, cancellationToken: cts.Token);
                await UniTask.Delay(dependency.NextTurnConfig.timeDelay * 2, cancellationToken: cts.Token);
                tscAnimationDone = false;
                spaceshipAnimation = (nextTurnStateData.TurnGame == 0) ? dependency.SkeletonConfig.catYellowSitInTheChair : dependency.SkeletonConfig.catGraySitInTheChair;
                dependency.Spaceship.StopAnimationLoop();
                BEPS01RFSHandleData.SetAnimation(dependency.Spaceship.GetSkeleton(), spaceshipAnimation, false, (trackEntry) => {
                    tscAnimationDone = true;
                });

                await UniTask.WaitUntil(() => tscAnimationDone, cancellationToken: cts.Token);
                await UniTask.Delay(dependency.NextTurnConfig.timeDelay * 2, cancellationToken: cts.Token);

                if (nextTurnStateData.TurnGame == 0)
                {
                    tscAnimationDone = false;
                    tscMoveDone = false;
                    dependency.LoopBackground.StartLoop();
                    isSpaceShip = true;
                    BEPS01RFSHandleData.SetAnimation(dependency.Spaceship.GetSkeleton(), dependency.SkeletonConfig.spaceshipMoveToSecondStation, false, (trackEntry) =>
                    {
                        tscAnimationDone = true;
                        dependency.Spaceship.InitState(dependency.SkeletonConfig.spaceshipSecondTurnIdle, dependency.SkeletonConfig.spaceshipSecondTurnIdle_Blink);
                        dependency.Spaceship.StartStateIdle();
                        isSpaceShip = false;
                        dependency.LoopBackground.StopLoop();
                    });

                    foreach (var tube in dependency.TubeItems)
                    {
                        tube.SetColorText(dependency.TextColorFade);
                    }

                    dependency.LayoutDashBox.transform.DOMoveX(dependency.PointResetTube.position.x, 1.5f).SetEase(Ease.InOutQuad);
                    Vector3 targetPosition = new Vector3(dependency.PointResetTube.position.x, dependency.PointDownSpaceStation.position.y, dependency.LayoutTube.transform.position.z);
                    dependency.LayoutTube.transform.DOMove(targetPosition, 1.5f).SetEase(Ease.InOutQuad).onComplete += () => {
                        tscMoveDone = true;
                        dependency.LayoutTube.transform.position = dependency.PointResetTube.position;
                        ClearDataTurn();
                    };

                    Transform spaceStation = dependency.SpaceStation.transform;
                    spaceStation.DOMoveY(dependency.PointDownSpaceStation.position.y, 1.5f).SetEase(Ease.InOutQuad).onComplete += () => {
                        spaceStation.position = new Vector3(spaceStation.position.x, dependency.PointUpSpaceStation.position.y, spaceStation.position.z);
                        catTransform.localScale = Vector3.one;
                        BEPS01RFSHandleData.SetAnimation(dependency.CatSkeleton, dependency.SkeletonConfig.catGrayIdle, true, null);
                        dependency.CatSkeleton.transform.position = dependency.TransformPlaceCatAppear.position;

                    };
                    await UniTask.WaitUntil(() => tscAnimationDone, cancellationToken: cts.Token);
                }else
                {
                    foreach (var tube in dependency.TubeItems)
                    {
                        tube.SetColorText(dependency.TextColorFade);
                    }
                }
              
                if (nextTurnStateData.TurnGame < (nextTurnStateData.MaxTurn - 1))
                {
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.InitData, null);
                }
                else
                {
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.EndGame, null);
                }
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

        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            SoundChannel soundData;
            if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_IMPACT, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxImpact);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_MAGIC, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxMagic);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_FLY_2, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipFly_2);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_ON, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipOn);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_OFF, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipOff);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
        }

        private void CheckProgress(float progress)
        {
            if (progress >= 0.1f && lastProgress < 0.1f)
            {
                lastProgress = 0.1f;
                dependency.LoopBackground.SetSpeed(500);
            }
            else if (progress >= 0.2f && lastProgress < 0.2f)
            {
                lastProgress = 0.2f;
                dependency.LoopBackground.SetSpeed(800);
            }
            else if (progress >= 0.3f && lastProgress < 0.3f)
            {
                lastProgress = 0.3f;
                dependency.LoopBackground.SetSpeed(1100);
            }
            else if (progress >= 0.4f && lastProgress < 0.4f)
            {
                lastProgress = 0.4f;
                dependency.LoopBackground.SetSpeed(1300);
            }
            else if (progress >= 0.5f && lastProgress < 0.5f)
            {
                lastProgress = 0.5f;
                dependency.LoopBackground.SetSpeed(1600);
            }
            else if (progress >= 0.6f && lastProgress < 0.6f)
            {
                lastProgress = 0.6f;
                dependency.LoopBackground.SetSpeed(1400);
              
            }
            else if (progress >= 0.66f && lastProgress < 0.66f)
            {
                lastProgress = 0.66f;
                dependency.LoopBackground.SetSpeed(1300);
                dependency.SpaceStation.transform.DOMoveY(dependency.PointCenterSpaceStation.position.y, 1.5f).SetEase(Ease.InOutQuad);
            }
            else if (progress >= 0.7f && lastProgress < 0.7f)
            {
                lastProgress = 0.7f;
                dependency.LoopBackground.SetSpeed(1200);
            }
            else if (progress >= 0.8f && lastProgress < 0.8f)
            {
                lastProgress = 0.8f;
                dependency.LoopBackground.SetSpeed(700);
            }
            else if (progress >= 0.85f && lastProgress < 0.85f)
            {
                lastProgress = 0.85f;
                dependency.LoopBackground.SetSpeed(400);
            }
            else if (progress >= 0.9f && lastProgress < 0.9f)
            {
                lastProgress = 0.9f;
                dependency.LoopBackground.SetSpeed(100);
            }
            else if (progress >= 0.95f && lastProgress < 0.95f)
            {
                lastProgress = 0.95f;
                dependency.LoopBackground.SetSpeed(20);
            }
        } 

        private void ClearDataTurn()
        {
            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutTextTube, true);
            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutDashBox, true);
            BEPS01RFSHandleData.EnableLayoutGroup(dependency.LayoutTube, true);
            dependency.TubeItems.Clear();
            dependency.DashBoxItems.Clear();
            dependency.TubeEmptyItems.Clear();
            dependency.TextsTubePoint.Clear();
            BEPS01RFSHandleData.DestroyItem(dependency.LayoutTextTube.transform);
            BEPS01RFSHandleData.DestroyItem(dependency.LayoutDashBox.transform);
            BEPS01RFSHandleData.DestroyItem(dependency.LayoutTube.transform);
        }



        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
            catAnimation = "";
            spaceshipAnimation = "";
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

    public class BEPS01RFSNextTurnStateData
    {
        public AudioClip AudioSentence { get; set; }
        public List<SyncData> SyncDatas { get; set; }
        public int TurnGame { get; set; }
        public int MaxTurn { get; set; }
    }


    public class BEPS01RFSNextTurnStateObjectDependency
    {
        public BEPS01RFSNextTurnConfig NextTurnConfig { get; set; } 
        public BEPS01RFSSkeletonConfig SkeletonConfig { get; set; }
        public BEPS01RFSGuiding Guiding { get; set; }
        public Color32 TextColorFade { get; set; }
        public Transform TransformPlaceCatStart { get; set; }
        public Transform TransformPlaceCatMove { get; set; }
        public Transform PointResetTube { get; set; }
        public Transform PointUpSpaceStation { get; set; }
        public Transform PointCenterSpaceStation { get; set; }
        public Transform PointDownSpaceStation { get; set; }
        public Transform TransformPlaceCatAppear { get; set; }
        public HorizontalLayoutGroup LayoutTube { get; set; }
        public HorizontalLayoutGroup LayoutDashBox { get; set; }
        public HorizontalLayoutGroup LayoutTextTube { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public List<BEPS01RFSTubeItem> TubeEmptyItems { get; set; }
        public List<BEPS01RFSDashBoxItem> DashBoxItems { get; set; }
        public List<Transform> TextsTubePoint { get; set; }
        public BEPS01RFSSpaceship Spaceship { get; set; }
        public SkeletonGraphic CatSkeleton { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
        public Transform SpaceStation { get; set; }
        public BEPS01RFSLoopBackground LoopBackground { get; set; }
    }
}