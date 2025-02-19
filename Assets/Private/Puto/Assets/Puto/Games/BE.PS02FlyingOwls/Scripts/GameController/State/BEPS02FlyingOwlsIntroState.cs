using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
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
    public class BEPS02FlyingOwlsIntroState : FSMState
    {

        private BEPS02FlyingOwlsIntroStateObjectDependency dependency;
        private BEPS02FlyingOwlsIntroStateData introStateData;
        private CancellationTokenSource cts;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            introStateData = (BEPS02FlyingOwlsIntroStateData)data;
            DoWork(introStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsIntroStateObjectDependency)data;
        }

        private async void DoWork(BEPS02FlyingOwlsIntroStateData introStateData)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            List<Transform> availableLandingSpots = new(BEPS02HandleData.GetListChildRect(dependency.OwlLandingSpots));
            bool tscMoveDone = false;
            try
            {
                int startIndex = (dependency.ListLandingPoint.Count - dependency.ListOwls.Count) / 2;
                if (startIndex >= 0 && startIndex + dependency.ListOwls.Count <= dependency.ListLandingPoint.Count)
                {
                    List<BEPS02WoodenItem> middleItems = dependency.ListLandingPoint.GetRange(startIndex, dependency.ListOwls.Count);
                    List<BEPS02WoodenItem> listExceptWooden  = dependency.ListLandingPoint.Except(middleItems).ToList();
                    foreach(var item in listExceptWooden)
                    {
                        item.SetColor(dependency.OwlConfig.colorDisable);
                    }   

                    if (!introStateData.IsNextTurn && !introStateData.DataPlay.isDifficult)
                    {
                        //Lươt dễ đầu tiên
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);
                        List<BEPS02OwlDrag> listRandomOwls = BEPS02HandleData.RandomSelectItems(dependency.ListOwls, introStateData.MaxOwlDrag);
                        List<BEPS02OwlDrag> listExceptOwls = dependency.ListOwls.Except(listRandomOwls).ToList();

                        for (int i = 0; i < dependency.ListOwls.Count; i++)
                        {
                            int owlIndex = i;
                            middleItems[i].SetColor(dependency.OwlConfig.colorNormal);
                            dependency.ListWoodens.Add(middleItems[i]);
                            GameObject shadowObject = SpawnShadowOwls(i, middleItems[i].transform);
                            dependency.ListShadowOwls.Add(shadowObject.GetComponent<BaseImage>());
                            SkeletonGraphic animationOwl = dependency.ListOwls[i].GetSkeleton();
                            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);

                            if (listExceptOwls.Contains(dependency.ListOwls[i]))
                            {
                                dependency.ListOwls[i].transform.position = middleItems[i].transform.position;
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                            }
                        }
                        List<RectTransform> listShadowOwlInit = new();
                        if (listShadowOwlInit.Count > 0) listShadowOwlInit.Clear();

                        for (int i = 0; i < listRandomOwls.Count; i++)
                        {
                            if (availableLandingSpots.Count == 0)
                            {
                                break;
                            }
                            SkeletonGraphic animationOwl = listRandomOwls[i].GetSkeleton();
                            listRandomOwls[i].transform.SetAsLastSibling();
                            listRandomOwls[i].SetDragObject(true);

                            int randomIndex = UnityEngine.Random.Range(0, availableLandingSpots.Count);
                            Transform spawnPoint = availableLandingSpots[randomIndex];
                            availableLandingSpots.RemoveAt(randomIndex);

                            BaseImage imgShadow = dependency.ListShadowOwls[listRandomOwls[i].GetIndex()];
                            imgShadow.transform.localScale = Vector3.one;
                            BEPS02HandleData.SetAlphaImage(imgShadow.GetComponent<Image>(), 0f);
                            listShadowOwlInit.Add(imgShadow.GetComponent<RectTransform>());
                            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);
                            int owlIndex = i;

                            BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                            {
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFly, aniIndex, true, null);
                            });

                            listRandomOwls[owlIndex].transform.DOMove(spawnPoint.position, 2f).SetEase(Ease.InOutSine).OnComplete(() => {

                                listRandomOwls[owlIndex].InitData(listShadowOwlInit, spawnPoint.GetComponent<RectTransform>());
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                                {
                                    SoundManager.Instance.StopFxOneShot();
                                    tscMoveDone = true;
                                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                });
                            });
                        }
                        await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                        await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);


                    }
                    else if (introStateData.DataPlay.isDifficult)
                    {
                        //Lượt khó
                        if (middleItems.Count == dependency.ListOwls.Count)
                        {
                            BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                            BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);

                            for (int i = 0; i < dependency.ListOwls.Count; i++)
                            {
                                int owlIndex = i;
                                dependency.ListWoodens.Add(middleItems[i]);
                                GameObject shadowObject = SpawnShadowOwls(i, middleItems[i].transform);
                                dependency.ListShadowOwls.Add(shadowObject.GetComponent<BaseImage>());
                                SkeletonGraphic animationOwl = dependency.ListOwls[i].GetSkeleton();
                                int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);

                                dependency.ListOwls[i].transform.DOMove(middleItems[i].transform.position, 2f).SetEase(Ease.InOutSine).OnComplete(() => {
                                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) => {
                                        SoundManager.Instance.StopFxOneShot();
                                        tscMoveDone = true;
                                        BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                    });
                                });
                            }
                        }
                        await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                        await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                    }else
                    {
                        //Lượt dễ thứ 2
                        List<BEPS02OwlDrag> listRandomOwls = BEPS02HandleData.RandomSelectItems(dependency.ListOwls, introStateData.MaxOwlDrag);
                        List<BEPS02OwlDrag> listExceptOwls = dependency.ListOwls.Except(listRandomOwls).ToList();

                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);

                        for (int i = 0; i < dependency.ListOwls.Count; i++)
                        {
                            int owlIndex = i;
                            dependency.ListWoodens.Add(middleItems[i]);
                            GameObject shadowObject = SpawnShadowOwls(i, middleItems[i].transform);
                            dependency.ListShadowOwls.Add(shadowObject.GetComponent<BaseImage>());
                            SkeletonGraphic animationOwl = dependency.ListOwls[i].GetSkeleton();
                            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);

                            if (listExceptOwls.Contains(dependency.ListOwls[i]))
                            {
                                dependency.ListOwls[i].transform.DOMove(middleItems[i].transform.position, 2f).SetEase(Ease.InOutSine).OnComplete(() => {
                                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) => {
                                        BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                    });
                                });
                            }
                        }

                        List<RectTransform> listShadowOwlInit = new();
                        if (listShadowOwlInit.Count > 0) listShadowOwlInit.Clear();

                        for (int i = 0; i < listRandomOwls.Count; i++)
                        {
                            if (availableLandingSpots.Count == 0)
                            {
                                break;
                            }
                            SkeletonGraphic animationOwl = listRandomOwls[i].GetSkeleton();
                            listRandomOwls[i].transform.SetAsLastSibling();
                            listRandomOwls[i].SetDragObject(true);

                            int randomIndex = UnityEngine.Random.Range(0, availableLandingSpots.Count);
                            Transform spawnPoint = availableLandingSpots[randomIndex];
                            availableLandingSpots.RemoveAt(randomIndex);

                            BaseImage imgShadow = dependency.ListShadowOwls[listRandomOwls[i].GetIndex()];
                            imgShadow.transform.localScale = Vector3.one;
                            BEPS02HandleData.SetAlphaImage(imgShadow.GetComponent<Image>(), 0f);
                            listShadowOwlInit.Add(imgShadow.GetComponent<RectTransform>());
                            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);
                            int owlIndex = i;

                            BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                            {
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFly, aniIndex, true, null);
                            });

                            listRandomOwls[owlIndex].transform.DOMove(spawnPoint.position, 2f).SetEase(Ease.InOutSine).OnComplete(() => {

                                listRandomOwls[owlIndex].InitData(listShadowOwlInit, spawnPoint.GetComponent<RectTransform>());
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                                {
                                    SoundManager.Instance.StopFxOneShot();
                                    tscMoveDone = true;
                                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                });
                            });
                        }
                        await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                        await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                    }
                  
                    //sync text 
                    bool tscAudioSyncText = false;
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND, introStateData.DataPlay.audioSentence, () => { tscAudioSyncText = true; });
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);

                    if (dependency.ListTextOwl.Count > 0 && introStateData.DataPlay.syncDatas != null)
                    {
                        for (int i = 0; i < dependency.ListTextOwl.Count; i++)
                        {
                            string firstContent = dependency.ListTextOwl[i].text;
                            string textResult = dependency.ListTextOwl[i].text;
                            if (!dependency.ListOwls[i].IsDragObject())
                            {
                                textResult = BEPS02HandleData.COLOR_YELLOW + dependency.ListTextOwl[i].text + "</color>";
                                dependency.ListTextOwl[i].text = textResult;
                            }
                            dependency.ListWoodens[i].SetColor(dependency.OwlConfig.colorSync);
                            await UniTask.Delay(Math.Abs(introStateData.DataPlay.syncDatas[i].e - introStateData.DataPlay.syncDatas[i].s), cancellationToken: cts.Token);
                            dependency.ListTextOwl[i].text = firstContent;
                            dependency.ListWoodens[i].SetColor(dependency.OwlConfig.colorNormal);
                        }
                    }

                    await UniTask.WaitUntil(() => tscAudioSyncText, cancellationToken: cts.Token);
                    await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                    tscMoveDone = false;

                    if (introStateData.DataPlay.isDifficult)
                    {
                        //cu bay xuong
                        List<BEPS02OwlDrag> listRandomOwls = BEPS02HandleData.RandomSelectItems(dependency.ListOwls, introStateData.MaxOwlDrag);
                        
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);

                        List<RectTransform> listShadowOwlInit = new();
                        if (listShadowOwlInit.Count > 0) listShadowOwlInit.Clear();
                        for (int i = 0; i < listRandomOwls.Count; i++)
                        {
                            if (availableLandingSpots.Count == 0)
                            {
                                break;
                            }
                            SkeletonGraphic animationOwl = listRandomOwls[i].GetSkeleton();
                            listRandomOwls[i].transform.SetAsLastSibling();
                            listRandomOwls[i].SetDragObject(true);
                            int randomIndex = UnityEngine.Random.Range(0, availableLandingSpots.Count);
                            Transform spawnPoint = availableLandingSpots[randomIndex];
                            availableLandingSpots.RemoveAt(randomIndex);
                            BaseImage imgShadow = dependency.ListShadowOwls[listRandomOwls[i].GetIndex()];
                            imgShadow.transform.localScale = Vector3.one;
                            BEPS02HandleData.SetAlphaImage(imgShadow.GetComponent<Image>(), 0f);
                            listShadowOwlInit.Add(imgShadow.GetComponent<RectTransform>());
                            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);
                            int owlIndex = i;

                            BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                            {
                                BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFly, aniIndex, true, null);
                                listRandomOwls[owlIndex].transform.DOMove(spawnPoint.position, 2f).SetEase(Ease.InOutSine).OnComplete(() => {

                                    listRandomOwls[owlIndex].InitData(listShadowOwlInit, spawnPoint.GetComponent<RectTransform>());
                                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                                    {
                                        SoundManager.Instance.StopFxOneShot();
                                        tscMoveDone = true;
                                        BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                    });
                                });
                            });
                        }
                        await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                    }
                    await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                    BEPS02HandleData.EnableOwls(dependency.ListOwls, true);
                    if (!introStateData.IsNextTurn)
                    {
                        bool tscAudioTopic = false;
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND, GetAudioTopic(), () => { tscAudioTopic = true; });
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);

                        await UniTask.WaitUntil(() => tscAudioTopic, cancellationToken: cts.Token);
                    }

                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayGame, null);
                }
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucantai ex: "+ ex);
            }
        }



        private GameObject SpawnShadowOwls(int index, Transform transform)
        {
            GameObject item = UnityEngine.Object.Instantiate(dependency.ShadowOwl, dependency.OwlsBox.transform, false);
            item.name = item.name + "_" + index;
            item.transform.position = transform.position;
            item.transform.localScale = Vector3.zero;
            return item;
        }

        private AudioClip GetAudioTopic()
        {
            return dependency.IntroConfig.audiosTopic[UnityEngine.Random.Range(0, dependency.IntroConfig.audiosTopic.Count)];
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

    public class BEPS02FlyingOwlsIntroStateData
    {
        public BEPS02PlayConversationData DataPlay { get; set; }
        public bool IsNextTurn { get; set; }
        public int MaxOwlDrag { get; set; }
    }

    public class BEPS02FlyingOwlsIntroStateObjectDependency
    {
        public BEPS02FlyingOwlsIntroConfig IntroConfig { get; set; }
        public BEPS02OwlConfig OwlConfig { get; set; }
        public GameObject ShadowOwl { get; set; }
        public GameObject OwlsBox { get; set; }
        public GameObject OwlLandingSpots { get; set; }
        public List<TMP_Text> ListTextOwl { get; set; }
        public List<BEPS02WoodenItem> ListLandingPoint { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
        public List<BEPS02WoodenItem> ListWoodens { get; set; }
    }
}