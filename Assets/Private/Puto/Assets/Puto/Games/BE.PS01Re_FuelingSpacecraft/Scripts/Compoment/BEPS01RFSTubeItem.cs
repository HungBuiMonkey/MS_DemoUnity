using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    [Serializable]
    public class BEPS01RFSTubeData
    {
        public int id;
        public string text;
        public AudioClip audio;
    }

    public class BEPS01RFSTubeItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform itemTransfrom;
        [SerializeField] private SkeletonGraphic skeleton;
        [SerializeField] private SkeletonGraphic fireWork;
        [SerializeField] private Image background;
        [SerializeField] private Image boxText;
        [SerializeField] private TMP_Text text;
        private Canvas itemCanvas;
        private List<BEPS01RFSDashBoxItem> dashBoxs;
        private RectTransform resetPosition;
        private BEPS01RFSTubeData tubeData;
        private BEPS01RFSTubeConfig tubeConfig;
        private BEPS01RFSDragResultConfig dragResultConfig;
        private BEPS01RFSSkeletonConfig skeletonConfig;
        private BEPS01RFSGuiding guiding;
        private bool isEnable = false;
        private bool isDrag = false;
        private bool isDragging = false;
        private bool isDraggable = false;
        private float sizeOulỉneText = 0;
        private int currentSortingOrder = 0;


        private void Awake()
        {
            itemCanvas = GetComponent<Canvas>();
            sizeOulỉneText = text.outlineWidth;

        }
        private void Start()
        {
            currentSortingOrder = itemCanvas.sortingOrder;
        }
        public void InitData(BEPS01RFSTubeData tubeData, BEPS01RFSTubeConfig tubeConfig,
            BEPS01RFSDragResultConfig dragResultConfig, BEPS01RFSSkeletonConfig skeletonConfig, BEPS01RFSGuiding guiding)
        {
            this.tubeData = tubeData;
            this.tubeConfig = tubeConfig;
            this.dragResultConfig = dragResultConfig;
            this.skeletonConfig = skeletonConfig;
            this.guiding = guiding;
            text.SetText(tubeData.text);
            text.ForceMeshUpdate();
            SetSizeOutline(0);
        }
        public void InitData(List<BEPS01RFSDashBoxItem> dashBoxs, RectTransform resetPosition)
        {
            this.dashBoxs = dashBoxs;
            this.resetPosition = resetPosition;
        }

        public void SetColor(BEPS01RFSColor color)
        {
            background.color = color.background;
            boxText.color = color.box;
        }


        public void SetColorText(Color32 color)
        {
            text.color = color;
        }
        public void SetSizeOutline(float value)
        {
            text.outlineWidth = value;
        }
        public void ResetSizeOutline()
        {
            text.outlineWidth = sizeOulỉneText;
        }

        public SkeletonGraphic GetSkeleton()
        {
            return skeleton;
        }
        public SkeletonGraphic GetFirework()
        {
            return fireWork;
        }

        public BEPS01RFSTubeData GetData()
        {
            return tubeData;
        }
        public TMP_Text GetText()
        {
            return text;
        }

        public void Enable(bool value)
        {
            isEnable = value;
        }
        public bool IsDragObject()
        {
            return isDrag;
        }
        public void SetDragObject(bool value)
        {
            isDrag = value;
        }
        public void ResetSortingOrder()
        {
            itemCanvas.sortingOrder = currentSortingOrder;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isEnable || isDraggable) return;
            guiding.ResetGuiding();
            transform.SetAsLastSibling();
            isDraggable = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isEnable || !isDraggable) return;
            if (!isDragging)
            {
                ResetSortingOrder();
                BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.Click, gameObject);
            }
            else
            {
                isDragging = false;
                isEnable = false;
                bool foundMatch = false;
                for (int i = 0; i < dashBoxs.Count; i++)
                {
                    if (BEPS01RFSHandleData.CheckTriggerOfTwoObject(itemTransfrom, dashBoxs[i].GetComponent<RectTransform>(), 0.2f) && !dashBoxs[i].GetSelected())
                    {
                        foundMatch = true;
                        bool isTubeMatch = BEPS01RFSHandleData.AreIntegersEqual(tubeData.id, dashBoxs[i].GetId());
                        if (isTubeMatch)
                        {
                            StartCoroutine(OnCorrect(dashBoxs[i]));
                        } else
                        {
                            StartCoroutine(OnWrong());
                        }
                        //BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.DragMatching, (gameObject, dashBoxs[i].gameObject, resetPosition.transform));
                        break;
                    }
                }
                if (!foundMatch)
                {
                    StartCoroutine(OnResetTube());
                    //BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.DragUnMatching, (gameObject, resetPosition.transform));
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
                    itemCanvas.sortingOrder = BEPS01RFSHandleData.SORTING_ORDER_DRAG;
                    guiding.ResetGuiding();
                    transform.SetAsLastSibling();
                    isDragging = true;
                    BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.Dragging, gameObject);
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
                itemCanvas.sortingOrder = BEPS01RFSHandleData.SORTING_ORDER_DRAG;
                guiding.ResetGuiding();
                transform.SetAsLastSibling();
                isDragging = true;
                BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.Dragging, gameObject);
            }
        }

        private IEnumerator OnResetTube()
        {
            bool tscReturnDone = false;
            float distance = Vector3.Distance(transform.position, resetPosition.position);
            float moveTime = Mathf.Lerp(BEPS01RFSHandleData.MIN_TIME, BEPS01RFSHandleData.MAX_TIME, distance / BEPS01RFSHandleData.MAX_DISTANCE);
            transform.DOMove(resetPosition.position, moveTime).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                ResetSortingOrder();
                tscReturnDone = true;
            });
            yield return new WaitUntil(() => tscReturnDone);
            BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
        }


        private IEnumerator OnCorrect(BEPS01RFSDashBoxItem currentDashBox)
        {
            SetDragObject(false);
            bool tscMoveTube = false;
            transform.DOMove(currentDashBox.transform.position, 0.35f).SetEase(Ease.InOutSine).OnComplete(() => { tscMoveTube = true; });
            yield return new WaitUntil(() => tscMoveTube);
            currentDashBox.Fade(0f);
            currentDashBox.SetSelected(true);
            bool tscCorrectDone = false;
            SetColor(tubeConfig.colorCorrect);
            SetColorText(tubeConfig.textCorrect);
            SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dragResultConfig.sfxCorrect);
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
            BEPS01RFSHandleData.SetAnimation(skeleton, skeletonConfig.tubeCorrect, false, (trackEntry) =>
            {
                tscCorrectDone = true;
                ResetSortingOrder();
                SetColor(tubeConfig.colorNormal);
                SetColorText(tubeConfig.textNormal);
                BEPS01RFSHandleData.SetAnimation(skeleton, skeletonConfig.tubeNormal, false, null);
            });
            yield return new WaitForSeconds(BEPS01RFSHandleData.MilisecondsToSeconds(dragResultConfig.timeDelay));

            bool tscFireDone = false;
            int randomIndex = UnityEngine.Random.RandomRange(0, skeletonConfig.fireworks.Length);
            fireWork.gameObject.SetActive(true);
            BEPS01RFSHandleData.SetAnimation(fireWork, skeletonConfig.fireworks[randomIndex], false, (trackEntry) =>
            {
                fireWork.gameObject.SetActive(false);
                tscFireDone = true;
            });
            yield return new WaitUntil(() => tscCorrectDone);
            yield return new WaitForSeconds(BEPS01RFSHandleData.MilisecondsToSeconds(dragResultConfig.timeDelay) + 0.05f);
            bool tscAudio = false;
            soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, tubeData.audio, () => { tscAudio = true; });
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
            yield return new WaitUntil(() => tscFireDone && tscAudio);
          
            BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragCorrect, null);
        }


        private IEnumerator OnWrong()
        {
            BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragWrong, null);
            bool tscWrongDone = false;
            SetColor(tubeConfig.colorWrong);
            SetColorText(tubeConfig.textWrong);
            SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, dragResultConfig.sfxWrong);
            ObserverManager.TriggerEvent<SoundChannel>(soundData);
            BEPS01RFSHandleData.SetAnimation(skeleton, skeletonConfig.tubeWrong, false, (trackEntry) => {
                tscWrongDone = true;
                BEPS01RFSHandleData.SetAnimation(skeleton, skeletonConfig.tubeNormal, false, null);
            });
            yield return new WaitUntil(() => tscWrongDone);
            yield return new WaitForSeconds(BEPS01RFSHandleData.MilisecondsToSeconds(dragResultConfig.timeDelay / 2));
            SetColor(tubeConfig.colorNormal);
            SetColorText(tubeConfig.textNormal);
            StartCoroutine(OnResetTube());
        }
    }
}