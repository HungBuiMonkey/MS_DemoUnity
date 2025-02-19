using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundClickUnderlineState : FSMState
    {
        private BEPP01BlendingSoundClickUnderlineStateDataObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundClickUnderlineStateEventData clickDataEvent = (BEPP01BlendingSoundClickUnderlineStateEventData)data;
            cts = new();
            DoWork(clickDataEvent);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundClickUnderlineStateDataObjectDependency)data;
        }

        private async void DoWork(BEPP01BlendingSoundClickUnderlineStateEventData data)
        {
            try
            {
                dependency.LockButtonUnderline.enabled = true;
                bool isPlayingSound = false;
                AudioClip audioAnswerClick = data.underlineClick.GetData().audioAnswer;
                SoundChannel soundDataAnswerClick = new(SoundChannel.PLAY_SOUND, audioAnswerClick, () => { isPlayingSound = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundDataAnswerClick);
                await UniTask.WaitUntil(() => isPlayingSound, cancellationToken: cts.Token);
                dependency.LockButtonUnderline.enabled = false;
                TriggerFinsihClickUnderline();
            }
            catch (OperationCanceledException ex)
            {

            }


        }
        private void TriggerFinsihClickUnderline()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.GUIDING_STATE, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        public override void OnExit()
        {
            base.OnExit();
            dependency.LockButtonUnderline.enabled = false;
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts.Cancel();
            cts.Dispose();
        }
    }

    public class BEPP01BlendingSoundClickUnderlineStateEventData
    {
        public BEPP01Text underlineClick { get; set; }
    }
    public class BEPP01BlendingSoundClickUnderlineStateDataObjectDependency
    {
        public RectTransform boxUnderline { get; set; }
        public Image LockButtonUnderline { get; set; }
    }
}