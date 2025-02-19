using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsDragResultState : FSMState
    {
        private BEPS02FlyingOwlsDragResultStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private BEPS02FlyingOwlsPlayStateEventData playStateEventData;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            /*BEPS02FlyingOwlsDragResultStateData dragStateData = (BEPS02FlyingOwlsDragResultStateData)data;

            DoWord(dragStateData);*/
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsDragResultStateObjectDependency)data;
        }

       /* private async void DoWord(BEPS02FlyingOwlsDragResultStateData dataDrag)
        {
            cts = new CancellationTokenSource();
            playStateEventData = new BEPS02FlyingOwlsPlayStateEventData();
            SoundChannel soundData;

            BEPS02DragItem currentOwl = dataDrag.EventDragData.OwlObject.GetComponent<BEPS02DragItem>();
            currentOwl.transform.SetAsLastSibling();
            BEPS02HandleData.EnableOwls(dependency.ListOwls, false);
            SkeletonGraphic owlAnimation = currentOwl.GetSkeleton();
            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlAnimation);
            try
            {
                if (dataDrag.EventDragData.UserInput == BEPS02FlyingOwlsUserInput.DragMatching)
                {
                    BaseImage currentShadowOwl = dataDrag.EventDragData.ShadowOwlObject.GetComponent<BaseImage>();
                    bool isDragOwlMatch = BEPS02HandleData.AreIntegersEqual(currentOwl.GetIndex(), BEPS02HandleData.GetIndexByName(currentShadowOwl.name));
                    bool tscMoveOwl = false;
                    if (isDragOwlMatch)
                    {
                        bool tscCorrectDone = false;
                        currentOwl.SetDragObject(false);
                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFly, aniIndex, true, null);
                        currentOwl.transform.DOMove(currentShadowOwl.transform.position, 0.4f).SetEase(Ease.InOutSine).OnComplete(() => { tscMoveOwl = true; });
                        await UniTask.WaitUntil(() => tscMoveOwl, cancellationToken: cts.Token);

                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                        {
                            if (currentShadowOwl.GetComponent<Image>().color.a > 0)
                            {
                                currentShadowOwl.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                            }else
                            {
                                BaseImage getShadow = dependency.ListShadowOwls.Find((item) => item.GetComponent<Image>().color.a > 0);
                                if(getShadow != null)
                                {
                                    getShadow.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                                }
                            }
                            soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.DragConfig.sfxCorrect, null, 1, false);
                            ObserverManager.TriggerEvent<SoundChannel>(soundData);

                            BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlTapCorrect, aniIndex, false, (trackEntry) =>
                            {
                                currentShadowOwl.transform.localScale = Vector3.zero;
                                BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                tscCorrectDone = true;
                            });
                        });
                        await UniTask.WaitUntil(() => tscCorrectDone, cancellationToken: cts.Token);
                        BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragCorrect, null);

                    }
                    else
                    {
                        bool tscWrongDone = false;
                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFly, aniIndex, true, null);
                        currentOwl.transform.DOMove(currentShadowOwl.transform.position, 0.4f).SetEase(Ease.InOutSine).OnComplete(() => {tscMoveOwl = true; });
                        await UniTask.WaitUntil(() => tscMoveOwl, cancellationToken: cts.Token);
                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                        {
                            if (currentShadowOwl.GetComponent<Image>().color.a > 0)
                            {
                                currentShadowOwl.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                            }
                            else
                            {
                                BaseImage getShadow = dependency.ListShadowOwls.Find((item) => item.GetComponent<Image>().color.a > 0);
                                if (getShadow != null)
                                {
                                    getShadow.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                                }
                            }
                            SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.DragConfig.sfxWrong, null, 1, false);
                            ObserverManager.TriggerEvent<SoundChannel>(soundData);

                            BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlTapWrong, aniIndex, false, (trackEntry) =>
                            {
                                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);
                                BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                                {
                                    BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFly, aniIndex, true, null);
                                    currentOwl.transform.DOMove(dataDrag.EventDragData.ResetPosition.position, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
                                    {
                                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                                        {
                                            SoundManager.Instance.StopFxOneShot();
                                            BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                            tscWrongDone = true;
                                        });
                                    });
                                });

                            });

                        });
                        await UniTask.WaitUntil(() => tscWrongDone, cancellationToken: cts.Token);
                        playStateEventData.OwlObject = currentOwl.gameObject;
                        BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragWrong, playStateEventData);
                    }
                }
                else
                {
                    bool tscReturnDone = false;
                    BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlDrop, aniIndex, false, (trackEntry) =>
                    {
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                        BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);

                        BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFly, aniIndex, true, null);
                        currentOwl.transform.DOMove(dataDrag.EventDragData.ResetPosition.position, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
                        {
                            BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                            {
                                SoundManager.Instance.StopFxOneShot();
                                BEPS02HandleData.SetNumberAnimation(owlAnimation, dependency.OwlConfig.owlNormal, aniIndex, true, null);
                                tscReturnDone = true;
                            });
                        });

                    });
                    await UniTask.WaitUntil(() => tscReturnDone, cancellationToken: cts.Token);
                    playStateEventData.OwlObject = currentOwl.gameObject;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayGame, playStateEventData);
                }
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
           
        }*/

        public override void OnExit()
        {
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }

    }
    public class BEPS02FlyingOwlsDragResultStateData
    {
        public List<AudioClip> AudioWords { get; set; }
        public BEPS02FlyingOwlsDragResultStateEventData EventDragData { get; set; }
    }

    public class BEPS02FlyingOwlsDragResultStateEventData
    {
        public GameObject OwlObject { get; set; }
        public GameObject ShadowOwlObject { get; set; }
        public Transform ResetPosition { get; set; }
        public BEPS02FlyingOwlsUserInput UserInput { get; set; }
    }


    public class BEPS02FlyingOwlsDragResultStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public BEPS02FlyingOwlsDragConfig DragConfig { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
        public List<BaseImage> ListShadowOwls { get; set; }
    }
}