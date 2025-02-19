using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSClickState : FSMState
    {
        private BEPS01RFSClickStateObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSClickStateEventData clickStateEventData = (BEPS01RFSClickStateEventData)data;
            DoWork(clickStateEventData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSClickStateObjectDependency)data;
        }

        private async void DoWork(BEPS01RFSClickStateEventData dataClick)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            dependency.Guiding.ResetGuiding();
            dependency.ButtonCatSpaceship.Enable(false);
            dependency.ButtonCatStation.Enable(false);
            if (dataClick.UserInput == BEPS01RFSUserInput.Click)
            {
                BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, false);
                try
                {
                    BEPS01RFSTubeItem objectClick = dataClick.ObjectEvent.GetComponent<BEPS01RFSTubeItem>();
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.ClickConfig.sfxClick);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.Delay(dependency.ClickConfig.timeDelay, cancellationToken: cts.Token);
                    bool tscAudio = false;
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, objectClick.GetData().audio, () => { tscAudio = true; });
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
                }
                catch (OperationCanceledException ex)
                {
                    LogMe.Log("Lucanhtai ex: " + ex);
                }
            }else if(dataClick.UserInput == BEPS01RFSUserInput.ClickCat)
            {
                bool tscAudio = false;
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.ClickConfig.sfxCat, () => { tscAudio = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
            }
            else
            {
                BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
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
    public class BEPS01RFSClickStateEventData
    {
        public GameObject ObjectEvent { get; set; }
        public BEPS01RFSUserInput UserInput { get; set; }
    }

    public class BEPS01RFSClickStateObjectDependency
    {
        public BEPS01RFSClickConfig ClickConfig { get; set; }
        public BEPS01RFSGuiding Guiding { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public BaseButton ButtonSkipGuiding { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
    }
}
