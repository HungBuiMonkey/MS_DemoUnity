using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;


namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsNextTurnState : FSMState
    {
        private BEPS02FlyingOwlsNextTurnStateObjectDependency dependency;
        private CancellationTokenSource cts;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS02FlyingOwlsNextTurnStateData dataNextTurn = (BEPS02FlyingOwlsNextTurnStateData)data;
            DoWord(dataNextTurn);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsNextTurnStateObjectDependency)data;
        }

        private async void DoWord(BEPS02FlyingOwlsNextTurnStateData dataNextTurn)
        {
            cts = new CancellationTokenSource();
            dependency.GuidingHand.ResetGuiding();
            BEPS02HandleData.EnableOwls(dependency.ListOwls, false);
            BEPS02HandleData.StopCoroutinesOwls(dependency.ListOwls);
            try
            {
                SoundChannel soundData;
                bool tscSfxWin = false;
              
                List<Transform> availableSpawnPoints = new(BEPS02HandleData.GetListChildRect(dependency.PointSpawnOwl));
                List<int> indexesOwl = BEPS02HandleData.GetIndexesByList(dependency.ListOwls);

                List<TMP_Text> listTextItem = BEPS02HandleData.SortListByIndex(dependency.ListTextOwl, indexesOwl);
                List<BEPS02OwlDrag> listOwls = BEPS02HandleData.SortListByIndex(dependency.ListOwls, indexesOwl);

            
                await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                //sync text 
                bool tscAudioSyncText = false;
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dataNextTurn.DataTurn.audioSentence, () => { tscAudioSyncText = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);

                if (dependency.ListTextOwl.Count > 0 && dataNextTurn.DataTurn.syncDatas != null)
                {
                    for (int i = 0; i < dependency.ListTextOwl.Count; i++)
                    {
                        string firstContent = dependency.ListTextOwl[i].text;
                        string textResult = dependency.ListTextOwl[i].text;
                        textResult = BEPS02HandleData.COLOR_YELLOW + dependency.ListTextOwl[i].text + "</color>";
                        dependency.ListTextOwl[i].text = textResult;
                        dependency.ListWoodens[i].SetColor(dependency.OwlConfig.colorSync);
                        await UniTask.Delay(Math.Abs(dataNextTurn.DataTurn.syncDatas[i].e - dataNextTurn.DataTurn.syncDatas[i].s), cancellationToken: cts.Token);
                        dependency.ListTextOwl[i].text = firstContent;
                        dependency.ListWoodens[i].SetColor(dependency.OwlConfig.colorNormal);
                    }
                }

                await UniTask.WaitUntil(() => tscAudioSyncText, cancellationToken: cts.Token);
                await UniTask.Delay(dependency.IntroConfig.miliSecondDelay * 2, cancellationToken: cts.Token);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.SfxWin, () => { tscSfxWin = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                foreach (var item in listOwls)
                {
                    SkeletonGraphic skeletonGraphic = item.GetSkeleton();
                    int indexItem = BEPS02HandleData.GetIndexCurrentAnimation(skeletonGraphic);
                    BEPS02HandleData.SetNumberAnimation(skeletonGraphic, dependency.OwlConfig.owlTapCorrect, indexItem, false, (track) =>
                    {
                        BEPS02HandleData.SetNumberAnimation(skeletonGraphic, dependency.OwlConfig.owlNormal, indexItem, false, null);
                    });
                    await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);
                }
                if (dataNextTurn.TurnGame >= (dataNextTurn.MaxTurn - 1))
                {
                    dependency.UIParticle.gameObject.SetActive(true);
                    dependency.EffectEndGame.Play();
                }
                await UniTask.WaitUntil(() => tscSfxWin, cancellationToken: cts.Token);
                await UniTask.Delay(dependency.IntroConfig.miliSecondDelay, cancellationToken: cts.Token);

                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);

                bool tscMoveDone = false;
                for (int i = 0; i < listOwls.Count; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                    Transform spawnPoint = availableSpawnPoints[randomIndex];
                    availableSpawnPoints.RemoveAt(randomIndex);
                    int owlIndex = i;
                    SkeletonGraphic animationOwl = listOwls[i].GetSkeleton();
                    int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(animationOwl);
                    listOwls[i].transform.SetAsLastSibling();
                    BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                    {
                        BEPS02HandleData.SetNumberAnimation(animationOwl, dependency.OwlConfig.owlFly, aniIndex, true, null);
                        listOwls[owlIndex].transform.DOMove(spawnPoint.position, 2f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            SoundManager.Instance.StopFxOneShot();
                            tscMoveDone = true;
                            listOwls[owlIndex].gameObject.SetActive(false);
                            GameObject.Destroy(dependency.ListShadowOwls[owlIndex].gameObject);
                        });
                    });
                }

               
                await UniTask.WaitUntil(() => tscMoveDone, cancellationToken: cts.Token);
                BEPS02HandleData.DragCorrectCount = 0;
                BEPS02HandleData.DragWrongCount = 0;
                if (dataNextTurn.TurnGame >= (dataNextTurn.MaxTurn - 1))
                {
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.OutroGame, null);
                }
                await UniTask.Delay(dependency.IntroConfig.miliSecondDelay * 2, cancellationToken: cts.Token);

                if (dataNextTurn.TurnGame < (dataNextTurn.MaxTurn - 1))
                {
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.InitData, null);
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
            if (dependency.ListOwls.Count > 0) dependency.ListOwls.Clear();
            if (dependency.ListTextOwl.Count > 0) dependency.ListTextOwl.Clear();
            if (dependency.ListWoodens.Count > 0) dependency.ListWoodens.Clear();
            if (dependency.ListShadowOwls.Count > 0) dependency.ListShadowOwls.Clear();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }

    }

    public class BEPS02FlyingOwlsNextTurnStateData{
        public int TurnGame { get; set; }
        public int MaxTurn { get; set; }
        public BEPS02PlayConversationData DataTurn { get; set; }
    }

    public class BEPS02FlyingOwlsNextTurnStateObjectDependency
    {
        public BEPS02FlyingOwlsIntroConfig IntroConfig { get; set; }
        public AudioClip SfxWin { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public BEPS02OwlConfig OwlConfig { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
        public List<TMP_Text> ListTextOwl { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
        public GameObject PointSpawnOwl { get; set; }
        public List<BEPS02WoodenItem> ListWoodens { get; set; }
        public UIParticle UIParticle { get; set; }
        public ParticleSystem EffectEndGame { get; set; }
    }
}
