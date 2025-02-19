using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSIntroState : FSMState
    {
        private BERV01FTSIntroStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private float[] timesDelayFish = { 0f, 1.5f, 2.5f, 3.5f };

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BERV01FTSIntroStateData introStateData = (BERV01FTSIntroStateData)data;
            DoWork(introStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BERV01FTSIntroStateObjectDependency)data;
        }

        private async void DoWork(BERV01FTSIntroStateData introStateData)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            try
            {
                timesDelayFish.Shuffle();
                float valueDelayFishSecond = 0;
                for (int i = 0; i < dependency.FishesPlay.Count; i++)
                {
                    float randomValue = timesDelayFish[i];
                    if (i == 2) valueDelayFishSecond = randomValue;
                    dependency.FishesPlay[i].EnableMovingFish(true, randomValue);
                }
                await UniTask.Delay((int)(valueDelayFishSecond * 1000), cancellationToken: cts.Token);
                await UniTask.Delay(dependency.IntroConfig.timeDelay + dependency.IntroConfig.timeDelay / 2, cancellationToken: cts.Token);
                bool tscAudio = false;
                AudioClip audioCta = dependency.IntroConfig.audioCTAPhonic;

                audioCta = (introStateData.TypeData == TypeWord.Word) 
                    ? audioCta = dependency.IntroConfig.audioCTAWord 
                    : audioCta = dependency.IntroConfig.audioCTAPhonic;
                
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, audioCta, () => { tscAudio = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                tscAudio = false;
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, introStateData.AudioData, () => { tscAudio = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                for (int i = 0; i < dependency.FishesPlay.Count; i++)
                {
                    dependency.FishesPlay[i].IsEnable = true;
                }

                BERV01FTSHandleData.TriggerFinishState(BERV01State.PlayGame, null);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }


        public override void OnExit()
        {
            base.OnExit();
            SoundManager.Instance.StopFx();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }
    }
    public class BERV01FTSIntroStateData
    {
        public AudioClip AudioData { get; set; }
        public TypeWord TypeData { get; set; }
    }


    public class BERV01FTSIntroStateObjectDependency
    {
       public BERV01FTSIntroConfig IntroConfig { get; set; }
       public List<BERV01FTSFish> FishesPlay { get; set; }
    }
}