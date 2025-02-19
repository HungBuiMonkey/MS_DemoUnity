using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSAdapter : Adapter
    {
        //[SerializeField] private BEPS01RFSMookData mookDataSO;
//[SerializeField] private BEPS01RFSTestPlayData testPlayData;
        //[SerializeField] private bool isMockData;
        private BEPS01RFSGamePlayData gamePlayData;
        private const string KEY_LEVER_HARD = "hard";
        private string pattern = @"([.!?;:])";

        public override T GetData<T>(int turn)
        {
            T data;
            /*
            if (isMockData)
            {
                gamePlayData = new BEPS01RFSGamePlayData();
                gamePlayData.dataPlay = new List<BEPS01RFSConversationData>();
                gamePlayData.dataPlay = mookDataSO.listData;
            }
            */
            Type listType = typeof(T);
            if (listType == typeof(BEPS01RFSInitStateData))
            {
                BEPS01RFSInitStateData initStateData = new BEPS01RFSInitStateData();
                initStateData.DataTurn = gamePlayData.dataPlay[turn];
                data = ConvertToType<T>(initStateData);
            }
            else if(listType == typeof(BEPS01RFSIntroStateData))
            {
                BEPS01RFSIntroStateData introStateData = new BEPS01RFSIntroStateData();
                introStateData.AudioSentence = gamePlayData.dataPlay[turn].audioSentence;
                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEPS01RFSNextTurnStateData))
            {
                BEPS01RFSNextTurnStateData nextTurnStateData = new BEPS01RFSNextTurnStateData();
                nextTurnStateData.AudioSentence = gamePlayData.dataPlay[turn].audioSentence;
                nextTurnStateData.SyncDatas = gamePlayData.dataPlay[turn].syncDatas;
                data = ConvertToType<T>(nextTurnStateData);
            }
            else if (listType == typeof(int))
            {
                int maxTubeDrag = 0;
                if (gamePlayData.dataPlay[turn].isDifficult)
                    maxTubeDrag = gamePlayData.dataPlay[turn].syncDatas.Count;
                else
                    maxTubeDrag = (gamePlayData.dataPlay[turn].syncDatas.Count > BEPS01RFSHandleData.COMPARISON_INDEX) ? BEPS01RFSHandleData.THREE_TUBES : BEPS01RFSHandleData.TWO_TUBES;
                data = ConvertToType<T>(maxTubeDrag);
            }
            else if (listType == typeof(List<UserEndGameData.Word>))
            {
                List<UserEndGameData.Word> wordList = new List<UserEndGameData.Word>();

                foreach (var turnPlay in gamePlayData.dataPlay)
                {
                    wordList.Add(turnPlay.wordDataSentence);
                    foreach (var itemTurn in turnPlay.wordDataAnswers)
                    {
                        wordList.Add(itemTurn);
                    }
                }
                data = ConvertToType<T>(wordList);
            }
            else
            {
                data = ConvertToType<T>(null);
            }
            return data;
        }
        public override int GetMaxTurn()
        {
            return gamePlayData.dataPlay.Count;
        }

        public override void SetData<T>(T data)
        {
            DataGameModel dataGameModel = ConvertToType<DataGameModel>(data);
            if (dataGameModel == null) LogMe.Log("Lucanhtai dataGameModel BEPS01Re_FuelingSpacecraft == null");
            LogMe.Log(dataGameModel.JsonConfig);
            BEPS01RFSConfigGameData configDataGame = JsonUtility.FromJson<BEPS01RFSConfigGameData>(dataGameModel.JsonConfig);
            if (configDataGame == null) LogMe.Log("Lucanhtai configDataGame BEPS01Re_FuelingSpacecraft == null");

            List<BEPS01RFSModelData> listDataTurn = configDataGame.data.ToList();
            listDataTurn = listDataTurn.OrderBy(item => item.order).ToList();

            BEPS01RFSGamePlayData playData = new BEPS01RFSGamePlayData();
            playData.dataPlay = new List<BEPS01RFSConversationData>();


            for (int i = 0; i < listDataTurn.Count; i++)
            {
                BEPS01RFSModelData dataServer = listDataTurn[i];
                BEPS01RFSConversationData turnData = new BEPS01RFSConversationData();
                turnData.audioWords = new List<AudioClip>();
                turnData.textSentence = dataGameModel.DataGamePrimitiveDict[dataServer.question_data].Text;
                turnData.audioSentence = dataGameModel.DataGamePrimitiveDict[dataServer.question_data].AudioDataGameModelsList[0].AudioClip;
                turnData.wordDataSentence = new UserEndGameData.Word();
                turnData.wordDataSentence.TextID = dataGameModel.DataGamePrimitiveDict[dataServer.question_data].TextID;
                turnData.wordDataSentence.Type = dataGameModel.DataGamePrimitiveDict[dataServer.question_data].Type;
                turnData.syncDatas = dataGameModel.DataGamePrimitiveDict[dataServer.question_data].AudioDataGameModelsList[0].SyncData;
                turnData.wordDataAnswers = new List<UserEndGameData.Word>();

                string keyLever = dataGameModel.DataGamePrimitiveDict[configDataGame.level].Text;
                turnData.isDifficult = keyLever.Equals(KEY_LEVER_HARD, StringComparison.OrdinalIgnoreCase); 
                foreach (var item in turnData.syncDatas)
                {
                    int idFilter = Array.Find(configDataGame.word_bk, (itemF) => dataGameModel.DataGamePrimitiveDict[itemF].Text.Equals(Regex.Replace(item.w, pattern, ""), StringComparison.OrdinalIgnoreCase));
                    AudioClip audioFilter = dataGameModel.DataGamePrimitiveDict[idFilter].AudioDataGameModelsList[0].AudioClip;
                    UserEndGameData.Word wordAns = new UserEndGameData.Word();
                    wordAns.TextID = dataGameModel.DataGamePrimitiveDict[idFilter].TextID;
                    wordAns.Type = dataGameModel.DataGamePrimitiveDict[idFilter].Type;
                    turnData.audioWords.Add(audioFilter);
                    turnData.wordDataAnswers.Add(wordAns);
                }
                playData.dataPlay.Add(turnData);
            }
            gamePlayData = playData;
        }
    }

    //Data play
    [Serializable]
    public class BEPS01RFSGamePlayData
    {
        public List<BEPS01RFSConversationData> dataPlay;
    }
    [Serializable]
    public class BEPS01RFSConversationData
    {
        public UserEndGameData.Word wordDataSentence;
        public List<UserEndGameData.Word> wordDataAnswers;
        public string textSentence;
        public AudioClip audioSentence;
        public List<AudioClip> audioWords;
        public List<SyncData> syncDatas;
        public bool isDifficult;
    }

    //Data server
    [Serializable]
    public class BEPS01RFSConfigGameData
    {
        public BEPS01RFSModelData[] data;
        public int level;
        public int[] word_bk;
    }

    [Serializable]
    public class BEPS01RFSModelData
    {
        public int order;
        public int question_data;
    }
}