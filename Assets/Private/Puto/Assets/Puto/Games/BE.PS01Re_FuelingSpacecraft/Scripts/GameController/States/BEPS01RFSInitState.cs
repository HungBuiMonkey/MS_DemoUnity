using Cysharp.Threading.Tasks;
using DataModel;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSInitState : FSMState
    {
        private BEPS01RFSInitStateObjectDependency dependency;
        private CancellationTokenSource cts;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSInitStateData initData = (BEPS01RFSInitStateData)data;
            DoWork(initData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSInitStateObjectDependency)data;
        }

        private void DoWork(BEPS01RFSInitStateData initData)
        {

          /*  try
            {
                cts = new CancellationTokenSource();
                await UniTask.WaitUntil(() => dependency.TestPlayData.isPlayGame, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }*/
            dependency.ButtonSkipCTA.Enable(false);
            dependency.ButtonCatSpaceship.Enable(false);
            dependency.ButtonCatStation.Enable(false);
            dependency.LayoutDashBox.GetComponent<CanvasGroup>().alpha = 0f;
            dependency.LayoutTube.GetComponent<CanvasGroup>().alpha = 1f;
            dependency.LayoutTextTube.GetComponent<CanvasGroup>().alpha = 0f;
            dependency.ButtonSkipGuiding.gameObject.SetActive(false);
            dependency.Guiding.InitData(dependency.GuidingConfig, dependency.ButtonSkipGuiding);
            List<TMP_Text> listTextTube = new List<TMP_Text>();
            if (initData.CurrentTurn == 0)
            {
                dependency.Spaceship.Fade(0f);
                dependency.CatSkeleton.transform.position = dependency.TransformPlaceCatAppear.position;
                BEPS01RFSHandleData.SetAnimation(dependency.CatSkeleton, dependency.SkeletonConfig.catYellowIdle, true, null);
                dependency.Spaceship.InitState(dependency.SkeletonConfig.spaceshipFirstTurnIdle, dependency.SkeletonConfig.spaceshipFirstTurnIdle_Blink);
            }

            float totalWidthTube = 0;
            for (int i = 0; i < initData.DataTurn.syncDatas.Count; i++)
            {
                SyncData syncData = initData.DataTurn.syncDatas[i];
                BEPS01RFSTubeData tubeData = new BEPS01RFSTubeData();
                tubeData.id = i;
                tubeData.audio = initData.DataTurn.audioWords[i];
                if(syncData.w.Length <= 5)
                {
                    tubeData.text = syncData.w;
                    BEPS01RFSTubeItem tubeItem = BEPS01RFSHandleData.SpawnItem(dependency.TubeItemShort, dependency.LayoutTube.gameObject).GetComponent<BEPS01RFSTubeItem>();
                    tubeItem.InitData(tubeData, dependency.TubeConfig, dependency.DragResultConfig, dependency.SkeletonConfig, dependency.Guiding);
                    tubeItem.Enable(false);
                    tubeItem.SetColor(dependency.TubeConfig.colorNormal);
                    tubeItem.SetColorText(dependency.TubeConfig.textNormal);
                    totalWidthTube += tubeItem.GetComponent<RectTransform>().rect.width;
                    dependency.TubeItems.Add(tubeItem);
                    listTextTube.Add(tubeItem.GetText());
                    //intit dash box
                    BEPS01RFSDashBoxItem dashBoxItem = BEPS01RFSHandleData.SpawnItem(dependency.DashBoxItemShort, dependency.LayoutDashBox.gameObject).GetComponent<BEPS01RFSDashBoxItem>();
                    dashBoxItem.Fade(1f);
                    dashBoxItem.InitData(i);
                    dependency.DashBoxItems.Add(dashBoxItem);
                }
                else
                {
                    if (syncData.w.Contains(" "))
                    {
                        string inputReplace = syncData.w.Replace(" ", "\n");
                        tubeData.text = inputReplace;
                    }
                    else
                    {
                        tubeData.text = syncData.w;
                    }
                    BEPS01RFSTubeItem tubeItem = BEPS01RFSHandleData.SpawnItem(dependency.TubeItemLong, dependency.LayoutTube.gameObject).GetComponent<BEPS01RFSTubeItem>();
                    tubeItem.InitData(tubeData, dependency.TubeConfig, 
                        dependency.DragResultConfig, dependency.SkeletonConfig, dependency.Guiding);
                    tubeItem.Enable(false);
                    tubeItem.SetColor(dependency.TubeConfig.colorNormal);
                    tubeItem.SetColorText(dependency.TubeConfig.textNormal);
                    totalWidthTube += tubeItem.GetComponent<RectTransform>().rect.width;
                    dependency.TubeItems.Add(tubeItem);
                    listTextTube.Add(tubeItem.GetText());
                    //intit dash box
                    BEPS01RFSDashBoxItem dashBoxItem = BEPS01RFSHandleData.SpawnItem(dependency.DashBoxItemLong, dependency.LayoutDashBox.gameObject).GetComponent<BEPS01RFSDashBoxItem>();
                    dashBoxItem.InitData(i);
                    dashBoxItem.Fade(1f);
                    dependency.DashBoxItems.Add(dashBoxItem);
                }
                //Init text tube
                TMP_Text textTubeItem = BEPS01RFSHandleData.SpawnItem(dependency.TextTubeObject, dependency.LayoutTextTube.gameObject).GetComponent<TMP_Text>();
                textTubeItem.SetText(syncData.w);
                textTubeItem.ForceMeshUpdate();
                dependency.TextsTubePoint.Add(textTubeItem.transform);
            }
            float remainingWidth = BEPS01RFSHandleData.MAX_SIZE_LAYOUT - totalWidthTube;
            if (remainingWidth > 0)
            {
                AddItemsSymmetrically(remainingWidth);
            }
            if (listTextTube.Count > 0)
            {
                BEPS01RFSHandleData.SetTextSizesEqualMin(listTextTube);
            }

            BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.IntroGame, false);
        }
        private void AddItemsSymmetrically(float remainingWidth)
        {
            while (remainingWidth >= 2 * BEPS01RFSHandleData.SIZE_SHORT)
            {
                if (remainingWidth >= 2 * BEPS01RFSHandleData.SIZE_LONG)
                {
                    AddItemAtBothEnds(dependency.TubeItemLong, dependency.DashBoxItemLong);
                    remainingWidth -= 2 * BEPS01RFSHandleData.SIZE_LONG;
                }
                else if (remainingWidth >= 2 * BEPS01RFSHandleData.SIZE_SHORT)
                {
                    AddItemAtBothEnds(dependency.TubeItemShort, dependency.DashBoxItemShort);
                    remainingWidth -= 2 * BEPS01RFSHandleData.SIZE_SHORT;
                }
                else
                {
                    break; 
                }
            }
            if (remainingWidth > 0)
            {
                GameObject tubeToAdd = remainingWidth >= BEPS01RFSHandleData.SIZE_LONG ? dependency.TubeItemLong : dependency.TubeItemShort;
                GameObject dashBoxToAdd = remainingWidth >= BEPS01RFSHandleData.SIZE_LONG ? dependency.DashBoxItemLong : dependency.DashBoxItemShort;
                AddSingleItem(tubeToAdd, dashBoxToAdd);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.LayoutTube.GetComponent<RectTransform>());
        }
        private void AddSingleItem(GameObject itemPrefab, GameObject dashBoxPrefab)
        {
            BEPS01RFSTubeItem singleItem = BEPS01RFSHandleData.SpawnItem(itemPrefab, dependency.LayoutTube.gameObject).GetComponent<BEPS01RFSTubeItem>();
            BEPS01RFSTubeData tubeEmptyData = new BEPS01RFSTubeData();
            tubeEmptyData.id = -1;
            tubeEmptyData.audio = null;
            tubeEmptyData.text = "";
            singleItem.InitData(tubeEmptyData, dependency.TubeConfig, dependency.DragResultConfig, dependency.SkeletonConfig, dependency.Guiding);
            singleItem.Enable(false);
            singleItem.SetColor(dependency.TubeConfig.colorNormal);
            singleItem.transform.SetSiblingIndex(0);
            dependency.TubeEmptyItems.Add(singleItem);

            BEPS01RFSDashBoxItem dashBoxSingleItem = BEPS01RFSHandleData.SpawnItem(dashBoxPrefab, dependency.LayoutDashBox.gameObject).GetComponent<BEPS01RFSDashBoxItem>();
            dashBoxSingleItem.InitData(-1);
            dashBoxSingleItem.transform.SetSiblingIndex(0);
            dashBoxSingleItem.Fade(0f);
        }

        private void AddItemAtBothEnds(GameObject tubePrefab, GameObject dashBoxPrefab)
        {
            BEPS01RFSTubeData tubeEmptyData = new BEPS01RFSTubeData();
            tubeEmptyData.id = -1;
            tubeEmptyData.audio = null;
            tubeEmptyData.text = "";

            BEPS01RFSTubeItem leftItem = BEPS01RFSHandleData.SpawnItem(tubePrefab, dependency.LayoutTube.gameObject).GetComponent<BEPS01RFSTubeItem>();
            leftItem.InitData(tubeEmptyData, dependency.TubeConfig, dependency.DragResultConfig, dependency.SkeletonConfig, dependency.Guiding);
            leftItem.Enable(false);
            leftItem.SetColor(dependency.TubeConfig.colorNormal);

            BEPS01RFSDashBoxItem dashBoxLeftItem = BEPS01RFSHandleData.SpawnItem(dashBoxPrefab, dependency.LayoutDashBox.gameObject).GetComponent<BEPS01RFSDashBoxItem>();
            dashBoxLeftItem.InitData(-1);
            dashBoxLeftItem.Fade(0f);


            BEPS01RFSTubeItem rightItem = BEPS01RFSHandleData.SpawnItem(tubePrefab, dependency.LayoutTube.gameObject).GetComponent<BEPS01RFSTubeItem>();
            rightItem.InitData(tubeEmptyData, dependency.TubeConfig, dependency.DragResultConfig, dependency.SkeletonConfig, dependency.Guiding);
            rightItem.Enable(false);
            rightItem.SetColor(dependency.TubeConfig.colorNormal);

            BEPS01RFSDashBoxItem dashBoxRightItem = BEPS01RFSHandleData.SpawnItem(dashBoxPrefab, dependency.LayoutDashBox.gameObject).GetComponent<BEPS01RFSDashBoxItem>();
            dashBoxRightItem.InitData(-1);
            dashBoxRightItem.Fade(0f);

            dependency.TubeEmptyItems.Add(leftItem);
            dependency.TubeEmptyItems.Add(rightItem);

            leftItem.transform.SetSiblingIndex(0);
            dashBoxLeftItem.transform.SetSiblingIndex(0); 
            rightItem.transform.SetSiblingIndex(dependency.LayoutTube.transform.childCount - 1);
            dashBoxRightItem.transform.SetSiblingIndex(dependency.LayoutTube.transform.childCount - 1);
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

    public class BEPS01RFSInitStateData
    {
        public BEPS01RFSConversationData DataTurn { get; set; }
        public int CurrentTurn { get; set; }
    }

    public class BEPS01RFSInitStateObjectDependency
    {
        public BEPS01RFSTubeConfig TubeConfig { get; set; }
        public BEPS01RFSGuidingConfig GuidingConfig { get; set; }
        public BEPS01RFSDragResultConfig DragResultConfig { get; set; }
        public BEPS01RFSSkeletonConfig SkeletonConfig { get; set; }
        public GameObject TubeItemLong { get; set; }
        public GameObject TubeItemShort { get; set; }
        public GameObject DashBoxItemLong { get; set; }
        public GameObject DashBoxItemShort { get; set; }
        public GameObject TextTubeObject { get; set; }
        public BEPS01RFSGuiding Guiding { get; set; }
        public BEPS01RFSSpaceship Spaceship { get; set; }
        public SkeletonGraphic CatSkeleton { get; set; }
        public Transform TransformPlaceCatAppear { get; set; }
        public HorizontalLayoutGroup LayoutTube { get; set; }
        public HorizontalLayoutGroup LayoutDashBox { get; set; }
        public HorizontalLayoutGroup LayoutTextTube { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public List<BEPS01RFSTubeItem> TubeEmptyItems { get; set; }
        public List<BEPS01RFSDashBoxItem> DashBoxItems { get; set; }
        public List<Transform> TextsTubePoint { get; set; }
        public BaseButton ButtonSkipCTA { get; set; }
        public BaseButton ButtonSkipGuiding { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
        public BEPS01RFSTestPlayData TestPlayData { get; set; }

    }
}