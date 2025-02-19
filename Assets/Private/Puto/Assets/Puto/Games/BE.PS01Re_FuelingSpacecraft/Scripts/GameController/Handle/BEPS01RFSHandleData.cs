using MonkeyBase.Observer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSHandleData
    {
        public static float MAX_SIZE_LAYOUT = 1998f;
        public static float SIZE_SHORT = 222f;
        public static float SIZE_LONG = 444f;
        public const int COMPARISON_INDEX = 5;
        public const int TWO_TUBES = 2;
        public const int THREE_TUBES = 3;
        public const float MAX_DISTANCE = 15f;
        public const float MIN_TIME = 0.1f;
        public const float MAX_TIME = 1f;
        public const string COLOR_SYNC= "<color=#F4A300>";
        public const string EVENT_SPACESHIP_FLY_1 = "Spaceship Fly 1";
        public const string EVENT_SPACESHIP_FLY_2 = "Spaceship Fly 2";
        public const string EVENT_SPACESHIP_FLY_3 = "Spaceship Fly 3";
        public const string EVENT_SPACESHIP_ON = "Spaceship On";
        public const string EVENT_SPACESHIP_OFF = "Spaceship Off";
        public const string EVENT_IMPACT = "Impact";
        public const string EVENT_MAGIC = "Magic";
        public const string EVENT_CHEER = "Cheer";
        public const int SORTING_ORDER_DRAG = 5;

        public static void TriggerFinishState(BEPS01RFSState state, object data)
        {
            BEPS01RFSDataChanner dataChanner = new BEPS01RFSDataChanner(state, data);
            ObserverManager.TriggerEvent(dataChanner);
        }

        public static void TriggerStateInput(BEPS01RFSUserInput input, object data)
        {
            BEPS01RFSInputChanner buttonData = new BEPS01RFSInputChanner(input, data);
            ObserverManager.TriggerEvent(buttonData);
        }

        public static GameObject SpawnItem(GameObject itemPrefab, GameObject parent)
        {
            return GameObject.Instantiate(itemPrefab, parent.transform, false);
        }
        public static void SetAnimation(SkeletonGraphic animation, string input, bool isLoop, Spine.AnimationState.TrackEntryDelegate onCompleteCallback)
        {
            animation.AnimationState.SetAnimation(0, input, isLoop).Complete += onCompleteCallback;
        }
        public static void EnableTubes(List<BEPS01RFSTubeItem> list, bool isEnable)
        {
            foreach (var item in list)
            {
                item.Enable(isEnable);
            }
        }
        public static void EnableTubes(List<BEPS01RFSTubeItem> list, int idTube, bool isEnable)
        {
            foreach (var item in list)
            {
                if (!AreIntegersEqual(item.GetData().id, idTube)) item.Enable(isEnable);
            }
        }
        public static void SetDragTube(List<BEPS01RFSTubeItem> list, bool isDrag)
        {
            foreach (var item in list)
            {
                item.SetDragObject(isDrag);
            }
        }
        public static List<T> RandomSelectItems<T>(List<T> itemList, int maxItem)
        {
            List<T> selectedItems = new List<T>();

            if (itemList.Count <= maxItem)
            {
                return itemList;
            }

            List<int> selectedIndices = new List<int>();

            while (selectedIndices.Count < maxItem)
            {
                int randomIndex = UnityEngine.Random.Range(0, itemList.Count);

                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                    selectedItems.Add(itemList[randomIndex]);
                }
            }
            return selectedItems;
        }
        public static bool AreIntegersEqual(int a, int b)
        {
            return a == b;
        }
        public static void EnableLayoutGroup(HorizontalLayoutGroup layoutGroup, bool value)
        {
            layoutGroup.enabled = value;
            layoutGroup.GetComponent<ContentSizeFitter>().enabled = value;
        }
        public static void SetTextSizesEqualMin(List<TMP_Text> textList)
        {
            float minSize = float.MaxValue;

            foreach (TMP_Text text in textList)
            {
                text.ForceMeshUpdate();
                minSize = Mathf.Min(minSize, text.fontSize);
            }
            foreach (TMP_Text text in textList)
            {
                text.enableAutoSizing = false;
                text.fontSize = minSize;
            }
        }
        public static void DestroyItem(Transform parent)
        {
            foreach (Transform child in parent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        public static float MilisecondsToSeconds(int value)
        {
            return (float)value / 1000;
        }
        public static List<Transform> GetRandomPoints(List<Transform> pointsRandom, int tubeItemCount)
        {
            List<Transform> selectedPoints = new List<Transform>();

            List<Transform> firstGroup = pointsRandom.GetRange(0, 3);
            List<Transform> secondGroup = pointsRandom.GetRange(3, 4);

            Transform firstRandom = firstGroup[UnityEngine.Random.Range(0, firstGroup.Count)];
            selectedPoints.Add(firstRandom);

            Transform secondRandom = secondGroup[UnityEngine.Random.Range(0, secondGroup.Count)];
            selectedPoints.Add(secondRandom);

            List<Transform> remainingPoints = new List<Transform>(pointsRandom);
            remainingPoints.Remove(firstRandom);
            remainingPoints.Remove(secondRandom);

            int remainingCount = tubeItemCount - selectedPoints.Count;
            for (int i = 0; i < remainingCount; i++)
            {
                Transform randomPoint = remainingPoints[UnityEngine.Random.Range(0, remainingPoints.Count)];
                selectedPoints.Add(randomPoint);
                remainingPoints.Remove(randomPoint);
            }

            return selectedPoints;
        }
        public static bool CheckTriggerOfTwoObject(RectTransform objectA, RectTransform objectB, float percent)
        {
            Vector3[] objectACorners = new Vector3[4];
            Vector3[] objectBCorners = new Vector3[4];

            objectA.GetWorldCorners(objectACorners);
            objectB.GetWorldCorners(objectBCorners);

            Rect rectA = new(objectACorners[0].x, objectACorners[0].y, objectACorners[2].x - objectACorners[0].x, objectACorners[2].y - objectACorners[0].y);

            Rect rectB = new(objectBCorners[0].x, objectBCorners[0].y, objectBCorners[2].x - objectBCorners[0].x, objectBCorners[2].y - objectBCorners[0].y);

            Rect intersection = Rect.MinMaxRect(
                Mathf.Max(rectA.xMin, rectB.xMin),
                Mathf.Max(rectA.yMin, rectB.yMin),
                Mathf.Min(rectA.xMax, rectB.xMax),
                Mathf.Min(rectA.yMax, rectB.yMax)
            );

            float intersectionArea = Mathf.Max(0, intersection.width) * Mathf.Max(0, intersection.height);
            float rectAArea = rectA.width * rectA.height;
            float rectBArea = rectB.width * rectB.height;

            float overlapPercentageA = (intersectionArea / rectAArea) * 100f;
            float overlapPercentageB = (intersectionArea / rectBArea) * 100f;

            return overlapPercentageA >= percent * 100f && overlapPercentageB >= percent * 100f;
        }


    }
}
