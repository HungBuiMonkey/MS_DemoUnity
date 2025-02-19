
using MonkeyBase.Event.Data;
using MonkeyBase.Observer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MonkeyBase.InBackGround
{
    public class DeleteResourceDownloaded : MonoBehaviour, EventListener<ReturnDataChanel>
    {
        protected string relativePathLocalResource;
        private const int LAST_ACCESS_GAP_DELETED = 7; // days
        protected virtual void Start()
        {
            this.ObserverStartListening<ReturnDataChanel>();
            RequestDataChanel dataRequest = new RequestDataChanel(RequestDataChanel.TypeDataRequest.GetStaleWords, LAST_ACCESS_GAP_DELETED);
            ObserverManager.TriggerEvent(dataRequest);
        }

        private void OnDestroy()
        {
            this.ObserverStopListening<ReturnDataChanel>();
        }

        public void OnMMEvent(ReturnDataChanel eventType)
        {
            if (eventType.TypeData == ReturnDataChanel.TypeDataReturn.GetStaleWords)
            {
                List<WordConfigDatabase> dataStaleWords = (List<WordConfigDatabase>)eventType.Data;
                
                int amountListWordIDs = dataStaleWords.Count;
                for (int count = 0; count < amountListWordIDs; count++)
                {
                   string absolutePathLocalResource = $"{relativePathLocalResource}{dataStaleWords[count].idWord}.bundle";
                    if (File.Exists(absolutePathLocalResource))
                    {
                        File.Delete(absolutePathLocalResource);
                    }
                }
            }
        }
    }

    public class WordConfigDatabase
    {
        public int idWord;
        public int idLesson;
    }
}
