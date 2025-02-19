using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSSharkPooling : MonoBehaviour, EventListener<BERV01FTSFishChanner>
    {
        [SerializeField] private GameObject sharkObject;
        [SerializeField] private Camera cameraGame;
        [SerializeField] private Transform pointLeft;
        [SerializeField] private Transform pointRight;

        private Queue<GameObject> sharksPool = new Queue<GameObject>();
        private List<GameObject> listAllShark;
        private float sharkSpeedIn = 50f;
        private float sharkSpeedOut = 20f;
        private List<BERV01FTSFish> fishesWait;
        private List<BERV01FTSFish> fishesPlay;
        private List<Image> correctFishesPoint;
        private AudioClip sfxChomp;
        private CancellationTokenSource cts;
        private bool isSendEventOutro = false;
        private int indexChangeFish = 0;

        public List<GameObject> ListAllShark
        {
            get { return listAllShark; }
        }

        private void Awake()
        {
            cts = new CancellationTokenSource();
            listAllShark = new List<GameObject>();
            for (int i = 0; i < BERV01FTSHandleData.MAX_SHARK_SPAWN; i++)
            {
                GameObject obj = Instantiate(sharkObject, transform);
                obj.SetActive(false);
                listAllShark.Add(obj);
                sharksPool.Enqueue(obj);
            }
        }
        public void InitData(List<BERV01FTSFish> fishesPlay, List<BERV01FTSFish> fishesWait, List<Image> correctFishesPoint, AudioClip sfxChomp)
        {
            this.fishesPlay = fishesPlay;
            this.fishesWait = fishesWait;
            this.correctFishesPoint = correctFishesPoint;
            this.sfxChomp = sfxChomp;
        }

        public GameObject GetFromPool()
        {
            if (sharksPool.Count > 0)
            {
                GameObject obj = sharksPool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject obj = Instantiate(sharkObject);
                return obj;
            }
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            sharksPool.Enqueue(obj);
        }
        private void SpawnNextFishFromCorrect(BERV01FTSFish fishObject)
        {
            fishesPlay.Remove(fishObject);
            fishesWait.Shuffle();
            BERV01FTSFish getFishWrong = fishesWait.Find((fish) => !fish.FishData.isCorrect);
            getFishWrong.gameObject.SetActive(true);
            fishesPlay.Add(getFishWrong);
            getFishWrong.SwimmingLane = fishObject.SwimmingLane;
            getFishWrong.FishDirection = fishObject.FishDirection;
            getFishWrong.IsEnable = true;
            BERV01FTSHandleData.SetSkin(getFishWrong.SkeletonFish, BERV01FTSHandleData.GetSkin(fishesPlay));
            getFishWrong.EnableMovingFish(true);
            fishesWait.Remove(getFishWrong);
        }
        private void ChangeFishWrongToCorrect(BERV01FTSFish fishChange)
        {
            if(indexChangeFish > 0) indexChangeFish--;
            fishesPlay.Remove(fishChange);
            fishesWait.Shuffle();
            fishChange.DOKill();
            BERV01FTSFish getFishCorrect= fishesWait.Find((fish) => fish.FishData.isCorrect);
            getFishCorrect.gameObject.SetActive(true);
            fishesPlay.Add(getFishCorrect);
            getFishCorrect.SwimmingLane = fishChange.SwimmingLane;
            getFishCorrect.FishDirection = fishChange.FishDirection;
            getFishCorrect.IsEnable = true;
            BERV01FTSHandleData.SetSkin(getFishCorrect.SkeletonFish, BERV01FTSHandleData.GetSkin(fishesPlay));
            getFishCorrect.EnableMovingFish(true);
            fishChange.gameObject.SetActive(false);
            fishesWait.Remove(getFishCorrect);
        }

        public void OnMMEvent(BERV01FTSFishChanner eventType)
        {
            switch (eventType.UserInput)
            {
                case BERV01FishState.SharkBite:
                    BERV01FTSFish fishObject = (BERV01FTSFish)eventType.Data;
                    BERV01FTSHandleData.SetPivot(fishObject.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f));
                    indexChangeFish++;
                    GameObject sharkObject = GetFromPool(); 
                    sharkObject.transform.SetAsLastSibling();
                    bool isCloserToLeft = BERV01FTSHandleData.IsCloserToLeftEdge(cameraGame, fishObject.gameObject);

                    Transform transFish = isCloserToLeft ? pointRight.transform : pointLeft.transform;
                    sharkObject.transform.localRotation = new Quaternion(0f, isCloserToLeft ? 0f : 180f, 0f, 0f);
                    sharkObject.transform.position = new Vector3(transFish.position.x, fishObject.transform.position.y, sharkObject.transform.position.z);

                    SkeletonGraphic skeletonShark = sharkObject.GetComponent<SkeletonGraphic>();
                    BERV01FTSHandleData.SetAnimation(skeletonShark, BERV01FTSHandleData.sharkIdle, true, null);
                    sharkObject.transform.DOMoveX(fishObject.transform.position.x, sharkSpeedIn)
                    .SetEase(Ease.OutQuad).SetSpeedBased()
                    .OnComplete(async () => {
                        SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, sfxChomp);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        BERV01FTSHandleData.SetAnimation(skeletonShark, BERV01FTSHandleData.sharkBite, false, (trackEntry) => {
                            BERV01FTSHandleData.SetAnimation(skeletonShark, BERV01FTSHandleData.sharkIdle, true, null);
                            Transform pointMove = (isCloserToLeft) ? pointLeft : pointRight;
                            sharkObject.transform.DOMoveX(pointMove.position.x, sharkSpeedOut)
                               .SetEase(Ease.Linear).SetSpeedBased()
                               .OnComplete(() => {
                                   ReturnToPool(sharkObject);
                                   bool hasActiveObject = listAllShark.Any(go => go.activeSelf);
                                   if (BERV01FTSHandleData.CurrentIndexFishCorrect >= BERV01FTSHandleData.MAX_FISH_CORRECT && !isSendEventOutro && !hasActiveObject)
                                   {
                                       BERV01FTSHandleData.TriggerFinishState(BERV01State.OutroGame, null);
                                       isSendEventOutro = true;
                                   } else
                                   {
                                       BERV01FTSHandleData.TriggerFinishState(BERV01State.PlayGame, null);
                                   }
                               });
                        });
                        try
                        {
                            await UniTask.Delay(400, cancellationToken: cts.Token);
                            if(BERV01FTSHandleData.CurrentIndexFishCorrect < BERV01FTSHandleData.MAX_FISH_CORRECT)
                            {
                                correctFishesPoint[BERV01FTSHandleData.CurrentIndexFishCorrect].transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
                                BERV01FTSHandleData.CurrentIndexFishCorrect++;
                            }
                            SpawnNextFishFromCorrect(fishObject);
                            fishObject.gameObject.SetActive(false);
                        }
                        catch (OperationCanceledException ex)
                        {
                            LogMe.Log("Lucanhtai ex: " + ex);
                        }
                    });

                    break;
                case BERV01FishState.SwimToPlace:
                    BERV01FTSFish fishChange = (BERV01FTSFish)eventType.Data;
                    if(indexChangeFish > 0)
                    {
                        ChangeFishWrongToCorrect(fishChange);
                    }
                    break;
            }
        }

        private void OnEnable()
        {
            this.ObserverStartListening<BERV01FTSFishChanner>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BERV01FTSFishChanner>();
        }
        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}
