using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsDraggingState : FSMState
    {
        private BEPS02FlyingOwlsDraggingStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private BEPS02OwlDrag objectDrag;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS02FlyingOwlsDraggingStateData draggingStateData = (BEPS02FlyingOwlsDraggingStateData)data;
            DoWord(draggingStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsDraggingStateObjectDependency)data;
        }

        private async void DoWord(BEPS02FlyingOwlsDraggingStateData dataDragging)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            dependency.GuidingHand.ResetGuiding();
            objectDrag = dataDragging.EventData.ObjectEvent.GetComponent<BEPS02OwlDrag>();
            //BEPS02HandleData.EnableOwls(dependency.ListOwls, objectDrag.name, false);
            BEPS02HandleData.FadeShadowOwls(dependency.ListShadowOwls);
            try
            {
                int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(objectDrag.GetSkeleton());
                BEPS02HandleData.SetNumberAnimation(objectDrag.GetSkeleton(), dependency.OwlConfig.owlUserTap, aniIndex, false, null);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, objectDrag.GetData().audio, null, 1, false);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => objectDrag.GetSkeleton().AnimationState.GetCurrent(0).IsComplete, cancellationToken: cts.Token);
                BEPS02HandleData.SetNumberAnimation(objectDrag.GetSkeleton(), dependency.OwlConfig.owlTapAndHold, aniIndex, true, null);

            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
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
            cts?.Cancel();
            cts?.Dispose();
        }

    }
    public class BEPS02FlyingOwlsDraggingStateData
    {
        public List<AudioClip> AudioWords { get; set; }
        public BEPS02FlyingOwlsDraggingStateEventData EventData { get; set; }
    }

    public class BEPS02FlyingOwlsDraggingStateEventData
    {
        public GameObject ObjectEvent { get; set; }
    }


    public class BEPS02FlyingOwlsDraggingStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
    }
}