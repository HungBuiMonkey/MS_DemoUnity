using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundClickEllieState : FSMState
    {
        private BEPP01BlendingSoundClickEllieStateDataObjectDependency dependency;
        private CancellationTokenSource cancellationTokenSource;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            cancellationTokenSource = new();
            CancellationToken token = cancellationTokenSource.Token;
            DoWork(token);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundClickEllieStateDataObjectDependency)data;
        }

        private async void DoWork(CancellationToken token)
        {

            dependency.ButtonEllie.enabled = false;
            dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieGuiding, true);
            try {
                bool isPlayingSound = false;
                SoundChannel soundDataGuing = new(SoundChannel.PLAY_SOUND, dependency.audioTopic, () => { isPlayingSound = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundDataGuing);
                await UniTask.WaitUntil(() => isPlayingSound, cancellationToken: token);

                dependency.ButtonEllie.enabled = true;
                TriggerFinsihClickEllie();
            }
            catch (Exception)
            {

            }

        }
        private void TriggerFinsihClickEllie()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.GUIDING_STATE, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        public override void OnExit()
        {
            base.OnExit();
            dependency.ButtonEllie.enabled = true;
            cancellationTokenSource?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

    public class BEPP01BlendingSoundClickEllieStateDataObjectDependency
    {
        public SkeletonGraphic ellieSkeleton { get; set; }
        public Button ButtonEllie { get; set; }
        public AudioClip audioTopic { get; set; }
        public string ellieGuiding { set; get; }
    }
}