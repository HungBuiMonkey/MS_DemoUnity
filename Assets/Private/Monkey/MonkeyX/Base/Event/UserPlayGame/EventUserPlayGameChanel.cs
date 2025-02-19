
using MonkeyBase.Observer;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
public struct EventUserPlayGameChanel : EventListener<EventUserPlayGameChanel>
{
    public UserEvent EventName;
    public object Data;
    public void OnMMEvent(EventUserPlayGameChanel eventType)
    {

    }

    public EventUserPlayGameChanel(UserEvent nameEvent, object data)
    {
        EventName = nameEvent;
        Data = data;
    }

    public enum UserEvent
    {
        CloseGame = 0,
        FinishGame = 1,
        FinishLesson = 2,
        OtherReport = 3,
        StartLesson = 4
    }
}

public class UserEndGameData
{
    public int TimeSpent { get; set; }
    public List<Scores> ScoresList { get; set; }
    public List<Phonic> PhonicList { get; set; }
    public List<Word> WordList { get; set; }
    public List<Video> VideoList { get; set; }
    public int MaxTurn { get; set; }
    public class Scores
    {
        public int Score { get; set; }
    }

    [Serializable]
    public class Phonic
    {
        public string TextPhonic { get; set; }
    }

    [Serializable]
    public class Word
    {
        public int TextID { get; set; }
        public TypeWord Type { get; set; }
    }

    [Serializable]
    public class Video
    {
        public int VideoID { get; set; }
        public VideoTimes VideoTime { get; set; }
        public List<VideoActions> ActionList { get; set; }
        public List<VideoInteractionActions> ActionInteractionList { get; set; }
      
        [Serializable]
        public class VideoTimes
        {
            [JsonProperty("time_video")] public int Time { get; set; }
            [JsonProperty("time_user")] public int UsageTime { get; set; }
        }

        [Serializable]
        public class VideoActions
        {
            [JsonProperty("name")] public string Name { get; set; }
            [JsonProperty("diff_tua_start")] public int RewindStartTime { get; set; }
            [JsonProperty("duration")] public int RewindTime { get; set; }
        }

        [Serializable]
        public class VideoInteractionActions
        {
            [JsonProperty("number_interact")] public int CurrentInteractive { get; set; }
            [JsonProperty("number_tap_thuc_te")] public int ActualTouchount { get; set; }
            [JsonProperty("number_tap_li_tuong")] public int IdealTouchCount { get; set; }
            [JsonProperty("diff_tap_interact")] public int FirstTimeTouch { get; set; }
        }
    }
}

public class UserChooseAnswerData
{
   
    public List<AnswerData> ListAnswerData { get; set; }


    [Serializable]
    public class AnswerData
    {
        [JsonProperty("target")] public string Target { get; set; }
        [JsonProperty("word_id")] public int WordID { get; set; }
        [JsonProperty("word_type")] public string WordType { get; set; }
        [JsonProperty("is_correct")] public string TypeValue { get; set; }
    }

    public enum AnswerType
    {
        Correct = 1,
        Incorrect = 2,
        Passive = 3
    }

}