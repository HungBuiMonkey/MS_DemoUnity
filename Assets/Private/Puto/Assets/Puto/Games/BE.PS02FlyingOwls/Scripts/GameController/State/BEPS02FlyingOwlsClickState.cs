using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsClickState : FSMState
    {
        private BEPS02FlyingOwlsClickStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private BEPS02OwlDrag owlClick;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS02FlyingOwlsClickStateData clickStateData = (BEPS02FlyingOwlsClickStateData)data;

            DoWord(clickStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsClickStateObjectDependency)data;
        }

        private async void DoWord(BEPS02FlyingOwlsClickStateData dataClick)
        {
            cts = new CancellationTokenSource();
            dependency.GuidingHand.ResetGuiding();
            BEPS02HandleData.FadeShadowOwls(dependency.ListShadowOwls);
            try
            {
                bool tscAudioWord = false;
                owlClick = dataClick.EventClickData.ObjectClick.GetComponent<BEPS02OwlDrag>();
                int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlClick.GetSkeleton());

                SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND, owlClick.GetData().audio, () => {
                    owlClick.Enable(true);
                    owlClick.SetPlaying(false);
                    tscAudioWord = true;
                }, 1, false);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => tscAudioWord, cancellationToken: cts.Token);
             
                BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayGame, null);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: "+ ex);
            }
        }
     
        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
            owlClick.EnableClick(false);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }

    }
    public class BEPS02FlyingOwlsClickStateData
    {
        public List<AudioClip> AudioWords { get; set; }
        public BEPS02FlyingOwlsClickStateEventData EventClickData { get; set; }
    }

    public class BEPS02FlyingOwlsClickStateEventData
    {
        public GameObject ObjectClick { get; set; }
    }


    public class BEPS02FlyingOwlsClickStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
    }
}