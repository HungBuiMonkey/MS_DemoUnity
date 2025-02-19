using DG.Tweening;
using MonkeyBase.Observer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{

    public class BEPS02Guiding : MonoBehaviour
    {
        [SerializeField] Image handLong;
        [SerializeField] Image handShort;
        private bool isGuiding = false;
        private int guidingCount = 0;
        private BEPS02FlyingOwlsGuidingConfig guidingConfig;
        private Vector3 owlPos;
        private Vector3 shadowOwlPos;
        private CanvasGroup canvasGroup;
        private Coroutine coroutineGuiding;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            SetColorImage(handLong, 1f);
            SetColorImage(handShort, 0f);
        }

        public void InitData(Vector3 owlPos, Vector3 shadowOwlPos, BEPS02FlyingOwlsGuidingConfig guidingConfig) {
            this.owlPos = owlPos;
            this.shadowOwlPos = shadowOwlPos;
            this.guidingConfig = guidingConfig;
        }

        public void StartGuiding(bool isDelay)
        {
            ResetGuiding();
            isGuiding = true;
            transform.localScale = Vector3.one;
            coroutineGuiding = StartCoroutine(DoSomethingDelay(isDelay));
        }
        private IEnumerator DoSomethingDelay(bool isDelay)
        {
           if(isDelay) yield return new WaitForSeconds(guidingConfig.secondWaitStartGuiding);
            while (isGuiding)
            {
                if (!isGuiding) break;
                if (guidingCount == 3)
                {
                    guidingCount = 0;
                    yield return new WaitForSeconds(guidingConfig.secondWaitStartGuiding);
                }
                SoundChannel soundData;

                transform.position = owlPos;
                canvasGroup.DOFade(1, guidingConfig.secondDelay).SetEase(Ease.Linear);

                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, guidingConfig.sfxAppear, null, 1, false);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                if (guidingCount == 0)
                {
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND, GetAudioTopic(), null, 1, false);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                }

                yield return new WaitForSeconds(guidingConfig.secondDelay * 3);
                SetColorImage(handLong, 0f);
                SetColorImage(handShort, 1f);

                yield return transform.DOMove(shadowOwlPos, 1f).SetEase(Ease.Linear).WaitForCompletion();
                yield return new WaitForSeconds(guidingConfig.secondDelay);
                SetColorImage(handLong, 1f);
                SetColorImage(handShort, 0f);
                yield return canvasGroup.DOFade(0, guidingConfig.secondDelay).SetEase(Ease.Linear).WaitForCompletion();
                transform.position = owlPos;
                if (guidingCount == 0)
                {
                    yield return new WaitForSeconds(GetAudioTopic().length);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
                guidingCount++;
            }
        }

        public void ResetGuiding()
        {
            if (coroutineGuiding != null) StopCoroutine(coroutineGuiding);
            guidingCount = 0;
            isGuiding = false;
            transform.DOKill();
            transform.localScale = Vector3.zero;
            canvasGroup.alpha = 0f; ;
        }

        private AudioClip GetAudioTopic()
        {
            return guidingConfig.audiosTopic[UnityEngine.Random.Range(0, guidingConfig.audiosTopic.Count)];
        }

        private void SetColorImage(Image image, float indexColor)
        {
            Color currentColor = image.color;
            currentColor.a = indexColor;
            image.color = currentColor;
        }
    }
}