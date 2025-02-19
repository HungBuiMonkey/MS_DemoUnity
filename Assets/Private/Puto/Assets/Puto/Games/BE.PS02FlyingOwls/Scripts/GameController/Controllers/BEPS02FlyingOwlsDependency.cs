using Coffee.UIExtensions;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsDependency : Dependency
    {
        [Header("Configuration")]
        [SerializeField] private BEPS02FlyingOwlsScriptableObject configSO;
        [Header("Owl UI")]
        [SerializeField] private GameObject owlsBox;
        [SerializeField] private GameObject shadowOwl;
        [SerializeField] private List<BEPS02OwlDrag> listOwlConfig;
        [SerializeField] private List<BEPS02WoodenItem> listAllLandingPoint;
        [Header("Owl Config")]
        [SerializeField] private List<BEPS02OwlDrag> listOwls;
        [SerializeField] private List<TMP_Text> listTextOwl;
        [SerializeField] private List<BEPS02WoodenItem> listWoodens;
        [SerializeField] private List<BaseImage> listShadowOwls;
        [SerializeField] private GameObject pointSpawnOwl;
        [SerializeField] private GameObject owlLandingSpots;
        [SerializeField] private HorizontalLayoutGroup pointWoodensGroup;
        [SerializeField] private Image backgroundFadeOut;
        [SerializeField] private BEPS02Guiding guidingHand;
        [Header("Effect")]
        [SerializeField] private UIParticle uIParticle;
        [SerializeField] private ParticleSystem effectEndGame;
        //Test
        [SerializeField] private BEPS02TestSelectLever test;


        private void Start()
        {
            SoundChannel soundDataBg = new SoundChannel(SoundChannel.PLAY_MUSIC, configSO.audioBackground, null, 1f, true);
            ObserverManager.TriggerEvent<SoundChannel>(soundDataBg);
        }
        public void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                SoundChannel soundDataBg = new SoundChannel(SoundChannel.PLAY_MUSIC, configSO.audioBackground, null, 1f, true);
                ObserverManager.TriggerEvent<SoundChannel>(soundDataBg);
            }
        }

        public override T GetStateData<T>()
        {
            T data;

            Type listType = typeof(T);

            if (listType == typeof(BEPS02FlyingOwlsInitStateObjectDependency))
            {
                BEPS02FlyingOwlsInitStateObjectDependency initStateData = new BEPS02FlyingOwlsInitStateObjectDependency();
                initStateData.DragConfig = configSO.dragConfig;
                initStateData.OwlConfig = configSO.owlConfig;
                initStateData.PointSpawnOwl = pointSpawnOwl;
                initStateData.PointWoodensGroup = pointWoodensGroup;
                initStateData.ListOwlConfig = listOwlConfig;
                initStateData.ListOwls = listOwls;
                initStateData.GuidingHand = guidingHand;
                initStateData.ListTextOwl = listTextOwl;
                initStateData.ListAllLandingPoint = listAllLandingPoint;
                initStateData.Test = test;
                data = ConvertToType<T>(initStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsIntroStateObjectDependency))
            {
                BEPS02FlyingOwlsIntroStateObjectDependency introStateData = new BEPS02FlyingOwlsIntroStateObjectDependency();
                introStateData.IntroConfig = configSO.introConfig;
                introStateData.OwlConfig = configSO.owlConfig;
                introStateData.OwlsBox = owlsBox;
                introStateData.ShadowOwl = shadowOwl;
                introStateData.OwlLandingSpots = owlLandingSpots;
                introStateData.ListOwls = listOwls;
                introStateData.ListTextOwl = listTextOwl;
                introStateData.ListLandingPoint = listAllLandingPoint;
                introStateData.ListShadowOwls = listShadowOwls;
                introStateData.ListWoodens = listWoodens;
                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsPlayStateObjectDependency))
            {
                BEPS02FlyingOwlsPlayStateObjectDependency playStateData = new BEPS02FlyingOwlsPlayStateObjectDependency();
                playStateData.GuidingConfig = configSO.guidingConfig;
                playStateData.OwlConfig = configSO.owlConfig;
                playStateData.PointSpawnOwl = pointSpawnOwl;
                playStateData.ListShadowOwls = listShadowOwls;
                playStateData.GuidingHand = guidingHand;
                playStateData.ListOwls = listOwls;
                playStateData.ListTextOwl = listTextOwl;
                data = ConvertToType<T>(playStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsPlayEffectStateObjectDependency))
            {
                BEPS02FlyingOwlsPlayEffectStateObjectDependency playEffectStateData = new BEPS02FlyingOwlsPlayEffectStateObjectDependency();
                playEffectStateData.OwlConfig = configSO.owlConfig;
                playEffectStateData.ListOwls = listOwls;
                data = ConvertToType<T>(playEffectStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsClickStateObjectDependency))
            {
                BEPS02FlyingOwlsClickStateObjectDependency clickObjectData = new BEPS02FlyingOwlsClickStateObjectDependency();
                clickObjectData.OwlConfig = configSO.owlConfig;
                clickObjectData.GuidingHand = guidingHand;
                clickObjectData.ListShadowOwls = listShadowOwls;
                data = ConvertToType<T>(clickObjectData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsDraggingStateObjectDependency))
            {
                BEPS02FlyingOwlsDraggingStateObjectDependency dragingStateData = new BEPS02FlyingOwlsDraggingStateObjectDependency();
                dragingStateData.GuidingHand = guidingHand;
                dragingStateData.OwlConfig = configSO.owlConfig;
                dragingStateData.ListOwls = listOwls;
                dragingStateData.ListShadowOwls = listShadowOwls;
                data = ConvertToType<T>(dragingStateData);
            }
           /* else if (listType == typeof(BEPS02FlyingOwlsDragResultStateObjectDependency))
            {
                BEPS02FlyingOwlsDragResultStateObjectDependency dragStateData = new BEPS02FlyingOwlsDragResultStateObjectDependency();
                dragStateData.OwlConfig = configSO.owlConfig;
                dragStateData.DragConfig = configSO.dragConfig;
                dragStateData.ListOwls = listOwls;
                dragStateData.ListShadowOwls = listShadowOwls;
                data = ConvertToType<T>(dragStateData);
            }*/
            else if (listType == typeof(BEPS02FlyingOwlsNextTurnStateObjectDependency))
            {
                BEPS02FlyingOwlsNextTurnStateObjectDependency nextTurnStateData = new BEPS02FlyingOwlsNextTurnStateObjectDependency();
                nextTurnStateData.OwlConfig = configSO.owlConfig;
                nextTurnStateData.IntroConfig = configSO.introConfig;
                nextTurnStateData.SfxWin = configSO.dragConfig.sfxWin;
                nextTurnStateData.ListOwls = listOwls;
                nextTurnStateData.ListTextOwl = listTextOwl;
                nextTurnStateData.ListWoodens = listWoodens;
                nextTurnStateData.GuidingHand = guidingHand;
                nextTurnStateData.PointSpawnOwl = pointSpawnOwl;
                nextTurnStateData.ListShadowOwls = listShadowOwls;
                nextTurnStateData.UIParticle = uIParticle;
                nextTurnStateData.EffectEndGame = effectEndGame;
                data = ConvertToType<T>(nextTurnStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsOutroStateObjectDependency))
            {
                BEPS02FlyingOwlsOutroStateObjectDependency outroStateData = new BEPS02FlyingOwlsOutroStateObjectDependency();
                outroStateData.BackgroundFadeOut = backgroundFadeOut;
                outroStateData.GuidingHand = guidingHand;
                outroStateData.UIParticle = uIParticle;
                outroStateData.EffectEndGame = effectEndGame;
                data = ConvertToType<T>(outroStateData);
            }
            else
            {
                data = ConvertToType<T>(null);
            }

            return data;
        }
    }
}
