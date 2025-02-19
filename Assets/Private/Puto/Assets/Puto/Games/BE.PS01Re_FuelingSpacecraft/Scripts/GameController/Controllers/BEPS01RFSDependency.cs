using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSDependency : Dependency
    {
        [Header("CONFIGURATION")]
        [Space(10)]
        [SerializeField] private BEPS01RFSConfigSO configSO;
        [SerializeField] private GameObject tubeItemLong;
        [SerializeField] private GameObject tubeItemShort;
        [SerializeField] private GameObject dashBoxItemLong;
        [SerializeField] private GameObject dashBoxItemShort;
        [SerializeField] private GameObject textTubeObject;
        [Header("POINT MOVE")]
        [Space(10)]
        [SerializeField] private Transform pointMoveTube;
        [SerializeField] private Transform pointResetTube;
        [SerializeField] private Transform pointUpSpaceStation;
        [SerializeField] private Transform pointCenterSpaceStation;
        [SerializeField] private Transform pointDownSpaceStation;
        [Header("TRANSFORM PLACE")]
        [Space(10)]
        [SerializeField] private Transform transformPlaceTube;
        [SerializeField] private Transform transformPlaceText;
        [SerializeField] private Transform transformPlaceCatMove;
        [SerializeField] private Transform transformPlaceCatStart;
        [SerializeField] private Transform transformPlaceCatAppear;
        [Header("CONTENT")]
        [Space(10)]
        [SerializeField] private BEPS01RFSLoopBackground loopBackground;
        [SerializeField] private Transform spaceStation;
        [SerializeField] private BaseButton buttonSkipCTA;
        [SerializeField] private BaseButton buttonSkipGuiding;
        [SerializeField] private BaseButton buttonCatStation;
        [SerializeField] private BaseButton buttonCatSpaceship;
        [SerializeField] private BEPS01RFSGuiding guiding;
        [SerializeField] private List<Transform> pointsRandom;
        [Header("CONTENT UI ITEM")]
        [Space(10)]
        [SerializeField] private BEPS01RFSSpaceship spaceship;
        [SerializeField] private SkeletonGraphic catSkeleton;
        [SerializeField] private HorizontalLayoutGroup layoutTube;
        [SerializeField] private HorizontalLayoutGroup layoutDashBox;
        [SerializeField] private HorizontalLayoutGroup layoutTextTube;
        [SerializeField] private List<BEPS01RFSTubeItem> tubeItems;
        [SerializeField] private List<BEPS01RFSTubeItem> tubeEmptyItems;
        [SerializeField] private List<BEPS01RFSDashBoxItem> dashBoxItems;
        [SerializeField] private List<Transform> textsTubePoint;
        //test
        [Header("TEST")]
        [SerializeField] private BEPS01RFSTestPlayData testPlayData;
         
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
            if (listType == typeof(BEPS01RFSInitStateObjectDependency))
            {
                BEPS01RFSInitStateObjectDependency initData = new BEPS01RFSInitStateObjectDependency();
                initData.TubeConfig = configSO.tubeConfig;
                initData.GuidingConfig = configSO.guidingConfig;
                initData.DragResultConfig = configSO.dragResultConfig;
                initData.SkeletonConfig = configSO.skeletonConfig;
                initData.DashBoxItemLong = dashBoxItemLong;
                initData.DashBoxItemShort = dashBoxItemShort;
                initData.TubeItemShort = tubeItemShort;
                initData.TubeItemLong = tubeItemLong;
                initData.TextTubeObject = textTubeObject;
                initData.Spaceship = spaceship;
                initData.CatSkeleton = catSkeleton;
                initData.LayoutTube = layoutTube;
                initData.LayoutDashBox = layoutDashBox;
                initData.TubeItems = tubeItems;
                initData.TubeEmptyItems = tubeEmptyItems;
                initData.DashBoxItems = dashBoxItems;
                initData.DashBoxItems = dashBoxItems;
                initData.LayoutTextTube = layoutTextTube;
                initData.TextsTubePoint = textsTubePoint;
                initData.ButtonSkipCTA = buttonSkipCTA;
                initData.ButtonSkipGuiding = buttonSkipGuiding;
                initData.ButtonCatStation = buttonCatStation;
                initData.ButtonCatSpaceship = buttonCatSpaceship;
                initData.TransformPlaceCatAppear = transformPlaceCatAppear;
                initData.Guiding = guiding;

                initData.TestPlayData = testPlayData;

                data = ConvertToType<T>(initData);
            }
            else if (listType == typeof(BEPS01RFSIntroStateObjectDependency))
            {
                BEPS01RFSIntroStateObjectDependency introData = new BEPS01RFSIntroStateObjectDependency();
                introData.IntroConfig = configSO.introConfig;
                introData.SkeletonConfig = configSO.skeletonConfig;
                introData.PointMoveTube = pointMoveTube;
                introData.TransformPlaceTube = transformPlaceTube;
                introData.TransformPlaceText = transformPlaceText;
                introData.LayoutDashBox = layoutDashBox;
                introData.LayoutTube = layoutTube;
                introData.LayoutTextTube = layoutTextTube;
                introData.Spaceship = spaceship;
                introData.TubeItems = tubeItems;
                introData.TextsTubePoint = textsTubePoint;
                introData.TubeEmptyItems = tubeEmptyItems;
                introData.DashBoxItems = dashBoxItems;
                introData.ButtonSkipCTA = buttonSkipCTA;
                introData.PointsRandom = pointsRandom;
                data = ConvertToType<T>(introData);
            }
            else if (listType == typeof(BEPS01RFSPlayStateObjectDependency))
            {
                BEPS01RFSPlayStateObjectDependency playData = new BEPS01RFSPlayStateObjectDependency();
                playData.Guiding = guiding;
                playData.TubeItems = tubeItems;
                playData.DashBoxItems = dashBoxItems;
                playData.ButtonCatStation = buttonCatStation;
                playData.ButtonCatSpaceship = buttonCatSpaceship;
                data = ConvertToType<T>(playData);
            }
            else if (listType == typeof(BEPS01RFSClickStateObjectDependency))
            {
                BEPS01RFSClickStateObjectDependency clickData = new BEPS01RFSClickStateObjectDependency();
                clickData.ClickConfig = configSO.clickConfig;
                clickData.Guiding = guiding;
                clickData.TubeItems = tubeItems;
                clickData.ButtonCatStation = buttonCatStation;
                clickData.ButtonCatSpaceship = buttonCatSpaceship;
                data = ConvertToType<T>(clickData);
            }
            else if (listType == typeof(BEPS01RFSDraggingStateObjectDependency))
            {
                BEPS01RFSDraggingStateObjectDependency draggingData = new BEPS01RFSDraggingStateObjectDependency();
                draggingData.Guiding = guiding;
                draggingData.TubeItems = tubeItems;
                draggingData.ButtonCatStation = buttonCatStation;
                draggingData.ButtonCatSpaceship = buttonCatSpaceship;
                data = ConvertToType<T>(draggingData);
            }
          /*  else if (listType == typeof(BEPS01RFSDragResultStateObjectDependency))
            {
                BEPS01RFSDragResultStateObjectDependency dragResultData = new BEPS01RFSDragResultStateObjectDependency();
                dragResultData.TubeConfig = configSO.tubeConfig;
                dragResultData.SkeletonConfig = configSO.skeletonConfig;
                dragResultData.DragResultConfig = configSO.dragResultConfig;
                dragResultData.Guiding = guiding;
                dragResultData.TubeItems = tubeItems;
                dragResultData.ButtonCatStation = buttonCatStation;
                dragResultData.ButtonCatSpaceship = buttonCatSpaceship;
                data = ConvertToType<T>(dragResultData);
            }*/
            else if (listType == typeof(BEPS01RFSNextTurnStateObjectDependency))
            {
                BEPS01RFSNextTurnStateObjectDependency nextTurnData = new BEPS01RFSNextTurnStateObjectDependency();
                nextTurnData.Guiding = guiding;
                nextTurnData.SkeletonConfig = configSO.skeletonConfig;
                nextTurnData.NextTurnConfig = configSO.nextTurnConfig;
                nextTurnData.TextColorFade = configSO.tubeConfig.textFade;
                nextTurnData.CatSkeleton = catSkeleton;
                nextTurnData.Spaceship = spaceship;
                nextTurnData.TubeItems = tubeItems;
                nextTurnData.DashBoxItems = dashBoxItems;
                nextTurnData.TubeEmptyItems = tubeEmptyItems;
                nextTurnData.TextsTubePoint = textsTubePoint;
                nextTurnData.TransformPlaceCatStart = transformPlaceCatStart;
                nextTurnData.TransformPlaceCatMove = transformPlaceCatMove;
                nextTurnData.SpaceStation = spaceStation;
                nextTurnData.LayoutTube = layoutTube;
                nextTurnData.LayoutDashBox = layoutDashBox;
                nextTurnData.LayoutTextTube = layoutTextTube;
                nextTurnData.PointResetTube = pointResetTube;
                nextTurnData.PointUpSpaceStation = pointUpSpaceStation;
                nextTurnData.PointCenterSpaceStation = pointCenterSpaceStation;
                nextTurnData.PointDownSpaceStation = pointDownSpaceStation;
                nextTurnData.TransformPlaceCatAppear = transformPlaceCatAppear;
                nextTurnData.ButtonCatStation = buttonCatStation;
                nextTurnData.ButtonCatSpaceship = buttonCatSpaceship;
                nextTurnData.LoopBackground = loopBackground;

                data = ConvertToType<T>(nextTurnData);
            }
            else if (listType == typeof(BEPS01RFSEndGameStateObjectDependency))
            {
                BEPS01RFSEndGameStateObjectDependency endGameData = new BEPS01RFSEndGameStateObjectDependency();
                endGameData.SkeletonConfig = configSO.skeletonConfig;
                endGameData.LayoutTube = layoutTube;
                endGameData.Spaceship = spaceship;
                endGameData.SpaceStation = spaceStation;
                endGameData.LoopBackground = loopBackground;
                endGameData.PointDownSpaceStation = pointDownSpaceStation;
                endGameData.PointResetTube = pointResetTube;

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