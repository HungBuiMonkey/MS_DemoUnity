using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MonkeyBase.Observer;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJAnswer : BEID01IntroDialogMJBoxChat
    {
        [SerializeField] private RectTransform mainContent;
        [SerializeField] private RectTransform contentPopup;
        [SerializeField] private AudioClip sfxAudioAppear;
        [SerializeField] private Button dialogClick;
        [SerializeField] private float maxWidth;

        private void Start()
        {
            dialogClick.onClick.AddListener(Click);
        }

        public override void InitData(BEID01IntroDialogMJBoxChatData data)
        {
            this.data = data;
            mkText.text = BreakLines(data.text);
        }

        private string BreakLines(string text)
        {
            string[] words = text.Split(" ");
            int maxCharacterPerLine = 45;
            string textResult = "";
            int lengthWord = 0;

            for (int i = 0; i < words.Length; ++i)
            {
                if (lengthWord + words[i].Length + 1 > maxCharacterPerLine)
                {
                    textResult += $"\n{words[i]} ";
                    lengthWord = words[i].Length + 1;
                }
                else
                {
                    textResult += $"{words[i]} ";
                    lengthWord += words[i].Length + 1;
                }
            }

            return textResult;
        }

        public override void SetReviewClick(bool value)
        {
            reviewClick = value;
        }


        public override void Play(Action callback = null)
        {
            gameObject.SetActive(true);
            StartCoroutine(IPlay(callback));
        }

        private IEnumerator IPlay(Action callback = null)
        {
            contentPopup.transform.localScale = Vector3.zero;
            SoundChannel popup = new SoundChannel(SoundChannel.PLAY_SOUND, sfxAudioAppear, null, volume: 0.5f);
            ObserverManager.TriggerEvent<SoundChannel>(popup);

            contentPopup.DOScale(Vector2.one, 0.25f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(0.25f);

            yield return new WaitForSeconds(0.2f);
            SoundChannel audio = new SoundChannel(SoundChannel.PLAY_SOUND, data.audio, null);
            ObserverManager.TriggerEvent<SoundChannel>(audio);
            yield return new WaitForSeconds(data.audio.length);
            callback?.Invoke();

            yield return new WaitForSeconds(0.5f);
            BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.PLAY_STATE_START, null);
            ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
        }

        private void Click()
        {
            if (reviewClick == true)
            {
                BEID01MJUserInputChannel reviewClick = new BEID01MJUserInputChannel(BEID01MJUserInputChannel.BUTTON_REVIEW_CLICK, gameObject);
                ObserverManager.TriggerEvent<BEID01MJUserInputChannel>(reviewClick);
            }
        }

        public override void DisableLayout()
        {
            contentPopup.GetComponent<ContentSizeFitter>().enabled = false;
            contentPopup.GetComponent<VerticalLayoutGroup>().enabled = false;
            mainContent.GetComponent<ContentSizeFitter>().enabled = false;
            mainContent.GetComponent<VerticalLayoutGroup>().enabled = false;
            canvasGroup.enabled = false;
        }
    }
}