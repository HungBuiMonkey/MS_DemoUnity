using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class DataServerModel
{
    [JsonProperty("lvs")] public List<LevelDatabase> listLevel { get; set; }
    [JsonProperty("v")] public int version { get; set; }

    [Serializable]
    public class LevelDatabase
    {
        [JsonProperty("i")] public int id { get; set; }
        [JsonProperty("t")] public string title { get; set; }
        [JsonProperty("n")] public string name { get; set; }
        [JsonProperty("th")] public string linkThumb { get; set; }
        [JsonProperty("o")] public int orderBy { get; set; }
        [JsonProperty("cs")] public List<CategoryDatabase> listCategory { get; set; }
    }

    [Serializable]
    public class CategoryDatabase
    {
        [JsonProperty("i")] public int id { get; set; }
        [JsonProperty("n")] public string name { get; set; }
        [JsonProperty("o")] public int orderBy { get; set; }
        [JsonProperty("th")] public string linkThumb { get; set; }
        [JsonProperty("lv_i")] public int levelID { get; set; }
        [JsonProperty("us")] public List<UnitDatabase> listUnit { get; set; }
    }

    [Serializable]
    public class UnitDatabase
    {
        [JsonProperty("i")] public int id { get; set; }
        [JsonProperty("t")] public string title { get; set; }
        [JsonProperty("o")] public int orderBy { get; set; }
        [JsonProperty("c_i")] public int categoryID { get; set; }
        public string categoryName { get; set; }
        [JsonProperty("ls")] public List<LessonDatabase> listLesson { get; set; }
    }

    [Serializable]
    public class LessonDatabase
    {
        [JsonProperty("i")] public int id { get; set; }
        [JsonProperty("t")] public string title { get; set; }
        [JsonProperty("a")] public int age { get; set; }
        [JsonProperty("ty")] public int type { get; set; }
        [JsonProperty("f")] public int free { get; set; }
        [JsonProperty("o")] public int orderBy { get; set; }
        [JsonProperty("u_i")] public int unitID { get; set; }
        [JsonProperty("as")] public List<ActivityDatabase> listActivity { get; set; }
    }

    [Serializable]
    public class ActivityDatabase
    {
        [JsonProperty("i")] public int id { get; set; }
        [JsonProperty("l_i")] public int lessonID { get; set; }
        [JsonProperty("g_i")] public int gameID { get; set; }
        [JsonProperty("o")] public int orderBy { get; set; }
        [JsonProperty("f")] public string linkActivityConfig { get; set; }
        [JsonProperty("p")] public int group { get; set; }
        [JsonProperty("g_c")] public GameConfigDatabase gameConfig { get; set; }
    }

    [Serializable]
    public class GameConfigDatabase
    {
        [JsonProperty("b")] public string background { get; set; }
        [JsonProperty("c")] public string character { get; set; }
    }

    [Serializable]
    public class VideoCallDownload
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("bundle_path")] public string bundlePath { get; set; }
    }

    public class DataUnitInsert
    {
        public int id { get; set; }
        public string title { get; set; }
        public int orderBy { get; set; }
        public int categoryID { get; set; }
        public int levelID { get; set; }

    }

    public class GameConfigDatabaseInsert
    {
        public string background { get; set; }
        public string character { get; set; }
        public int activityID { get; set; }
    }
}
