using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSEndGameState : FSMState
    {
        private BEPS01RFSEndGameStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private bool isSpaceShip = false;
        private float lastProgress = 0f;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            DoWork();
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSEndGameStateObjectDependency)data;
        }

        private async void DoWork()
        {
            cts = new CancellationTokenSource();
            try
            {
                bool tscAnimationDone = false;
                dependency.LoopBackground.StartLoop();
                isSpaceShip = true;
                dependency.Spaceship.GetSkeleton().AnimationState.Event += HandleEvent;
                BEPS01RFSHandleData.SetAnimation(dependency.Spaceship.GetSkeleton(), dependency.SkeletonConfig.spaceshipMoveToEarth, false, (trackEntry) =>
                {
                    tscAnimationDone = true;
                    dependency.LoopBackground.StopLoop();
                });
                Vector3 targetPosition = new Vector3(dependency.PointResetTube.position.x, dependency.PointDownSpaceStation.position.y, dependency.LayoutTube.transform.position.z);
                dependency.LayoutTube.transform.DOMove(targetPosition, 1.5f).SetEase(Ease.InOutQuad).onComplete += () => {
                    dependency.LayoutTube.transform.position = dependency.PointResetTube.position;
                };
                dependency.SpaceStation.transform.DOMoveY(dependency.PointDownSpaceStation.position.y, 1.5f).SetEase(Ease.InOutQuad);

                await UniTask.WaitUntil(() => tscAnimationDone, cancellationToken: cts.Token);
                await UniTask.Delay(1000, cancellationToken: cts.Token);
                BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.FinishGame, null);

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
            if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_FLY_3, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipFly_3);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_SPACESHIP_ON, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxSpaceshipOn);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
            else if (e.Data.Name.Equals(BEPS01RFSHandleData.EVENT_CHEER, StringComparison.OrdinalIgnoreCase))
            {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SkeletonConfig.sfxCheer);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            }
        }
        private void CheckProgress(float progress)
        {
            if (progress >= 0.1f && lastProgress < 0.1f)
            {
                lastProgress = 0.1f;
                dependency.LoopBackground.SetSpeed(600);
            }
            else if (progress >= 0.2f && lastProgress < 0.2f)
            {
                lastProgress = 0.2f;
                dependency.LoopBackground.SetSpeed(900);
            }
            else if (progress >= 0.25f && lastProgress < 0.25f)
            {
                lastProgress = 0.25f;
                dependency.LoopBackground.SetSpeed(1300);
            }
            else if (progress >= 0.3f && lastProgress < 0.3f)
            {
                lastProgress = 0.3f;
                dependency.LoopBackground.SetSpeed(1600);
            }
            else if (progress >= 0.4f && lastProgress < 0.4f)
            {
                lastProgress = 0.4f;
                dependency.LoopBackground.SetSpeed(1800);
            }
            else if (progress >= 0.45f && lastProgress < 0.45f)
            {
                lastProgress = 0.45f;
                dependency.LoopBackground.SetSpeed(1700);
            }
            else if (progress >= 0.5f && lastProgress < 0.5f)
            {
                lastProgress = 0.5f;
                dependency.LoopBackground.SetSpeed(1600);
            }
            else if (progress >= 0.55f && lastProgress < 0.55f)
            {
                lastProgress = 0.55f;
                dependency.LoopBackground.SetSpeed(1400);
            }
            else if (progress >= 0.6f && lastProgress < 0.6f)
            {
                lastProgress = 0.6f;
                dependency.LoopBackground.SetSpeed(1100);
            } 
            else if (progress >= 0.65f && lastProgress < 0.66f)
            {
                lastProgress = 0.66f;
                dependency.LoopBackground.SetSpeed(800);
            }
            else if (progress >= 0.7f && lastProgress < 0.7f)
            {
                lastProgress = 0.7f;
                dependency.LoopBackground.SetSpeed(400);
            }
            else if (progress >= 0.73f && lastProgress < 0.75f)
            {
                lastProgress = 0.75f;
                dependency.LoopBackground.SetSpeed(150);
            }
            else if (progress >= 0.76f && lastProgress < 0.76f)
            {
                lastProgress = 0.76f;
                dependency.LoopBackground.SetSpeed(20);
            }
            else if (progress >= 0.8f && lastProgress < 0.8f)
            {
                lastProgress = 0.8f;
                isSpaceShip = false;
                dependency.LoopBackground.StopLoop();
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
    public class BEPS01RFSEndGameStateObjectDependency
    {
        public BEPS01RFSSkeletonConfig SkeletonConfig { get; set; }
        public Transform PointResetTube { get; set; }
        public Transform PointDownSpaceStation { get; set; }
        public HorizontalLayoutGroup LayoutTube { get; set; }
        public BEPS01RFSSpaceship Spaceship { get; set; }
        public Transform SpaceStation { get; set; }
        public BEPS01RFSLoopBackground LoopBackground { get; set; }
    }
}