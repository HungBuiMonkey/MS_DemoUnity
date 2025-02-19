using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundAdapter : Adapter
    {
        private BEPP01PlayBlendingSoundData bEPP01PlayBlendingSoundData;
        public override T GetData<T>(int turn)
        {
            T data;

            Type listType = typeof(T);
            if (listType == typeof(BEPP01BlendingSoundInitStateData))
            {
                BEPP01BlendingSoundInitStateData bEPP01BlendingSoundInitStateData = new();
                bEPP01BlendingSoundInitStateData.strSentence = bEPP01PlayBlendingSoundData.datas[turn].strSentence;
                bEPP01BlendingSoundInitStateData.listAnswer = bEPP01PlayBlendingSoundData.datas[turn].listData;
                bEPP01BlendingSoundInitStateData.listPhonic = bEPP01PlayBlendingSoundData.datas[turn].listPhonic;
                data = ConvertToType<T>(bEPP01BlendingSoundInitStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundIntroStateData))
            {
                BEPP01BlendingSoundIntroStateData introStateData = new();
                introStateData.audioWord = bEPP01PlayBlendingSoundData.datas[turn].audioSentence;
                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundGuidingStateData))
            {
                BEPP01BlendingSoundGuidingStateData guidingStateData = new();
                guidingStateData.audioWord = bEPP01PlayBlendingSoundData.datas[turn].audioSentence;
                data = ConvertToType<T>(guidingStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundClickAnswerStateDataPlay))
            {
                BEPP01BlendingSoundClickAnswerStateDataPlay clickAnswerStateDataPlay = new();
                clickAnswerStateDataPlay.listAnswer = bEPP01PlayBlendingSoundData.datas[turn].listData;
                data = ConvertToType<T>(clickAnswerStateDataPlay);
            }
            else if (listType == typeof(BEPP01BlendingSoundEndGameStateData))
            {
                BEPP01BlendingSoundEndGameStateData endGameStateData = new();
                endGameStateData.audioWord = bEPP01PlayBlendingSoundData.datas[turn].audioSentence;
                endGameStateData.WordList = new List<UserEndGameData.Word>();
                foreach(var dataTurn in bEPP01PlayBlendingSoundData.datas)
                {
                    endGameStateData.WordList.Add(dataTurn.wordDataBlending);
                    foreach (var item in dataTurn.listData)
                    {
                        endGameStateData.WordList.Add(item.wordDataAnswer);
                    }
                }

                data = ConvertToType<T>(endGameStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundPrepareNextTurnStateData))
            {
                BEPP01BlendingSoundPrepareNextTurnStateData prepareNextTurnStateData = new();
                prepareNextTurnStateData.listAnswer = bEPP01PlayBlendingSoundData.datas[turn].listData;
                data = ConvertToType<T>(prepareNextTurnStateData);
            }
            else
            {
                data = ConvertToType<T>(null);
            }
            return data;
        }

        public override int GetMaxTurn()
        {
            return bEPP01PlayBlendingSoundData.datas.Count;
        }

        public override void SetData<T>(T data)
        {
            DataGameModel dataGameModel = ConvertToType<DataGameModel>(data);
            LogMe.Log("Lucanhtai :" + dataGameModel.JsonConfig);
            BEPP01DataConfig dataSeverConfig = JsonConvert.DeserializeObject<BEPP01DataConfig>(dataGameModel.JsonConfig);
            bEPP01PlayBlendingSoundData = new();
            bEPP01PlayBlendingSoundData.datas = new();
          

            for (int turnData = 0; turnData < dataSeverConfig.data.Length; turnData++)
            {
                DataObjectBlending dataObject = new();
                dataObject.listData = new();
                dataObject.wordDataBlending = new UserEndGameData.Word();
                dataObject.listPhonic = new List<string>();
                int wordBlending = dataSeverConfig.data[turnData].blending;

                dataObject.wordIdBlending = wordBlending;
                dataObject.audioSentence = dataGameModel.DataGamePrimitiveDict[wordBlending].AudioDataGameModelsList[0].AudioClip;
                dataObject.strSentence = dataGameModel.DataGamePrimitiveDict[wordBlending].Text;
                dataObject.wordDataBlending.TextID = dataGameModel.DataGamePrimitiveDict[wordBlending].TextID;
                dataObject.wordDataBlending.Type = dataGameModel.DataGamePrimitiveDict[wordBlending].Type;
                foreach(var item in dataObject.strSentence)
                {
                    dataObject.listPhonic.Add(item.ToString());
                }
                int startIndexSinglePhonic = 0;
                var usedIndices = new HashSet<int>();

                for (int i = 0; i < dataSeverConfig.data[turnData].phonic.Length; i++)
                {
                    BEPP01AnswerData bEPP01AnswerData = new();
                    bEPP01AnswerData.wordDataAnswer = new UserEndGameData.Word();
                    bEPP01AnswerData.indexes = new List<int>();
                   
                    int idWordPhonic = dataSeverConfig.data[turnData].phonic[i];
                    bEPP01AnswerData.wordAnswerId = idWordPhonic;
                    bEPP01AnswerData.strAnswer = dataGameModel.DataGamePrimitiveDict[idWordPhonic].Text;
                    bEPP01AnswerData.audioAnswer = dataGameModel.DataGamePrimitiveDict[idWordPhonic].AudioDataGameModelsList[0].AudioClip;
                    bEPP01AnswerData.wordDataAnswer.TextID = dataGameModel.DataGamePrimitiveDict[idWordPhonic].TextID;
                    bEPP01AnswerData.wordDataAnswer.Type = dataGameModel.DataGamePrimitiveDict[idWordPhonic].Type;
                    

                    if (bEPP01AnswerData.strAnswer.Length > 1 && !bEPP01AnswerData.strAnswer.Contains("_"))
                    {
                        int currentIndex = 0;
                        foreach (char phoneme in bEPP01AnswerData.strAnswer)
                        {
                            int phonemeIndex = dataObject.strSentence.IndexOf(phoneme, currentIndex);
                            while (phonemeIndex != -1)
                            {
                                // Kiểm tra và đảm bảo chỉ số phoneme không trùng lặp
                                if (!usedIndices.Contains(phonemeIndex))
                                {
                                    bEPP01AnswerData.indexes.Add(phonemeIndex);
                                    usedIndices.Add(phonemeIndex);
                                    currentIndex = phonemeIndex + 1;
                                    break;
                                }
                                currentIndex = phonemeIndex + 1;
                                phonemeIndex = dataObject.strSentence.IndexOf(phoneme, currentIndex);
                            }
                        }
                    }
                    else if (bEPP01AnswerData.strAnswer.Contains("_"))
                    {
                        string[] splitPhonemes = bEPP01AnswerData.strAnswer.Split('_');
                        foreach (string phoneme in splitPhonemes)
                        {
                            int startIndex = 0;
                            while ((startIndex = dataObject.strSentence.IndexOf(phoneme, startIndex)) != -1)
                            {
                                if (!usedIndices.Contains(startIndex))
                                {
                                    bEPP01AnswerData.indexes.Add(startIndex);
                                    usedIndices.Add(startIndex);
                                }
                                startIndex++;
                            }
                        }
                    }
                    else
                    {
                        foreach (char phoneme in bEPP01AnswerData.strAnswer)
                        {
                            while ((startIndexSinglePhonic = dataObject.strSentence.IndexOf(phoneme, startIndexSinglePhonic)) != -1)
                            {
                                if (!usedIndices.Contains(startIndexSinglePhonic))
                                {
                                    bEPP01AnswerData.indexes.Add(startIndexSinglePhonic);
                                    usedIndices.Add(startIndexSinglePhonic);
                                    startIndexSinglePhonic++;
                                    break;
                                }
                                startIndexSinglePhonic++;
                            }
                        }
                    }
                    bEPP01AnswerData.indexes = bEPP01AnswerData.indexes.Distinct().ToList();
                    dataObject.listData.Add(bEPP01AnswerData);

                }

                bEPP01PlayBlendingSoundData.datas.Add(dataObject);
            }

        }

    }
    
    //Data for play
    [Serializable]
    public class BEPP01PlayBlendingSoundData
    {
        public List<DataObjectBlending> datas;
    }
    [Serializable]
    public class DataObjectBlending
    {
        public UserEndGameData.Word wordDataBlending;
        public int wordIdBlending;
        public string strSentence;
        public AudioClip audioSentence;
        public List<BEPP01AnswerData> listData;
        public List<string> listPhonic;
    }
    [Serializable]
    public class BEPP01AnswerData
    {
        public UserEndGameData.Word wordDataAnswer;
        public int wordAnswerId;
        public string strAnswer;
        public AudioClip audioAnswer;
        public int turn;
        public List<int> indexes;
    }
    [Serializable]
    public class BEPP01DataConfig
    {
        public BEPP01DataQuestion[] data;
    }
    [Serializable]
    public class BEPP01DataQuestion
    {
        public int blending;
        public int[] phonic;
    }
}

