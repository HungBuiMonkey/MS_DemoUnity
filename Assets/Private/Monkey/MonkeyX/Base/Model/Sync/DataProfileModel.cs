

using static UserEndGameData;
using System;
using System.Collections.Generic;

[Serializable]
public class DataProfileModel
{
    public List<Lesson> ListLesson;
    public long Version;
    public int CurrentLevelID;
    [Serializable]
    public class Lesson
    {
        public int ID;
        public int TotalActivity;
        public int CurrentIndexActivity;
        public bool IsComplete;
        public int Type;
        public int UnitID;
        public List<Phonic> ListPhonic;
        public List<Word> ListWord;
        public List<Video> ListVideo;
    }
}