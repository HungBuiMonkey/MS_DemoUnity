using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSDragResultState : FSMState
    {
        private BEPS01RFSDragResultStateObjectDependency dependency;
        private CancellationTokenSource cts;
  

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSDragResultStateEventData resultStateEventData  = (BEPS01RFSDragResultStateEventData)data;
            //DoWork(resultStateEventData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSDragResultStateObjectDependency)data;
        }

       /* private async void DoWork(BEPS01RFSDragResultStateEventData resultData)
        {
            cts = new CancellationTokenSource();
            SoundChannel soundData;
            dependency.ButtonCatSpaceship.Enable(false);
            dependency.ButtonCatStation.Enable(false);

            BEPS01RFSTubeItem currentTube = resultData.TubeObject.GetComponent<BEPS01RFSTubeItem>();
            currentTube.transform.SetAsLastSibling();
            BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, false);
            SkeletonGraphic tubeAnimation = currentTube.GetSkeleton();

            try
            {
           
                if (resultData.UserInput == BEPS01RFSUserInput.DragMatching)
                {
                    BEPS01RFSDashBoxItem currentDashBox = resultData.DashBoxObject.GetComponent<BEPS01RFSDashBoxItem>();
                    bool isTubeMatch = BEPS01RFSHandleData.AreIntegersEqual(currentTube.GetData().id, currentDashBox.GetId());

                    bool tscMoveTube = false;
                    currentTube.transform.DOMove(currentDashBox.transform.position, 0.35f).SetEase(Ease.InOutSine).OnComplete(() => { tscMoveTube = true; });
                    await UniTask.WaitUntil(() => tscMoveTube, cancellationToken: cts.Token);

                    if (isTubeMatch)
                    {
                      
                        currentDashBox.Fade(0f);
                        currentDashBox.SetSelected(true);
                        bool tscCorrectDone = false;
                        currentTube.SetColor(dependency.TubeConfig.colorCorrect);
                        currentTube.SetColorText(dependency.TubeConfig.textCorrect);
                        currentTube.SetDragObject(false);
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.DragResultConfig.sfxCorrect);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        BEPS01RFSHandleData.SetAnimation(tubeAnimation, dependency.SkeletonConfig.tubeCorrect, false, (trackEntry) => {
                            tscCorrectDone = true;
                            BEPS01RFSHandleData.SetAnimation(tubeAnimation, dependency.SkeletonConfig.tubeNormal, false, null);
                        });
                        await UniTask.Delay(dependency.DragResultConfig.timeDelay, cancellationToken: cts.Token);
                        bool tscFireDone = false;
                        int randomIndex = UnityEngine.Random.RandomRange(0, dependency.SkeletonConfig.fireworks.Length);
                        SkeletonGraphic tubeFirewwork = currentTube.GetFirework();
                        tubeFirewwork.gameObject.SetActive(true);
                        BEPS01RFSHandleData.SetAnimation(tubeFirewwork, dependency.SkeletonConfig.fireworks[randomIndex], false, (trackEntry) => {
                            tubeFirewwork.gameObject.SetActive(false);
                            tscFireDone = true;
                        });
                        await UniTask.WaitUntil(() => tscCorrectDone, cancellationToken: cts.Token);
                        await UniTask.Delay(dependency.DragResultConfig.timeDelay + 50, cancellationToken: cts.Token);
                        bool tscAudio = false;
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, currentTube.GetData().audio, () => { tscAudio = true; });
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        await UniTask.WaitUntil(() => tscFireDone && tscAudio, cancellationToken: cts.Token);
                        currentTube.ResetSortingOrder();
                        currentTube.SetColor(dependency.TubeConfig.colorNormal);
                        currentTube.SetColorText(dependency.TubeConfig.textNormal);
                        BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragCorrect, null);
                    }
                    else
                    {
                        bool tscWrongDone = false;
                        currentTube.SetColor(dependency.TubeConfig.colorWrong);
                        currentTube.SetColorText(dependency.TubeConfig.textWrong);
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.DragResultConfig.sfxWrong);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        BEPS01RFSHandleData.SetAnimation(tubeAnimation, dependency.SkeletonConfig.tubeWrong, false, (trackEntry) => {
                            tscWrongDone = true;
                            BEPS01RFSHandleData.SetAnimation(tubeAnimation, dependency.SkeletonConfig.tubeNormal, false, null);
                        });
                        await UniTask.WaitUntil(() => tscWrongDone, cancellationToken: cts.Token);
                        await UniTask.Delay(dependency.DragResultConfig.timeDelay / 2, cancellationToken: cts.Token);
                        currentTube.SetColor(dependency.TubeConfig.colorNormal);
                        currentTube.SetColorText(dependency.TubeConfig.textNormal);
                        bool tscReturnDone = false;
                        float distance = Vector3.Distance(currentTube.transform.position, resultData.ResetPosition.position);
                        float moveTime = Mathf.Lerp(BEPS01RFSHandleData.MIN_TIME, BEPS01RFSHandleData.MAX_TIME, distance / BEPS01RFSHandleData.MAX_DISTANCE);
                        currentTube.transform.DOMove(resultData.ResetPosition.position, moveTime).SetEase(Ease.InOutCubic).OnComplete(() =>
                        {
                            currentTube.ResetSortingOrder();
                            tscReturnDone = true;
                        });
                        await UniTask.WaitUntil(() => tscReturnDone, cancellationToken: cts.Token);
                        BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragWrong, null);
                    }
                }
                else
                {
                    bool tscReturnDone = false;
                    float distance = Vector3.Distance(currentTube.transform.position, resultData.ResetPosition.position);
                    float moveTime = Mathf.Lerp(BEPS01RFSHandleData.MIN_TIME, BEPS01RFSHandleData.MAX_TIME, distance / BEPS01RFSHandleData.MAX_DISTANCE);
                    currentTube.transform.DOMove(resultData.ResetPosition.position, moveTime).SetEase(Ease.InOutCubic).OnComplete(() =>
                    {
                        currentTube.ResetSortingOrder();
                        tscReturnDone = true;
                    });
                    await UniTask.WaitUntil(() => tscReturnDone, cancellationToken: cts.Token);
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
                }

            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }*/


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

    public class BEPS01RFSDragResultStateEventData
    {
        public GameObject TubeObject { get; set; }
        public GameObject DashBoxObject { get; set; }
        public Transform ResetPosition { get; set; }
        public BEPS01RFSUserInput UserInput { get; set; }
    }
    public class BEPS01RFSDragResultStateObjectDependency
    {
        public BEPS01RFSSkeletonConfig SkeletonConfig { get; set; }
        public BEPS01RFSDragResultConfig DragResultConfig { get; set; }
        public BEPS01RFSTubeConfig TubeConfig { get; set; }
        public BEPS01RFSGuiding Guiding { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
    }
}