
namespace MonkeyBase.Event.Data
{
    public struct RequestDataChanel
    {
        public enum TypeDataRequest
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
            StartDownLoadLessonInBackground = 11,
            GetStaleWords = 12,
        }

        public TypeDataRequest TypeData;
        public object Data;
        public void OnMMEvent(RequestDataChanel eventType)
        {

        }

        public RequestDataChanel(TypeDataRequest typeData, object data)
        {
            TypeData = typeData;
            Data = data;
        }

        public RequestDataChanel(TypeDataRequest typeData)
        {
            TypeData = typeData;
            Data = null;
        }
    }
}
