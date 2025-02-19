using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02HandleData
    {
        public const string COLOR_YELLOW = "<color=#ffe000>";
        public const int COMPARISON_INDEX = 5;
        public const int TWO_OWLS = 2;
        public const int THREE_OWLS = 3;
        public const int MAX_CHARACTERS = 11;
        public static bool IsPlayingEffect { get; set; } = false;
        public static int DragCorrectCount { get; set; } = 0;
        public static int DragWrongCount { get; set; } = 0;
        public static int IndDragWrong { get; set; } = 0;
        public static int IndDragScreen { get; set; } = 0;


        public static void TriggerFinishState(BEPS02FlyingOwlsState state, object data)
        {
            BEPS02FlyingOwlsDataChanner dataChanner = new BEPS02FlyingOwlsDataChanner(state, data);
            ObserverManager.TriggerEvent(dataChanner);
        }

        public static void TriggerStateDrag(BEPS02FlyingOwlsUserInput input, object data)
        {
            BEPS02FlyingOwlsInputChanner buttonData = new BEPS02FlyingOwlsInputChanner(input, data);
            ObserverManager.TriggerEvent(buttonData);
        }

        public static List<RectTransform> GetListChildRect(GameObject gameObject)
        {
            RectTransform[] allRectTransforms = gameObject.GetComponentsInChildren<RectTransform>(false);
            List<RectTransform> childRectTransforms = new List<RectTransform>();

            foreach (RectTransform rectTransform in allRectTransforms)
            {
                if (rectTransform != gameObject.GetComponent<RectTransform>())
                {
                    childRectTransforms.Add(rectTransform);
                }
            }

            return childRectTransforms.ToList();
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

        public static void SetAlphaImage(UnityEngine.UI.Image image, float alpha)
        {
            Color tempColor = image.color;
            tempColor.a = alpha;
            image.color = tempColor;
        }

        public static BaseImage GetShadowOwlByOwl(List<BaseImage> listShadowOwl, string nameOwl)
        {
            int indexShadow = GetIndexByName(nameOwl);
            for (int i = 0; i < listShadowOwl.Count; i++)
            {
                if (GetIndexByName(listShadowOwl[i].name) == indexShadow)
                {
                    return listShadowOwl[i];
                }
            }
            return null;
        }
      
        public static int GetIndexByName(string itemName)
        {
            string[] objectItem = itemName.Split("_");
            return int.Parse(objectItem[1]);
        }

        public static void EnableOwls(List<BEPS02OwlDrag> list, bool isEnable)
        {
            foreach (var item in list)
            {
                if (!item.IsPlaying()) item.Enable(isEnable);
            }
        }
        public static void StopCoroutinesOwls(List<BEPS02OwlDrag> list)
        {
            foreach (var item in list)
            {
                item.StopAllCoroutines();
            }
        }
        public static void FadeShadowOwls(List<BaseImage> listShadowOwl)
        {
            foreach (var item in listShadowOwl)
            {
                if (item.GetComponent<Image>().color.a > 0)
                {
                    item.GetComponent<Image>().DOFade(0, 0.2f).SetEase(Ease.Linear);
                }
            }
        }

        public static void EnableOwls(List<BEPS02OwlDrag> list, string name, bool isEnable)
        {
            foreach (var item in list)
            {
               if(!item.name.Equals(name)) item.Enable(isEnable);
            }
        }

        public static int GetIndexCurrentAnimation(SkeletonGraphic skeletonGraphic)
        {
            TrackEntry currentTrackEntry = skeletonGraphic.AnimationState.GetCurrent(0);
            return ExtractNumber(currentTrackEntry.Animation.Name);
        }

        public static void SetNumberAnimation(SkeletonGraphic owlAnimation, string input, int newNumber, bool isLoop, Spine.AnimationState.TrackEntryDelegate onCompleteCallback)
        {
            string pattern = @"(\d)";
            string replacement = newNumber.ToString();
            string result = Regex.Replace(input, pattern, replacement);
            owlAnimation.AnimationState.SetAnimation(0, result, isLoop).Complete += onCompleteCallback;
        }

        public static int ExtractNumber(string input)
        {
            string pattern = @"(\d+)";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                if (int.TryParse(match.Value, out int result))
                {
                    return result;
                }
            }
            return 0;
        }

        public static List<int> GetIndexesByList(List<BEPS02OwlDrag> items)
        {
            List<int> list = new();
            foreach (var item in items)
            {
                list.Add(GetIndexByName(item.name));
            }
            return list;
        }

        public static List<int> GetIndexesByList(List<BaseImage> items)
        {
            List<int> list = new();
            foreach (var item in items)
            {
                list.Add(GetIndexByName(item.name));
            }
            return list;
        }

        public static async void PlayEffectOwl(AudioClip audio, CancellationToken token)
        {
            bool tscSfxFly = false;
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    SoundChannel soundData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, audio, () => { tscSfxFly = true; });
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.WaitUntil(() => tscSfxFly, cancellationToken: token);
                    tscSfxFly = false;
                }
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }

        public static List<T> SortListByIndex<T>(List<T> listItem, List<int> indexList)
        {
            return listItem
                 .Select((item, index) => new { Item = item, Index = indexList[index] })
                 .OrderBy(x => x.Index)
                 .Select(x => x.Item)
                 .ToList();
        }

        public static bool AreIntegersEqual(int a, int b)
        {
            return a == b;
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
