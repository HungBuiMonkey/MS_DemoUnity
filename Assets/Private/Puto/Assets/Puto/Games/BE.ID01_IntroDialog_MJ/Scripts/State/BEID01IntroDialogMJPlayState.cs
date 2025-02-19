using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonkeyBase.Observer;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Coffee.UIExtensions;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJPlayState : FSMState
    {
        private BEID01IntroDialogMJPlayStateDependency dependency;
        private CancellationTokenSource cts;
        private List<BEID01IntroDialogMJBoxChat> listBoxChat;
        private BEID01IntroDialogMJPlayStateData playStateData;
        private int currentBoxChatIndex;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            cts = new();
            playStateData = (BEID01IntroDialogMJPlayStateData)data;
            DoWork(playStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJPlayStateDependency)data;
        }
        private async void DoWork(BEID01IntroDialogMJPlayStateData playStateData)
        {
            try
            {
                listBoxChat = playStateData.ListBoxChat;
                var rect = dependency.ConversationContainer.GetScrollRect();
                var content = dependency.ConversationContainer.GetContentRectTransform();
                rect.enabled = false;
                if (currentBoxChatIndex >= listBoxChat.Count)
                {
                    currentBoxChatIndex = 0;
                    await UniTask.Delay(dependency.PlayConfig.milisecondDelayAppearNextButton, cancellationToken: cts.Token);
                    var nextButton = dependency.NextButton.GetComponentInChildren<Button>();
                    nextButton.onClick.AddListener(OnClickNextButton);

                    dependency.NextButton.SetActive(true);
                    dependency.NextButton.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InOutBack);
                    await UniTask.Delay(dependency.PlayConfig.milisecondDelayAppearNextButton, cancellationToken: cts.Token);
                    StartFirework();
                    bool tscAudioYeah = false;
                    SoundChannel soundChannel = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dependency.PlayConfig.yeahAudio, () =>
                    {
                        tscAudioYeah = true;
                    });
                    ObserverManager.TriggerEvent<SoundChannel>(soundChannel);
                    await UniTask.WaitUntil(() => tscAudioYeah, cancellationToken: cts.Token);
                    StoptFirework();
                    BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.REVIEW_STATE_START, null);
                    ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
                }
                else
                {
                    listBoxChat[currentBoxChatIndex].Play();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                    currentBoxChatIndex++;
                }
            }
            catch (System.Exception)
            {

            }
        }
        private void OnClickNextButton()
        {
            dependency.NextButton.GetComponentInChildren<Button>().interactable = false;
            //next game
            foreach (var item in listBoxChat)
            {
                item.SetReviewClick(false);
            }
            dependency.LayerFade.DOFade(1f, dependency.PlayConfig.timeFadeOut).OnComplete(() =>
            {
                BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.GAME_PLAY_END, null);
                ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
            });
        }
        private void StartFirework()
        {
            dependency.UIParticle.gameObject.SetActive(true);
            foreach (ParticleSystem particle in dependency.Fireworks)
            {
                particle.Play();
            }
        }
        private void StoptFirework()
        {
            foreach (ParticleSystem particle in dependency.Fireworks)
            {
                particle.Stop();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            StoptFirework();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            StoptFirework();
            dependency.UIParticle.gameObject.SetActive(false);
            cts?.Cancel();
            cts?.Dispose();
        }
    }
    public class BEID01IntroDialogMJPlayStateData
    {
        public List<BEID01IntroDialogMJBoxChat> ListBoxChat { get; set; }
    }
    public class BEID01IntroDialogMJPlayStateDependency
    {
        public BEID01MJConversationContainer ConversationContainer { get; set; }
        public BEID01MJPlayConfig PlayConfig { get; set; }
        public GameObject NextButton { get; set; }
        public Image LayerFade { get; set; }
        public List<ParticleSystem> Fireworks { get; set; }
        public UIParticle UIParticle { get; set; }
    }
}