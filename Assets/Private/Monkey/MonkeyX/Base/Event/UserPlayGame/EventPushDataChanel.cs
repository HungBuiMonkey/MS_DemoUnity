

using MonkeyBase.Observer;
using System.Collections.Generic;

public struct EventPushDataChanel : EventListener<EventPushDataChanel>
{
    public PushDataEvent EventName;
    public object Data;
    public void OnMMEvent(EventPushDataChanel eventType)
    {

    }

    public EventPushDataChanel(PushDataEvent nameEvent, object data)
    {
        EventName = nameEvent;
        Data = data;
    }

    public enum PushDataEvent
    {
        StartLesson = 0,
        EndActivity = 1,
        EndLesson = 2,
    }

    public class EndActivityData
    {
        public string GameCode { get; set; }
        public string ActivityID { get; set; }
        public List<UserEndGameData.Scores> ScoresList { get; set; }
        public int TimeSpend { get; set; }
    }

    public class EndLessonData
    {
        public string ActivityID { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public int TimeSpend { get; set; }
    }
}


