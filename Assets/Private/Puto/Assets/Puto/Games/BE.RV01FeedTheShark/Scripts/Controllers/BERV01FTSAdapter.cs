using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSAdapter : Adapter
    {

        [SerializeField] private BERV01FTSMookData mookDataSO;
        [SerializeField] private bool isMockData;
        private BERV01FTSGamePlayData gamePlayData;

        public override T GetData<T>(int turn)
        {
            T data;
            if (isMockData)
            {
                gamePlayData = new BERV01FTSGamePlayData();
                gamePlayData = mookDataSO.mookData;
                gamePlayData.listData.Shuffle();
            }

            Type listType = typeof(T);
            if (listType == typeof(BERV01FTSInitStateData))
            {
                BERV01FTSInitStateData initStateData = new BERV01FTSInitStateData();
                initStateData.ListData = gamePlayData.listData;
                data = ConvertToType<T>(initStateData);
            }
            else if (listType == typeof(BERV01FTSIntroStateData))
            {
                BERV01FTSIntroStateData introStateData = new BERV01FTSIntroStateData();
                introStateData.AudioData = gamePlayData.audioMain;
                introStateData.TypeData = gamePlayData.wordDataMain.Type;
                data = ConvertToType<T>(introStateData);
            }
            else
            {
                data = ConvertToType<T>(null);
            }
            return data;
        }

        public override int GetMaxTurn()
        {
            return gamePlayData.listData.Count;
        }

        public override void SetData<T>(T data)
        { 

        }
    }

    //Data play
    [Serializable]
    public class BERV01FTSGamePlayData
    {
        public UserEndGameData.Word wordDataMain;
        public AudioClip audioMain;
        public List<BERV01FTSFishData> listData;
    }

    [Serializable]
    public class BERV01FTSFishData
    {
        public UserEndGameData.Word wordDataFish;
        public string text;
        public AudioClip audio;
        public bool isCorrect;
    }

}