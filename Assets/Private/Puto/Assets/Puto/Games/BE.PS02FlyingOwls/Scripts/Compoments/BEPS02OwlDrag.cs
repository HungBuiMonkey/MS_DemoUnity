using Cysharp.Threading.Tasks;
using DG.Tweening;
using Monkey.MJ5.BEPS01Re_FuelingSpacecraft;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    [Serializable]
    public class BEPS02DataOwl
    {
        public AudioClip audio;
        public string text;
    }

    public class BEPS02OwlDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform itemTransfrom;
        [SerializeField] private SkeletonGraphic owlAnimation;
        [SerializeField] private BaseText textContent;
        private List<RectTransform> pointsShadowTransform;
        private RectTransform resetPosition;
        private BEPS02Guiding guiding;
        private BEPS02OwlConfig owlConfig;
        private BEPS02FlyingOwlsDragConfig dragConfig;
        private bool isEnable = true;
        private int indexItem;
        private bool isDragging = false;
        private bool isDrag = false;
        private bool isClicking = false;
        private bool isDraggable = false;
        private bool isPlaying = false;
        private BEPS02DataOwl dataOwl;

        public void InitData(List<RectTransform> pointsShadow, RectTransform resetPosition)
        {
            this.pointsShadowTransform = pointsShadow;
            this.resetPosition = resetPosition;
        }
        public void InitData(BEPS02DataOwl dataOwl, BEPS02OwlConfig owlConfig, BEPS02FlyingOwlsDragConfig dragConfig, BEPS02Guiding guiding)
        {
            this.dataOwl = dataOwl;
            this.owlConfig = owlConfig;
            this.dragConfig = dragConfig;
            this.guiding = guiding;
            indexItem = BEPS02HandleData.GetIndexByName(gameObject.name);
            textContent.SetText(dataOwl.text);
            textContent.GetComponent<TMP_Text>().ForceMeshUpdate();
        }
        public void Enable(bool value)
        {
            isEnable = value;
        }
        public void EnableClick(bool value)
        {
            isClicking = value;
        }
        public bool IsDragObject()
        {
            return isDrag;
        }
        public bool IsPlaying()
        {
            return isPlaying;
        }
        public void SetPlaying(bool value)
        {
            isPlaying = value;
        }
        public void SetDragObject(bool value)
        {
            isDrag = value;
        }
        public SkeletonGraphic GetSkeleton()
        {
            return owlAnimation;
        }
        public int GetIndex()
        {
            return indexItem;
        }
        public BEPS02DataOwl GetData()
        {
            return dataOwl;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isEnable || isDraggable) return;
            guiding.ResetGuiding();
            isPlaying = true;
            if (isDrag)
            {
                transform.SetAsLastSibling();
                BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.Dragging, gameObject);
            }
            else {
                if (!isClicking)
                {
                    isClicking = true;
                    BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.Click, gameObject);
                }
            }
            isDraggable = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isEnable || !isDraggable) return;
            if (isDrag && !isDragging)
            {
                isPlaying = false;
                int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlAnimation);
                BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlNormal, aniIndex, true, null);
                BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.UnClick, gameObject);
            }else
            {
                isDragging = false;
                isEnable = false;
                bool foundMatch = false;
                for (int i = 0; i < pointsShadowTransform.Count; i++)
                {
                    if (BEPS02HandleData.CheckTriggerOfTwoObject(itemTransfrom, pointsShadowTransform[i], 0.2f))
                    {
                        //BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.DragMatching, (gameObject, pointsShadowTransform[i].gameObject, resetPosition.transform));
                        BEPS02ShadowOwl shadowOwl = pointsShadowTransform[i].GetComponent<BEPS02ShadowOwl>();
                        if(!shadowOwl.IsDraged)
                        {
                            shadowOwl.IsDraged = true;
                            foundMatch = true;
                            bool isDragOwlMatch = BEPS02HandleData.AreIntegersEqual(indexItem, BEPS02HandleData.GetIndexByName(pointsShadowTransform[i].name));
                            if (isDragOwlMatch)
                            {
                                StartCoroutine(OnCorrect(pointsShadowTransform[i].gameObject));
                            }
                            else
                            {
                                StartCoroutine(OnWrong(pointsShadowTransform[i].gameObject));
                            }
                        }
                        break;
                    }
                }
                if (!foundMatch)
                {
                    if(isDrag) StartCoroutine(OnResetOwl());
                    //BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.DragUnMatching, (gameObject, resetPosition.transform));
                }

            }
            isDraggable = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isEnable || !isDraggable) return;
            if (isDrag)
            {
                if (!isDragging)
                {
                    isPlaying = true;
                    guiding.ResetGuiding();
                    transform.SetAsLastSibling();
                    isDragging = true;
                    BEPS02HandleData.IndDragScreen++;
                    BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.Dragging, gameObject);
                }
                RectTransformUtility.ScreenPointToLocalPointInRectangle(itemTransfrom.parent as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
                itemTransfrom.localPosition = localPoint;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isEnable || !isDraggable) return;
            if (isDrag)
            {
                guiding.ResetGuiding();
                transform.SetAsLastSibling();
                isDragging = true;           
                isPlaying = true;
                isDragging = true;
                BEPS02HandleData.IndDragScreen++;
                BEPS02HandleData.TriggerStateDrag(BEPS02FlyingOwlsUserInput.Dragging, gameObject);
            }
        }

        private IEnumerator OnResetOwl()
        {
            transform.SetAsLastSibling();
            bool tscReturnDone = false;
            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlAnimation);
            BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlDrop, aniIndex, false, (trackEntry) =>
            {
                BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayEffect, true);

                BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFly, aniIndex, true, null);
                transform.DOMove(resetPosition.position, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    BEPS02HandleData.IsPlayingEffect = false;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayEffect, false);
                    tscReturnDone = true;
                    isPlaying = false;
                    BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                    {

                        BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlNormal, aniIndex, true, null);
                    });
                });

            });
            yield return new WaitUntil(() => tscReturnDone);
            yield return new WaitForSeconds(0.35f);
            BEPS02FlyingOwlsPlayStateEventData playStateEventData = new BEPS02FlyingOwlsPlayStateEventData();
            playStateEventData.OwlObject = gameObject;
            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayGame, playStateEventData);
        }

        private IEnumerator OnCorrect(GameObject shadowOwlObject)
        {
            BEPS02HandleData.DragCorrectCount++;
            BEPS02HandleData.DragWrongCount = 0;
            transform.SetAsLastSibling();
            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlAnimation);
            bool tscMoveOwl = false;
            bool tscCorrectDone = false;
            SetDragObject(false);
            BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFly, aniIndex, true, null);
            transform.DOMove(shadowOwlObject.transform.position, 0.4f).SetEase(Ease.InOutSine).OnComplete(() => { tscMoveOwl = true; });
            yield return new WaitUntil(() => tscMoveOwl);
            BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
            {
                if (shadowOwlObject.GetComponent<Image>().color.a > 0)
                {
                    shadowOwlObject.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                }
                else
                {
                    var getShadow = pointsShadowTransform.Find((item) => item.GetComponent<Image>().color.a > 0);
                    if (getShadow != null)
                    {
                        getShadow.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                    }
                }
                SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dragConfig.sfxCorrect, null, 1, false);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);

                BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlTapCorrect, aniIndex, false, (trackEntry) =>
                {
                    shadowOwlObject.transform.localScale = Vector3.zero;
                    BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlNormal, aniIndex, true, null);
                    tscCorrectDone = true;
                    isPlaying = false;
                });
            });
            yield return new WaitUntil(() => tscCorrectDone);
            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragCorrect, null);
        }

        private IEnumerator OnWrong(GameObject shadowOwlObject)
        {
            BEPS02HandleData.DragWrongCount++;
            BEPS02HandleData.IndDragWrong++;
            if (BEPS02HandleData.DragWrongCount > 3) BEPS02HandleData.DragWrongCount = 0;

            transform.SetAsLastSibling();
            int aniIndex = BEPS02HandleData.GetIndexCurrentAnimation(owlAnimation);
            bool tscWrongDone = false;
           
            /* bool tscMoveOwl = false;
            BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFly, aniIndex, true, null);
             transform.DOMove(shadowOwlObject.transform.position, 0.4f).SetEase(Ease.InOutSine).OnComplete(() => { tscMoveOwl = true; });
             yield return new WaitUntil(() => tscMoveOwl);*/
            if (shadowOwlObject.GetComponent<Image>().color.a > 0)
            {
                shadowOwlObject.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
            }
            else
            {
                var getShadow = pointsShadowTransform.Find((item) => item.GetComponent<Image>().color.a > 0);
                if (getShadow != null)
                {
                    getShadow.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                }
            }

            /*BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
            {*/
                SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dragConfig.sfxWrong, null, 1, false);
                ObserverManager.TriggerEvent<SoundChannel>(soundData);

                BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlTapWrong, aniIndex, false, (trackEntry) =>
                {
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayEffect, true);
                    BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlNormalToFly, aniIndex, false, (trackEntry) =>
                    {
                        shadowOwlObject.GetComponent<BEPS02ShadowOwl>().IsDraged = false;        
                        BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFly, aniIndex, true, null);
                        transform.DOMove(resetPosition.position, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
                        {
                            BEPS02HandleData.IsPlayingEffect = false;
                            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayEffect, false);
                            tscWrongDone = true;
                            isPlaying = false;
                            BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlFlyToNormal, aniIndex, false, (trackEntry) =>
                            {
                                BEPS02HandleData.SetNumberAnimation(owlAnimation, owlConfig.owlNormal, aniIndex, true, null);
                            });
                        });
                    });

                });

            /*});*/
            yield return new WaitUntil(() => tscWrongDone);
            yield return new WaitForSeconds(0.35f);
            BEPS02FlyingOwlsPlayStateEventData playStateEventData = new BEPS02FlyingOwlsPlayStateEventData();
            playStateEventData.OwlObject = gameObject;
            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragWrong, playStateEventData);
        }
    }
}