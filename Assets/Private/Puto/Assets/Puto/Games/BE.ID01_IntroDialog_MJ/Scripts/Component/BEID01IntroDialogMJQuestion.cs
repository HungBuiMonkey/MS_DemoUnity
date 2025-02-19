using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine.Unity;

using MonkeyBase.Observer;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJQuestion : BEID01IntroDialogMJBoxChat
    {
        [SerializeField] private RectTransform mainContent;
        [SerializeField] private RectTransform contentPopup;
        [SerializeField] private Button dialogClick;
        [SerializeField] private AudioClip sfxAudioAppear;
        [SerializeField] private float maxWidth;

        private bool canClick = false;
      
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
        public override void DisableLayout()
        {
            contentPopup.GetComponent<ContentSizeFitter>().enabled = false;
            contentPopup.GetComponent<HorizontalLayoutGroup>().enabled = false;
            mainContent.GetComponent<ContentSizeFitter>().enabled = false;
            mainContent.GetComponent<VerticalLayoutGroup>().enabled = false;
            canvasGroup.enabled = false;
        }
        public override void Play(Action callback = null)
        {
            gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPopup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mainContent);

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPopup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(mainContent);
            StartCoroutine(IPlay(callback));
        }
        private IEnumerator IPlay(Action callback = null)
        {
            //Appear
            contentPopup.transform.localScale = Vector3.zero;
            SoundChannel popup = new SoundChannel(SoundChannel.PLAY_SOUND, sfxAudioAppear, null, volume: 0.5f);
            ObserverManager.TriggerEvent<SoundChannel>(popup);
            contentPopup.DOScale(Vector2.one, 0.25f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(0.25f);
            //canClick = true;
            callback?.Invoke();
            BEID01MJUserInputChannel buttonClick = new BEID01MJUserInputChannel(BEID01MJUserInputChannel.BUTTON_CLICK, gameObject);
            ObserverManager.TriggerEvent<BEID01MJUserInputChannel>(buttonClick);
        }
        private void Click()
        {
            if (canClick == true)
            {
                BEID01MJUserInputChannel buttonClick = new BEID01MJUserInputChannel(BEID01MJUserInputChannel.BUTTON_CLICK, gameObject);
                ObserverManager.TriggerEvent<BEID01MJUserInputChannel>(buttonClick);
            }
            else
            {
                if (reviewClick == true)
                {
                    BEID01MJUserInputChannel reviewClick = new BEID01MJUserInputChannel(BEID01MJUserInputChannel.BUTTON_REVIEW_CLICK, gameObject);
                    ObserverManager.TriggerEvent<BEID01MJUserInputChannel>(reviewClick);
                }
            }
        }
        public void SetClick(bool value)
        {
            canClick = value;
        }
    }
}