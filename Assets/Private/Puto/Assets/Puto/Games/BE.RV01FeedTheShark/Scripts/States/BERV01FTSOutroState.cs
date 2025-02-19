using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSOutroState : FSMState
    {
        private BERV01FTSOutroStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private float sharkSpeedOutro = 8f;
        private float fishSpeedOutro = 15f;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            DoWork();
        }

        public override void SetUp(object data)
        {
            dependency = (BERV01FTSOutroStateObjectDependency)data;
        }

        private async void DoWork()
        {
            cts = new CancellationTokenSource();
            SoundManager.Instance.StopFxOneShot();
            try
            {
                bool tscmoveDone = false;
                bool tscAudio = false;
                int randomIndex = UnityEngine.Random.Range(0, dependency.SharkPooling.ListAllShark.Count);

                GameObject sharkOutro = dependency.SharkPooling.ListAllShark[randomIndex];
                sharkOutro.SetActive(true);
                sharkOutro.transform.SetAsLastSibling();
                bool isCloserToLeft = BERV01FTSHandleData.IsCloserToLeftEdge(dependency.CameraGame, sharkOutro);
                BERV01FTSHandleData.SetPivot(sharkOutro.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f));
                sharkOutro.transform.localRotation = new Quaternion(0f, isCloserToLeft ? 180f : 0f , 0f, 0f);
                sharkOutro.transform.position = new Vector3(sharkOutro.transform.position.x, dependency.PointSharkCenter.position.y, sharkOutro.transform.position.y);

                SkeletonGraphic skeletonShark = sharkOutro.GetComponent<SkeletonGraphic>();
                sharkOutro.transform.DOKill();
                sharkOutro.transform.DOMoveX(dependency.PointSharkCenter.transform.position.x, sharkSpeedOutro).SetEase(Ease.OutQuad).SetSpeedBased().onComplete += () =>
                 {
                     tscmoveDone = true;
                 };

                BERV01FTSFish[] allFishes = dependency.UIFish.GetComponentsInChildren<BERV01FTSFish>(true);

                List<BERV01FTSFish> activeFishes = new List<BERV01FTSFish>();
                foreach (BERV01FTSFish fish in allFishes)
                {
                    if (fish.gameObject.activeInHierarchy)
                    {
                        activeFishes.Add(fish);
                    }
                }

                foreach (var fish in activeFishes)
                {
                    fish.IsEnable = false;
                    fish.IsPlaying = true;
                    fish.StopAllCoroutines();
                    BERV01FTSHandleData.SetAnimation(fish.SkeletonFish, BERV01FTSHandleData.fishIdle, true, null);
                    fish.MoveFishWithSpeed(fishSpeedOutro);

                }
                dependency.UIParticle.gameObject.SetActive(true);
                dependency.EffectOutro.Play();
                await UniTask.WaitUntil(() => tscmoveDone, cancellationToken: cts.Token);
                BERV01FTSHandleData.SetAnimation(skeletonShark, BERV01FTSHandleData.sharkHappy, true, null);
                SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.OutroConfig.sfxWin, () => { tscAudio = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
              
                await UniTask.WaitUntil(() => tscAudio, cancellationToken: cts.Token);
                await UniTask.Delay(dependency.OutroConfig.timeDelay, cancellationToken: cts.Token);
                BERV01FTSHandleData.TriggerFinishState(BERV01State.FinishGame, null);
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


    public class BERV01FTSOutroStateObjectDependency
    {
        public BERV01FTSOutroConfig OutroConfig { get; set; }
        public BERV01FTSSharkPooling SharkPooling { get; set; }
        public Camera CameraGame { get; set; }
        public Transform UIFish { get; set; }
        public Transform PointSharkCenter { get; set; }
        public UIParticle UIParticle { get; set; }
        public ParticleSystem EffectOutro { get; set; }
    }
}