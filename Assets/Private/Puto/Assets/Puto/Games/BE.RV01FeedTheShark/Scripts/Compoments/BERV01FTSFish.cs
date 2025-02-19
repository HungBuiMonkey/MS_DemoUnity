using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSFish : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private SkeletonGraphic skeletonFish;
        [SerializeField] private SkeletonGraphic skeletonStunned;
        [SerializeField] private TMP_Text text;
        [SerializeField] private BERV01TypeFish typeFish;
        private float fishSpeed = 1.2f;
        private float fishSpeedWrong = 12f;
        private float timeFishScale = 0.2f;
        private bool isMoving, isEnable, isPlaying = false;
        private BERV01FishDirection fishDirection;
        private BERV01FTSFishSwimmingLane swimmingLane;
        private BERV01FTSClickConfig clickConfig;
        private BERV01FTSFishData fishData;
        private Vector3 vectorScale;
        private RectTransform rectTransform;

        public BERV01FTSFishData FishData {
            get { return fishData; }
            set { fishData = value; }
        }
        public BERV01FTSFishSwimmingLane SwimmingLane
        {
            get { return swimmingLane; }
            set {
                swimmingLane = value;
            }
        }
        public BERV01FishDirection FishDirection
        {
            get { return fishDirection; }
            set { 
                fishDirection = value;
                if(fishDirection == BERV01FishDirection.Left)
                {
                    swimmingLane.SetPositionLeftX((typeFish == BERV01TypeFish.Short) ? BERV01FTSHandleData.SHORT_LANE_FISH_X : BERV01FTSHandleData.LONG_LANE_FISH_X);
                    transform.position = swimmingLane.PointRight.position;
                    transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                    text.transform.localRotation = new Quaternion(0f, 180f, 0f, 0f);
                } else
                {
                    swimmingLane.SetPositionRightX((typeFish == BERV01TypeFish.Short) ? BERV01FTSHandleData.SHORT_LANE_FISH_X : BERV01FTSHandleData.LONG_LANE_FISH_X);
                    transform.position = swimmingLane.PointLeft.position;
                } 
            }
        }
        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }
        public TMP_Text TextFish
        {
            get { return text; }
        }
        public SkeletonGraphic SkeletonFish
        {
            get { return skeletonFish; }
        }

        public void InitData(BERV01FTSFishData fishData, BERV01FTSClickConfig clickConfig)
        {
            this.fishData = fishData;
            this.clickConfig = clickConfig;
            string fishText = fishData.text;

            if (fishData.text.Contains(" "))
            {
                string inputReplace = fishData.text.Replace(" ", "\n");
                fishText = inputReplace;
            }
            text.text = fishText;
            BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishIdle, true, null);
            vectorScale = new Vector3(0.8f, 0.8f, 0.8f);
            rectTransform = GetComponent<RectTransform>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isEnable) return;
            isPlaying = true;
            BERV01FTSHandleData.TriggerFinishState(BERV01State.PlayGame, (true, false));
            EnableMovingFish(false);
            transform.DOScale(vectorScale, timeFishScale).SetEase(Ease.InSine).onComplete += () =>
            {
                transform.DOScale(Vector3.one, timeFishScale).SetEase(Ease.OutSine);
            };
            BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishUserTap, true, null);
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isEnable) return;
            if (BERV01FTSHandleData.CurrentIndexFishCorrect < BERV01FTSHandleData.MAX_FISH_CORRECT)
            {
                if (fishData.isCorrect)
                    OnCorrect();
                else
                    StartCoroutine(OnWrong());
            } else
            {
                BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishIdle, true, null);
                EnableMovingFish(true);
            }
        }

        public void EnableMovingFish(bool isEnable, float timeDelay = 0)
        {
          
            isMoving = isEnable;
            if (isMoving) 
                StartCoroutine(MoveFish(timeDelay));
            else 
                transform.DOKill();
        }

        private IEnumerator MoveFish(float timeDelay = 0)
        {
            if (!gameObject.activeInHierarchy) yield break;
            yield return new WaitForSeconds(timeDelay);
            Transform pointStart = (fishDirection == BERV01FishDirection.Left) ? swimmingLane.PointRight : swimmingLane.PointLeft;
            Transform pointEnd = (fishDirection == BERV01FishDirection.Left) ? swimmingLane.PointLeft : swimmingLane.PointRight;

            transform.DOMoveX(pointEnd.position.x, fishSpeed)
             .SetEase(Ease.Linear).SetSpeedBased()
             .OnComplete(() => {
                 transform.position = new Vector3(pointStart.position.x, transform.position.y, transform.position.z);
                 if(!fishData.isCorrect) BERV01FTSHandleData.TriggerStateFish(BERV01FishState.SwimToPlace, this);
                 if(gameObject.activeInHierarchy) StartCoroutine(MoveFish());
             });
        }


        public void MoveFishWithSpeed(float speed, Action onMoveDone = null)
        {
            transform.DOKill();
            Transform pointEnd = (fishDirection == BERV01FishDirection.Left) ? swimmingLane.PointLeft : swimmingLane.PointRight;
            transform.DOMoveX(pointEnd.position.x, speed)
             .SetEase(Ease.Linear).SetSpeedBased().onComplete += () => {
                 onMoveDone?.Invoke();
             };
        }

        private void OnCorrect()
        {
            isEnable = false;
            BERV01FTSHandleData.TriggerStateFish(BERV01FishState.SharkBite, this);
            SoundChannel soundData;
            BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishUserUnTap, true, (trackEntry) => {
                BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishIdle, true, null);
            });
            skeletonStunned.transform.localScale = Vector3.one;
            BERV01FTSHandleData.SetAnimation(skeletonStunned, BERV01FTSHandleData.stunned, true, null);
            soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, clickConfig.sfxCorrect, () => {
                soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, fishData.audio);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);
            });
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
        }

        private IEnumerator OnWrong()
        {
            isEnable = false;
            bool isEffectDone = false;
            SoundChannel soundData;
            StartCoroutine(IEEffectWrong(callback: () => { isEffectDone = true; }));
            soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, clickConfig.sfxWrong);
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
            yield return new WaitForSeconds(clickConfig.sfxWrong.length);
            yield return new WaitUntil(() => isEffectDone);
            soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, fishData.audio);
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
            BERV01FTSHandleData.SetAnimation(skeletonFish, BERV01FTSHandleData.fishIdle, true, null);
            MoveFishWithSpeed(fishSpeedWrong, () => { 
                EnableMovingFish(true);
                isEnable = true;
                isPlaying = false;
                BERV01FTSHandleData.TriggerFinishState(BERV01State.PlayGame, null);
            });
        }
     
        private IEnumerator IEEffectWrong(float duration = 0.25f, Action? callback = null, bool isRunEffectLocal = true)
        {
            float amplitude = rectTransform.sizeDelta.y * 0.1f;
            var defaultPos = transform.localPosition;
            var arg = new int[5] { 123, 250, 42, 193, 302 };
            var factor = 1f;
            for (int i = 0; i < 5; i++)
            {
                var px = (float)amplitude * factor * (float)Math.Cos(Mathf.Deg2Rad * arg[i]);
                var py = (float)amplitude * factor * (float)Math.Sin(Mathf.Deg2Rad * arg[i]);
                if (isRunEffectLocal)
                {
                    LeanTween.moveLocal(gameObject, defaultPos + new Vector3(px, py, 0), duration / 6);
                }
                else
                {
                    LeanTween.move(gameObject, defaultPos + new Vector3(px, py, 0), duration / 6);
                }
                factor -= 0.05f;
                yield return new WaitForSeconds(duration / 6);
            }
            LeanTween.moveLocal(gameObject, defaultPos, duration / 6);
            yield return new WaitForSeconds(duration / 6);
            callback?.Invoke();
        }
    }
}