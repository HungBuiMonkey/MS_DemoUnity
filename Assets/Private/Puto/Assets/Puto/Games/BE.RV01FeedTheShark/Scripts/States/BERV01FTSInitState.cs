using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSInitState : FSMState
    {
        private BERV01FTSInitStateObjectDependency dependency;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BERV01FTSInitStateData initStateData = (BERV01FTSInitStateData)data;
            SetData(initStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BERV01FTSInitStateObjectDependency)data;
        }
        private void SetData(BERV01FTSInitStateData initStateData)
        {
            BERV01FTSHandleData.CurrentIndexFishCorrect = 0;

            dependency.SmallFishes.Shuffle();
            dependency.BigFishes.Shuffle();

            List<BERV01FTSFish> listAllFish = new List<BERV01FTSFish>();

            int smallFishIndex = 0; 
            int bigFishIndex = 0;   

            for (int i = 0; i < initStateData.ListData.Count; i++)
            {
                BERV01FTSFishData fishData = initStateData.ListData[i];

                if (fishData.text.Length <= 5)
                {
                    BERV01FTSFish fish = GameObject.Instantiate(dependency.SmallFishes[smallFishIndex], dependency.UIFish);
                    fish.InitData(fishData, dependency.ClickConfig);
                    listAllFish.Add(fish);

                    smallFishIndex = (smallFishIndex + 1) % dependency.SmallFishes.Count;
                }
                else
                {
                    BERV01FTSFish fish = GameObject.Instantiate(dependency.BigFishes[bigFishIndex], dependency.UIFish);
                    fish.InitData(fishData, dependency.ClickConfig);
                    listAllFish.Add(fish);

                    bigFishIndex = (bigFishIndex + 1) % dependency.BigFishes.Count;
                }
            }

            listAllFish.Shuffle();

            int indFishCorrect = 0;
            int indFishWrong = 0;
            for (int i = 0; i < listAllFish.Count; i++)
            {
                listAllFish[i].name = $"{listAllFish[i].name}_{listAllFish[i].FishData.text}_{listAllFish[i].FishData.isCorrect.ToString()}";
                if (listAllFish[i].FishData.isCorrect && indFishCorrect < BERV01FTSHandleData.MAX_FISH_PLAY / 2)
                {
                    indFishCorrect++;
                    dependency.FishesPlay.Add(listAllFish[i]);
                } else if(!listAllFish[i].FishData.isCorrect && indFishWrong < BERV01FTSHandleData.MAX_FISH_PLAY / 2)
                {
                    indFishWrong++;
                    dependency.FishesPlay.Add(listAllFish[i]);
                }   else
                {
                    listAllFish[i].gameObject.SetActive(false);
                    dependency.FishesWait.Add(listAllFish[i]);
                }
            }

            AssignFishDirections();
            dependency.SharkPooling.InitData(dependency.FishesPlay, dependency.FishesWait, dependency.CorrectFishesPoint, dependency.ClickConfig.sfxChomp);
            BERV01FTSHandleData.TriggerFinishState(BERV01State.IntroGame, null);
        }
        private void AssignFishDirections()
        {
            BERV01FishDirection initialDirection = (Random.Range(0, 2) == 0)
                ? BERV01FishDirection.Right
                : BERV01FishDirection.Left;
            BERV01FTSHandleData.SkinsFish.Shuffle();
            for (int i = 0; i < dependency.FishesPlay.Count; i++)
            {
                int indexSkin = i % 3;
                dependency.LaneFishes[i].Index = i;
                BERV01FTSFish fish = dependency.FishesPlay[i];
                BERV01FTSHandleData.SetSkin(fish.SkeletonFish, BERV01FTSHandleData.SkinsFish[indexSkin]);
                fish.SwimmingLane = dependency.LaneFishes[i];
                fish.FishDirection = (i % 2 == 0)
                    ? initialDirection
                    : (initialDirection == BERV01FishDirection.Right
                        ? BERV01FishDirection.Left
                        : BERV01FishDirection.Right);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
    public class BERV01FTSInitStateData
    {
        public List<BERV01FTSFishData> ListData { get; set; }
    }


    public class BERV01FTSInitStateObjectDependency
    {
        public BERV01FTSClickConfig ClickConfig { get; set; }
        public BERV01FTSSharkPooling SharkPooling { get; set; }
        public List<BERV01FTSFish> SmallFishes { get; set; }
        public List<BERV01FTSFish> BigFishes { get; set; }
        public List<BERV01FTSFish> FishesPlay { get; set; }
        public List<BERV01FTSFish> FishesWait { get; set; }
        public List<BERV01FTSFishSwimmingLane> LaneFishes { get; set; }
        public List<Image> CorrectFishesPoint { get; set; }
        public Transform UIFish { get; set; }

    }
}