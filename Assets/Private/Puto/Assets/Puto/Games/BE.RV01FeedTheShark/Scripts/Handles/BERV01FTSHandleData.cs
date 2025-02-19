using MonkeyBase.Observer;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSHandleData
    {
        public static string[] SkinsFish = { "than hong", "than vang", "than xanh" };
        public const string fishIdle = "Idie";
        public const string fishUserTap = "user tap";
        public const string fishUserUnTap = "user Untap";
        public const string fishUserUnTapLoop = "user Untap loop";
        public const string stunned = "star fly";

        public const string sharkIdle = "Idie";
        public const string sharkBite = "shark bite";
        public const string sharkHappy = "shark happy";

        public const int MAX_FISH_CORRECT = 5;
        public const int MAX_FISH_WRONG = 7;
        public const int MAX_SHARK_SPAWN = 2;
        public const int MAX_FISH_PLAY = 4;
        public const float SHORT_LANE_FISH_X = 450f;
        public const float LONG_LANE_FISH_X = 550f;

        public static int CurrentIndexFishCorrect { get; set; } = 0;


        public static void TriggerFinishState(BERV01State state, object data)
        {
            BERV01FTSDataChanner dataChanner = new BERV01FTSDataChanner(state, data);
            ObserverManager.TriggerEvent(dataChanner);
        }

        public static void TriggerStateInput(BERV01UserInput input, object data)
        {
            BERV01FTSInputChanner buttonData = new BERV01FTSInputChanner(input, data);
            ObserverManager.TriggerEvent(buttonData);
        }
        public static void TriggerStateFish(BERV01FishState state, object data)
        {
            BERV01FTSFishChanner fishData = new BERV01FTSFishChanner(state, data);
            ObserverManager.TriggerEvent(fishData);
        }

        public static void SetAnimation(SkeletonGraphic animation, string input, bool isLoop, Spine.AnimationState.TrackEntryDelegate onCompleteCallback)
        {
            animation.AnimationState.SetAnimation(0, input, isLoop).Complete += onCompleteCallback;
        }
        public static void SetPivot(RectTransform rectTransform, Vector2 newPivot)
        {
            Vector3 deltaPosition = rectTransform.localPosition;

            // Tính delta của pivot
            Vector2 pivotDelta = newPivot - rectTransform.pivot;

            // Kích thước của RectTransform
            Vector3 sizeDelta = rectTransform.rect.size;

            // Điều chỉnh deltaPosition theo hướng xoay (lật khi y = 180)
            bool isFlipped = Mathf.Abs(rectTransform.localRotation.eulerAngles.y - 180) < 0.1f; 
            deltaPosition.x += pivotDelta.x * sizeDelta.x * rectTransform.localScale.x * (isFlipped ? -1 : 1);
            deltaPosition.y += pivotDelta.y * sizeDelta.y * rectTransform.localScale.y;

            // Gán lại pivot và vị trí mới
            rectTransform.pivot = newPivot;
            rectTransform.localPosition = deltaPosition;
        }
        public static float GetRandomFloatWithStep(float min, float max, float step)
        {
            int steps = Mathf.FloorToInt((max - min) / step) + 1;

            int randomStepIndex = Random.Range(0, steps);

            return min + randomStepIndex * step;
        }

        public static bool IsCloserToLeftEdge(Camera camera, GameObject gameObject)
        {
            Vector3 screenPosition = camera.WorldToScreenPoint(gameObject.transform.position);

            float screenWidth = Screen.width;

            float distanceToLeft = screenPosition.x;            
            float distanceToRight = screenWidth - screenPosition.x;

            return distanceToLeft < distanceToRight;
        }

        public static bool IsClosestToCenter(GameObject obj, Camera cameraGame)
        {

            Vector3 screenPosition = cameraGame.WorldToScreenPoint(obj.transform.position);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if (screenPosition.x <= 0 || screenPosition.x >= screenWidth) return false;

            Vector2 screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

            float distanceToCenter = Vector2.Distance(screenCenter, new Vector2(screenPosition.x, screenPosition.y));

            float maxAcceptableDistance = Mathf.Min(screenWidth, screenHeight) / 3f;

            return distanceToCenter <= maxAcceptableDistance;
        }

        public static void SetSkin(SkeletonGraphic skeletonGraphic, string skinName)
        {
            var skeleton = skeletonGraphic.Skeleton;
            var skin = skeleton.Data.FindSkin(skinName);
            if (skin == null) return;
            skeleton.SetSkin(skin);
            skeleton.SetToSetupPose();
            skeletonGraphic.AnimationState.Apply(skeleton);
        }
        public static string GetCurrentSkinName(SkeletonGraphic skeletonGraphic)
        {
            var currentSkin = skeletonGraphic.Skeleton.Skin;
            return currentSkin != null ? currentSkin.Name : "";
        }

        private static string CheckMissingAndRandomSkin(List<string> listSkins)
        {
            var missingSkins = SkinsFish.Except(listSkins).ToList();

            if (missingSkins.Count > 0)
            {
                return missingSkins.First();
            }
            else
            {
                string randomSkin = SkinsFish[Random.Range(0, SkinsFish.Length)];
                return randomSkin;
            }
        }

         public static string GetSkin(List<BERV01FTSFish> fishesPlay)
        {
            List<string> listSkins = new List<string>();
            foreach (var fish in fishesPlay)
            {
                string skinName = BERV01FTSHandleData.GetCurrentSkinName(fish.SkeletonFish);
                if (!string.IsNullOrEmpty(skinName)) listSkins.Add(skinName);
            }
            string newSkin = CheckMissingAndRandomSkin(listSkins);
            return newSkin;
        }

    }
}