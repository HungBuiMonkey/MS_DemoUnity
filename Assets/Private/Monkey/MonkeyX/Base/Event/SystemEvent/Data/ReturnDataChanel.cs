namespace MonkeyBase.Event.Data
{

    public struct ReturnDataChanel 
    {
        public enum TypeDataReturn
        {
            CheckDataServer = 0,
            SyncUserDataLearn = 1,
            GetDataMap = 2,
            DownLoadLesson = 3,
            GetDataLesson = 4,
            GetDataListLevel = 5,
            UpdateDataLesson = 6,
            GetDataActivityByUnit = 7,
            GetLevel = 8,
            GetLessonOnboarding = 9,
            GetListLessonDownloadInBackground = 10,
            FinishDownLoadLessonInBackground = 11,
            GetStaleWords = 12,
        }

        public TypeDataReturn TypeData;
        public object Data;
        public void OnMMEvent(ReturnDataChanel eventType)
        {

        }

        public ReturnDataChanel(TypeDataReturn typeData, object data)
        {
            TypeData = typeData;
            Data = data;
        }

        public ReturnDataChanel(TypeDataReturn typeData)
        {
            TypeData = typeData;
            Data = null;
        }
    }
}
