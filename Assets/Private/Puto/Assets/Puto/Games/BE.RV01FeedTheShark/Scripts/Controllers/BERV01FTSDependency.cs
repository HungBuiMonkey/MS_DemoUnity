using Coffee.UIExtensions;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSDependency : Dependency
    {
        [Header("CONFIGURATION")]
        [SerializeField] private BERV01FTSConfigSO configSO;
        [SerializeField] private Camera cameraGame;
        [Header("FISH CONFIG")]
        [SerializeField] private List<BERV01FTSFish> smallFishes;
        [SerializeField] private List<BERV01FTSFish> bigFishes;
        [Header("FISH PLAY")]
        [SerializeField] private BERV01FTSSharkPooling sharkPooling;
        [SerializeField] private List<BERV01FTSFish> fishesPlay;
        [SerializeField] private List<BERV01FTSFish> fishesWait;
        [SerializeField] private List<Image> correctFishesPoint;
        [SerializeField] private List<BERV01FTSFishSwimmingLane> laneFishes;
        [SerializeField] private Transform pointSharkCenter;
        [Header("Guiding")]
        [SerializeField] private BaseButton buttonSkipGuiding;
        [SerializeField] private CanvasGroup uiGuidingHand;
        [SerializeField] private Image handLong;
        [SerializeField] private Image handSort;
        [Header("Effect")]
        [SerializeField] private UIParticle uIParticle;
        [SerializeField] private ParticleSystem effectOutroGame;
        private void Start()
        {
            SoundChannel bgMusic = new(SoundChannel.PLAY_MUSIC, configSO.audioBackground, null, 1f, true);
            ObserverManager.TriggerEvent<SoundChannel>(bgMusic);
        }
        public void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                SoundChannel bgMusic = new(SoundChannel.PLAY_MUSIC, configSO.audioBackground, null, 1f, true);
                ObserverManager.TriggerEvent<SoundChannel>(bgMusic);
            }
        }

        public override T GetStateData<T>()
        {
            T data;

            Type listType = typeof(T);
            if (listType == typeof(BERV01FTSInitStateObjectDependency))
            {
                BERV01FTSInitStateObjectDependency initData = new BERV01FTSInitStateObjectDependency();
                initData.ClickConfig = configSO.clickConfig;
                initData.SharkPooling = sharkPooling;
                initData.SmallFishes = smallFishes;
                initData.BigFishes = bigFishes;
                initData.FishesPlay = fishesPlay;
                initData.FishesWait = fishesWait;
                initData.LaneFishes = laneFishes;
                initData.CorrectFishesPoint = correctFishesPoint;
                initData.UIFish = sharkPooling.transform;

                data = ConvertToType<T>(initData);
            }
            else if (listType == typeof(BERV01FTSIntroStateObjectDependency))
            {
                BERV01FTSIntroStateObjectDependency introData = new BERV01FTSIntroStateObjectDependency();
                introData.IntroConfig = configSO.introConfig;
                introData.FishesPlay = fishesPlay;

                data = ConvertToType<T>(introData);
            }
            else if (listType == typeof(BERV01FTSPlayStateObjectDependency))
            {
                BERV01FTSPlayStateObjectDependency playData = new BERV01FTSPlayStateObjectDependency();
                playData.GuidingConfig = configSO.guidingConfig;
                playData.CameraGame = cameraGame;
                playData.FishesPlay = fishesPlay;
                playData.UiGuiding = uiGuidingHand;
                playData.ButtonSkipGuiding = buttonSkipGuiding;
                playData.HandLong = handLong;
                playData.HandShort = handSort;

                data = ConvertToType<T>(playData);
            }
            else if (listType == typeof(BERV01FTSOutroStateObjectDependency))
            {
                BERV01FTSOutroStateObjectDependency endGameData = new BERV01FTSOutroStateObjectDependency();
                endGameData.OutroConfig = configSO.outroConfig;
                endGameData.SharkPooling = sharkPooling;
                endGameData.CameraGame = cameraGame;
                endGameData.UIFish = sharkPooling.transform;
                endGameData.PointSharkCenter = pointSharkCenter;
                endGameData.UIParticle = uIParticle;
                endGameData.EffectOutro = effectOutroGame;
                data = ConvertToType<T>(endGameData);
            }
            else
            {
                data = ConvertToType<T>(null);
            }
            return data;
        }


    }
}