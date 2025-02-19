using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsAdapter : Adapter
    {
      /*  [SerializeField] bool isMookData;
        [SerializeField] private BEPS02FlyingOwlsMookDataScriptableObject mookDataScriptableObject;*/
        private BEPS02FlyingOwlsPlayData bEPS02FlyingOwlsPlayData;
        private const string KEY_LEVER_HARD = "hard";
        private string pattern = @"([.!?;:])";
        public override T GetData<T>(int turn)
        {
            T data;

           /* if (isMookData)
            {
                bEPS02FlyingOwlsPlayData = new BEPS02FlyingOwlsPlayData();
                bEPS02FlyingOwlsPlayData.dataPlay = new List<BEPS02PlayConversationData>();
                bEPS02FlyingOwlsPlayData.dataPlay = mookDataScriptableObject.listData;
            }*/

            Type listType = typeof(T);
            if (listType == typeof(BEPS02FlyingOwlsInitStateData))
            {
                BEPS02FlyingOwlsInitStateData initStateData = new BEPS02FlyingOwlsInitStateData();
                initStateData.SyncDatasPlay = bEPS02FlyingOwlsPlayData.dataPlay[turn].syncDatas;
                initStateData.AudioWords = bEPS02FlyingOwlsPlayData.dataPlay[turn].audioWords;
                initStateData.AudioWords = bEPS02FlyingOwlsPlayData.dataPlay[turn].audioWords;
                initStateData.IsDifficult = bEPS02FlyingOwlsPlayData.dataPlay[turn].isDifficult;

                data = ConvertToType<T>(initStateData);
            }
            else if (listType == typeof(int))
            {
                int maxOwlDrag = 0;
                if (bEPS02FlyingOwlsPlayData.dataPlay[turn].isDifficult)
                    maxOwlDrag = bEPS02FlyingOwlsPlayData.dataPlay[turn].syncDatas.Count;
                else
                    maxOwlDrag = (bEPS02FlyingOwlsPlayData.dataPlay[turn].syncDatas.Count > BEPS02HandleData.COMPARISON_INDEX) ? BEPS02HandleData.THREE_OWLS : BEPS02HandleData.TWO_OWLS;
                data = ConvertToType<T>(maxOwlDrag);
            }
            else if (listType == typeof(BEPS02FlyingOwlsIntroStateData))
            {
                BEPS02FlyingOwlsIntroStateData introStateData = new BEPS02FlyingOwlsIntroStateData();
                introStateData.DataPlay = bEPS02FlyingOwlsPlayData.dataPlay[turn];

                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsClickStateData))
            {
                BEPS02FlyingOwlsClickStateData clickStateData = new BEPS02FlyingOwlsClickStateData();
                clickStateData.AudioWords = bEPS02FlyingOwlsPlayData.dataPlay[turn].audioWords;

                data = ConvertToType<T>(clickStateData);
            }
            else if (listType == typeof(BEPS02FlyingOwlsDraggingStateData))
            {
                BEPS02FlyingOwlsDraggingStateData draggingStateData = new BEPS02FlyingOwlsDraggingStateData();
                draggingStateData.AudioWords = bEPS02FlyingOwlsPlayData.dataPlay[turn].audioWords;

                data = ConvertToType<T>(draggingStateData);
            }
           /* else if (listType == typeof(BEPS02FlyingOwlsDragResultStateData))
            {
                BEPS02FlyingOwlsDragResultStateData dragStateData = new BEPS02FlyingOwlsDragResultStateData();
                dragStateData.AudioWords = bEPS02FlyingOwlsPlayData.dataPlay[turn].audioWords;

                data = ConvertToType<T>(dragStateData);
            }*/
            else if (listType == typeof(BEPS02FlyingOwlsNextTurnStateData))
            {
                BEPS02FlyingOwlsNextTurnStateData nextTurnStateData = new BEPS02FlyingOwlsNextTurnStateData();
                nextTurnStateData.DataTurn = bEPS02FlyingOwlsPlayData.dataPlay[turn];

                data = ConvertToType<T>(nextTurnStateData);
            }
            else if (listType == typeof(List<UserEndGameData.Word>))
            {
                List<UserEndGameData.Word> wordList = new List<UserEndGameData.Word>();

                foreach (var turnPlay in bEPS02FlyingOwlsPlayData.dataPlay)
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
            return bEPS02FlyingOwlsPlayData.dataPlay.Count;
        }

        public override void SetData<T>(T data)
        {

            DataGameModel dataGameModel = ConvertToType<DataGameModel>(data);
            if (dataGameModel == null) LogMe.Log("Lucanhtai dataGameModel BEPS02FlyingOwls == null");
            LogMe.Log(dataGameModel.JsonConfig);
            BEPS02ConfigGameData configDataGame = JsonUtility.FromJson<BEPS02ConfigGameData>(dataGameModel.JsonConfig);
            if (configDataGame == null) LogMe.Log("Lucanhtai configDataGame BEPS02FlyingOwls == null");

            List<BEPS02ModelData> listDataTurn = configDataGame.data.ToList();
            listDataTurn = listDataTurn.OrderBy(item => item.order).ToList();

            BEPS02FlyingOwlsPlayData playData = new BEPS02FlyingOwlsPlayData();
            playData.dataPlay = new List<BEPS02PlayConversationData>();


            for (int i = 0; i < listDataTurn.Count; i++)
            {
                BEPS02ModelData dataServer = listDataTurn[i];
                BEPS02PlayConversationData turnData = new BEPS02PlayConversationData();
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
                    turnData.wordDataAnswers.Add(wordAns);
                    turnData.audioWords.Add(audioFilter);
                }
                playData.dataPlay.Add(turnData);
            }
            bEPS02FlyingOwlsPlayData = playData;
        }

    }


    //Data for play
    [Serializable]
    public class BEPS02FlyingOwlsPlayData
    {
        public List<BEPS02PlayConversationData> dataPlay;
    }

    [Serializable]
    public class BEPS02PlayConversationData
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
    public class BEPS02ConfigGameData
    {
        public BEPS02ModelData[] data;
        public int level;
        public int[] word_bk;
    }

    [Serializable]
    public class BEPS02ModelData
    {
        public int order;
        public int question_data;
    }

}