
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJAdapter : Adapter
    {
        [SerializeField] private BEID01IntroDialogMJMockDataScriptableObject mockDataScriptableObject;
        [SerializeField] private bool isMockData;
        private BEID01IntroDialogMJGamePlayData gameData;
        public override T GetData<T>(int turn)
        {
            T data;
            if (isMockData)
            {
                gameData = new();
                gameData.listData = new List<BEID01IntroDialogMJConversationData>();
                gameData.listData = mockDataScriptableObject.mockData.listData;
            }
            Type listType = typeof(T);

            if (listType == typeof(BEID01IntroDialogMJInitStateData))
            {
                BEID01IntroDialogMJInitStateData initStateData = new BEID01IntroDialogMJInitStateData();
                initStateData.ListData = gameData.listData;
                initStateData.Background = gameData.background;
                initStateData.CharacterName = gameData.characterName;
                initStateData.CharacterAsset = gameData.characterAsset;
                data = ConvertToType<T>(initStateData);
            }
            else if (listType == typeof(BEID01IntroDialogMJIntroStateData))
            {
                BEID01IntroDialogMJIntroStateData introStateData = new BEID01IntroDialogMJIntroStateData();
                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEID01IntroDialogMJPlayStateData))
            {
                BEID01IntroDialogMJPlayStateData playStateData = new BEID01IntroDialogMJPlayStateData();
                data = ConvertToType<T>(playStateData);
            }
            else if (listType == typeof(BEID01IntroDialogMJListenAgainStateData))
            {
                BEID01IntroDialogMJListenAgainStateData listenAgainStateData = new BEID01IntroDialogMJListenAgainStateData();
                data = ConvertToType<T>(listenAgainStateData);
            }
            else if (listType == typeof(BEID01IntroDialogMJEndGameStateData))
            {
                BEID01IntroDialogMJEndGameStateData endGameStateData = new BEID01IntroDialogMJEndGameStateData();
                endGameStateData.WordList = new List<UserEndGameData.Word>();
                endGameStateData.WordList.Add(gameData.wordDataBackground); 
                endGameStateData.WordList.Add(gameData.wordDataCharacter); 
                foreach (var itemW in gameData.listData)
                {
                    endGameStateData.WordList.Add(itemW.wordData);
                }

                data = ConvertToType<T>(endGameStateData);
            }
            
            else
            {
                data = ConvertToType<T>(null);
            }

            return data;
        }

        public override int GetMaxTurn()
        {
            return 1;
        }

        public override void SetData<T>(T data)
        {
            DataGameModel dataGameModel = ConvertToType<DataGameModel>(data);
            if (dataGameModel == null) LogMe.Log("Lucanhtai dataGameModel IntroDialogMJ == null");
            BEID01IntroDialogMJConfigGameData configDataGame = JsonUtility.FromJson<BEID01IntroDialogMJConfigGameData>(dataGameModel.JsonConfig);
            if (dataGameModel == null) LogMe.Log("Lucanhtai configDataGame IntroDialogMJ == null");
            LogMe.Log(dataGameModel.JsonConfig);
            List<BEID01IntroDialogMJModelData> listData = configDataGame.data.ToList();
            listData = listData.OrderBy(item => item.order).ToList();

            BEID01IntroDialogMJGamePlayData gamePlayData = new BEID01IntroDialogMJGamePlayData();
            gamePlayData.listData = new List<BEID01IntroDialogMJConversationData>();
            gamePlayData.background = dataGameModel.DataGamePrimitiveDict[configDataGame.background].ImageDataGameModelsList[0].Sprite;
            gamePlayData.wordDataBackground = new UserEndGameData.Word();
            gamePlayData.wordDataBackground.TextID = dataGameModel.DataGamePrimitiveDict[configDataGame.background].TextID;
            gamePlayData.wordDataBackground.Type = dataGameModel.DataGamePrimitiveDict[configDataGame.background].Type;

            gamePlayData.characterName = dataGameModel.DataGamePrimitiveDict[configDataGame.character].Text;
            gamePlayData.characterAsset = dataGameModel.DataGamePrimitiveDict[configDataGame.character].AnimationDataGameModelsList[0].SkeletonDataAsset;
            gamePlayData.wordDataCharacter = new UserEndGameData.Word();
            gamePlayData.wordDataCharacter.TextID = dataGameModel.DataGamePrimitiveDict[configDataGame.character].TextID;
            gamePlayData.wordDataCharacter.Type = dataGameModel.DataGamePrimitiveDict[configDataGame.character].Type;

            for (int turnData = 0; turnData < listData.Count; turnData++)
            {
                BEID01IntroDialogMJModelData modelData = listData[turnData];
                /*LogMe.LogError("question_data: "+ dataGameModel.DataGamePrimitiveDict[modelData.question_data].Text);
                LogMe.LogError("answer_w: " + dataGameModel.DataGamePrimitiveDict[modelData.answer_w].Text);*/

                if (modelData.question_data != 0)
                {
                    BEID01IntroDialogMJConversationData itemQuestionData = new BEID01IntroDialogMJConversationData();
                    itemQuestionData.wordData = new UserEndGameData.Word();
                    itemQuestionData.text = dataGameModel.DataGamePrimitiveDict[modelData.question_data].Text;
                    itemQuestionData.wordData.TextID = dataGameModel.DataGamePrimitiveDict[modelData.question_data].TextID;
                    itemQuestionData.wordData.Type = dataGameModel.DataGamePrimitiveDict[modelData.question_data].Type;
                    itemQuestionData.audioClip = dataGameModel.DataGamePrimitiveDict[modelData.question_data].AudioDataGameModelsList[0].AudioClip;
                    gamePlayData.listData.Add(itemQuestionData);
                }

                if (modelData.answer_w != 0)
                {
                    BEID01IntroDialogMJConversationData itemAnsweData = new BEID01IntroDialogMJConversationData();
                    itemAnsweData.wordData = new UserEndGameData.Word();
                    itemAnsweData.text = dataGameModel.DataGamePrimitiveDict[modelData.answer_w].Text;
                    itemAnsweData.wordData.TextID = dataGameModel.DataGamePrimitiveDict[modelData.answer_w].TextID;
                    itemAnsweData.wordData.Type = dataGameModel.DataGamePrimitiveDict[modelData.answer_w].Type;

                    itemAnsweData.audioClip = dataGameModel.DataGamePrimitiveDict[modelData.answer_w].AudioDataGameModelsList[0].AudioClip;
                    gamePlayData.listData.Add(itemAnsweData);
                }

            }

            this.gameData = gamePlayData;
        }
    }

}