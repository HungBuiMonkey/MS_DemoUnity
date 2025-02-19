using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsPlayState : FSMState
    {
        private BEPS02FlyingOwlsPlayStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private bool tscOwlPlaying = false;
        private BEPS02FlyingOwlsPlayStateData dataPlay;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            dataPlay = (BEPS02FlyingOwlsPlayStateData)data;
            cts = new CancellationTokenSource();
            tscOwlPlaying = dependency.ListOwls.Any(owl => owl.IsPlaying());
            DoWord();
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsPlayStateObjectDependency)data;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            tscOwlPlaying = dependency.ListOwls.Any(owl => owl.IsPlaying());
            if (dataPlay != null && BEPS02HandleData.DragCorrectCount == dataPlay.MaxOwlDrag)
            {
                if (dataPlay.TurnGame < dataPlay.MaxTurn)
                {
                    CheckNextTurn();
                }
            }
        }

        private async void CheckNextTurn()
        {
            try
            {
                await UniTask.WaitUntil(() => !tscOwlPlaying, cancellationToken: cts.Token);
                BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.NextTurnGame, null);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
            return;
        }


        private async void DoWord()
        {
            try
            {
                if (BEPS02HandleData.DragCorrectCount == dataPlay.MaxOwlDrag)
                {
                    if (dataPlay.TurnGame < dataPlay.MaxTurn)
                    {
                        await UniTask.WaitUntil(() => !tscOwlPlaying, cancellationToken: cts.Token);
                        BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.NextTurnGame, null);
                    }
                }else
                {
                    List<int> indexesOwl = BEPS02HandleData.GetIndexesByList(dependency.ListOwls);
                    List<BEPS02OwlDrag> listOwls = BEPS02HandleData.SortListByIndex(dependency.ListOwls, indexesOwl);

                    List<BEPS02OwlDrag> listOwlsSelected = new();
                    if (listOwlsSelected.Count > 0) listOwlsSelected.Clear();
                    foreach (var owl in listOwls)
                    {
                        if(!owl.IsPlaying()) owl.Enable(true);
                        BaseImage currentShadowOwl = BEPS02HandleData.GetShadowOwlByOwl(dependency.ListShadowOwls, owl.name);
                        currentShadowOwl.GetComponent<BEPS02ShadowOwl>().IsDraged = false;
                        if (owl.IsDragObject())
                        {
                            listOwlsSelected.Add(owl);
                        }
                    }
                  
                    await UniTask.WaitUntil(() => !tscOwlPlaying, cancellationToken: cts.Token);

                    if (BEPS02HandleData.DragWrongCount == 3)
                    {
                        dependency.GuidingHand.ResetGuiding();
                        if (dataPlay.EventDataPlay != null)
                        {
                            BEPS02OwlDrag owlObject = dataPlay.EventDataPlay.OwlObject.GetComponent<BEPS02OwlDrag>();
                            BaseImage currentShadowOwl = BEPS02HandleData.GetShadowOwlByOwl(dependency.ListShadowOwls, owlObject.name);
                            if (currentShadowOwl != null)
                            {
                                currentShadowOwl.GetComponent<Image>().DOFade(1, 0.25f).SetEase(Ease.Linear);
                                await UniTask.Delay(250, cancellationToken: cts.Token);
                                Vector3 posOwl = new(owlObject.transform.position.x + 0.2f, owlObject.transform.position.y + 1f, owlObject.transform.position.z);
                                Vector3 posShadowOwl = new(currentShadowOwl.transform.position.x + 0.2f, currentShadowOwl.transform.position.y + 1f, currentShadowOwl.transform.position.z);
                                dependency.GuidingHand.InitData(posOwl, posShadowOwl, dependency.GuidingConfig);
                                dependency.GuidingHand.StartGuiding(false);
                            }
                        }
                    }
                    else if (listOwlsSelected.Count > 0)
                    {
                        int radomOwlIndex = UnityEngine.Random.Range(0, listOwlsSelected.Count);
                        BEPS02OwlDrag owlGuiding = listOwlsSelected[radomOwlIndex];
                        BaseImage shadowOwlGuiding = BEPS02HandleData.GetShadowOwlByOwl(dependency.ListShadowOwls, owlGuiding.name);
                        Vector3 posOwl = new(owlGuiding.transform.position.x + 0.2f, owlGuiding.transform.position.y + 1f, owlGuiding.transform.position.z);
                        Vector3 posShadowOwl = new(shadowOwlGuiding.transform.position.x + 0.2f, shadowOwlGuiding.transform.position.y + 1f, shadowOwlGuiding.transform.position.z);

                        dependency.GuidingHand.InitData(posOwl, posShadowOwl, dependency.GuidingConfig);
                        dependency.GuidingHand.StartGuiding(true);
                    }
               
                }
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
    public class BEPS02FlyingOwlsPlayStateData
    {
        public BEPS02FlyingOwlsPlayStateEventData EventDataPlay { get; set; }
        public int MaxOwlDrag { get; set; }
        public int TurnGame { get; set; }
        public int MaxTurn { get; set; }
    }
    public class BEPS02FlyingOwlsPlayStateEventData
    {
        public GameObject OwlObject { get; set; }
    }


    public class BEPS02FlyingOwlsPlayStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public BEPS02FlyingOwlsGuidingConfig GuidingConfig { get; set; }
        public List<TMP_Text> ListTextOwl { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
        public GameObject PointSpawnOwl { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
    }
}