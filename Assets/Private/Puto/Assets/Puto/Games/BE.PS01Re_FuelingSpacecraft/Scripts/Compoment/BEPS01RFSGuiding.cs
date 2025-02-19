using DG.Tweening;
using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSGuiding : MonoBehaviour
    {
        [SerializeField] private Image handLong;
        [SerializeField] private Image handShort;
        [SerializeField] private Transform pointSpawnGuiding;
        private BEPS01RFSGuidingConfig guidingConfig;
        private bool isGuiding = false;
        private BEPS01RFSTubeItem tubeItem;
        private BEPS01RFSDashBoxItem dashBoxItem;
        private BaseButton buttonSKip;
        private CanvasGroup canvasGroup;
        private Coroutine coroutineGuiding;

        private BEPS01RFSTubeItem tempItemDrag;
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            SetColorImage(handLong, 1f);
            SetColorImage(handShort, 0f);
        }
        public void InitData(BEPS01RFSGuidingConfig guidingConfig, BaseButton buttonSKip)
        {
            this.guidingConfig = guidingConfig;
            this.buttonSKip = buttonSKip;
        }
        public void InitData(BEPS01RFSTubeItem tubeItem, BEPS01RFSDashBoxItem dashBoxItem)
        {
            this.tubeItem = tubeItem;
            this.dashBoxItem = dashBoxItem;
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
            SoundChannel soundData;
            if (isDelay) yield return new WaitForSeconds(guidingConfig.secondWaitStartGuiding);
            if (tempItemDrag == null)
            {
                tempItemDrag = Instantiate(tubeItem, pointSpawnGuiding, false);
                tempItemDrag.transform.localPosition = Vector3.zero;
                tempItemDrag.Enable(false);
                tempItemDrag.transform.SetAsFirstSibling();
                tempItemDrag.GetComponent<CanvasGroup>().alpha = 0.5f;
                tempItemDrag.GetComponent<Canvas>().sortingOrder = 2;
                tempItemDrag.GetComponent<GraphicRaycaster>().enabled = false;
            }
            while (isGuiding)
            {
                buttonSKip.gameObject.SetActive(true);
                transform.position = tubeItem.transform.position;
                if (tempItemDrag != null) tempItemDrag.transform.position = tubeItem.transform.position;
                canvasGroup.DOFade(1, guidingConfig.secondDelay * 1.5f).SetEase(Ease.Linear).onComplete += () => {
                    SetColorImage(handLong, 1f);
                    SetColorImage(handShort, 0f);
                };
                pointSpawnGuiding.GetComponent<CanvasGroup>().DOFade(1, guidingConfig.secondDelay * 1.5f).SetEase(Ease.Linear);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, guidingConfig.sfxAppear);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                yield return new WaitForSeconds(guidingConfig.secondDelay * 2);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, guidingConfig.sfxClick);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                SetColorImage(handLong, 0f);
                SetColorImage(handShort, 1f);
                yield return new WaitForSeconds(guidingConfig.secondDelay);
                if (tempItemDrag != null) tempItemDrag.transform.DOMove(dashBoxItem.transform.position, 1f).SetEase(Ease.InOutQuad);
                yield return transform.DOMove(dashBoxItem.transform.position, 1f).SetEase(Ease.InOutQuad).WaitForCompletion();
                yield return new WaitForSeconds(guidingConfig.secondDelay);
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND, guidingConfig.sfxUnClick);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
                SetColorImage(handLong, 1f);
                SetColorImage(handShort, 0f);
                yield return new WaitForSeconds(guidingConfig.secondDelay);
                pointSpawnGuiding.GetComponent<CanvasGroup>().DOFade(0, guidingConfig.secondDelay * 2).SetEase(Ease.Linear);
                yield return canvasGroup.DOFade(0, guidingConfig.secondDelay * 2).SetEase(Ease.Linear).WaitForCompletion();
                buttonSKip.gameObject.SetActive(false);
                yield return new WaitForSeconds(guidingConfig.secondWaitStartGuiding);
            }
        }

        public void ResetGuiding()
        {
            SoundManager.Instance.StopFx();
            buttonSKip.gameObject.SetActive(false);
            isGuiding = false;
            transform.DOKill();
            transform.localScale = Vector3.zero;
            canvasGroup.DOFade(0f, 0.1f).onComplete += () => {
                SetColorImage(handLong, 0f);
                SetColorImage(handShort, 0f);
            };
            if (coroutineGuiding != null) StopCoroutine(coroutineGuiding);
            pointSpawnGuiding.GetComponent<CanvasGroup>().DOFade(0f, 0.1f).SetEase(Ease.Linear).onComplete += () => {
                if (tempItemDrag != null)
                {
                    Destroy(tempItemDrag);
                    BEPS01RFSHandleData.DestroyItem(pointSpawnGuiding);
                }
            };
    
        }
        private void SetColorImage(Image image, float indexColor)
        {
            Color currentColor = image.color;
            currentColor.a = indexColor;
            image.color = currentColor;
        }

    }
}