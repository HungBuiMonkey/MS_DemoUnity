using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJFiveToolTest
{
    [Serializable]
    public struct InforActivity
    {
        [JsonProperty("i")] public int  activytiID;
        [JsonProperty("g_i")] public int gameID;
        [JsonProperty("f")] public string linkDownLoad;
        [JsonProperty("n")] public string name;
    }

    public struct InforActivityPlayGame
    {
        public int GameID;
        public string LinkDownload;
    }
}
