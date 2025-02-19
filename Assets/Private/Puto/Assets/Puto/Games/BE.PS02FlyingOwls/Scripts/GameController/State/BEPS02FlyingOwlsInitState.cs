using Cysharp.Threading.Tasks;
using DataModel;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsInitState : FSMState
    {
        private BEPS02FlyingOwlsInitStateObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS02FlyingOwlsInitStateData initStateData = (BEPS02FlyingOwlsInitStateData)data;
            SetData(initStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsInitStateObjectDependency)data;
        }

        private async void SetData(BEPS02FlyingOwlsInitStateData dataInit)
        {
            cts = new CancellationTokenSource();
           /* try
            {
                cts = new CancellationTokenSource();
                await UniTask.WaitUntil(() => dependency.Test.isPlayGame, cancellationToken: cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }*/

            List<Transform> availableSpawnPoints = new(BEPS02HandleData.GetListChildRect(dependency.PointSpawnOwl));

            for (int i = 0; i < dataInit.SyncDatasPlay.Count; i++)
            {
                if (availableSpawnPoints.Count == 0)
                {
                    break;
                }
                int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[randomIndex];
                BEPS02DataOwl dataOwl = new BEPS02DataOwl();

                if (dataInit.SyncDatasPlay[i].w.Contains(" ") && (dataInit.SyncDatasPlay[i].w.Length - 1) > BEPS02HandleData.MAX_CHARACTERS)
                {
                    string inputReplace = dataInit.SyncDatasPlay[i].w.Replace(" ", "\n");
                    dataOwl.text = inputReplace;
                }
                else
                {
                    dataOwl.text = dataInit.SyncDatasPlay[i].w;
                }
                dataOwl.audio = dataInit.AudioWords[i];

                BEPS02OwlDrag item = dependency.ListOwlConfig[i];
                item.gameObject.SetActive(true);
                item.name = $"Owl_{i}";
                item.SetPlaying(false);
                item.transform.position = spawnPoint.position;
                item.InitData(dataOwl, dependency.OwlConfig, dependency.DragConfig, dependency.GuidingHand);
                BEPS02HandleData.SetNumberAnimation(item.GetSkeleton(), dependency.OwlConfig.owlFly, (i + 1), true, null);
                item.Enable(false);
                item.SetDragObject(false);
                item.GetComponentInChildren<TMP_Text>().enableAutoSizing = true;
                dependency.ListTextOwl.Add(item.GetComponentInChildren<TMP_Text>());
                dependency.ListOwls.Add(item);
                LayoutRebuilder.ForceRebuildLayoutImmediate(item.GetComponent<RectTransform>());
                availableSpawnPoints.RemoveAt(randomIndex);
            }

            for (int i = 0; i < dependency.ListAllLandingPoint.Count; i++)
            {
                BEPS02WoodenItem woodenItem = dependency.ListAllLandingPoint[i];
                woodenItem.name = $"Wooden_{i}";
                woodenItem.SetColor(dependency.OwlConfig.colorNormal);
            }

            if (dependency.ListTextOwl.Count > 0)
            {
                SetTextSizesEqualMin(dependency.ListTextOwl);
            }

            TriggerFinishSetData();
        }

        private void SetTextSizesEqualMin(List<TMP_Text> textList)
        {
            float minSize = float.MaxValue;

            foreach (TMP_Text text in textList)
            {
                text.ForceMeshUpdate();
                minSize = Mathf.Min(minSize, text.fontSize);
            }

            foreach (TMP_Text text in textList)
            {
                text.enableAutoSizing = false;
                text.fontSize = minSize;
            }
        }

        private void TriggerFinishSetData()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(dependency.PointWoodensGroup.GetComponent<RectTransform>());
            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.IntroGame, null);
        }

    }
    public class BEPS02FlyingOwlsInitStateData
    {
        public List<SyncData> SyncDatasPlay { get; set; }
        public List<AudioClip> AudioWords { get; set; }
        public bool IsDifficult { get; set; }
        public bool IsNextTurn { get; set; }
    }

    public class BEPS02FlyingOwlsInitStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public BEPS02FlyingOwlsDragConfig DragConfig { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public List<BEPS02OwlDrag> ListOwlConfig { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
        public List<TMP_Text> ListTextOwl { get; set; }
        public List<BEPS02WoodenItem> ListAllLandingPoint { get; set; }
        public GameObject PointSpawnOwl { get; set; }
        public HorizontalLayoutGroup PointWoodensGroup { get; set; }

        public BEPS02TestSelectLever Test { get; set; }
    }
}