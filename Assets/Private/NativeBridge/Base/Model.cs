
using Newtonsoft.Json;

namespace Monkey.MonkeyBase.NativeBridge
{
    public class RequestData
    {
        public string id;
        public string type;
        public string payload;
    }

    #region ResponseData

    public class ResponseData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }
    }


    #endregion

    public class ReceivedData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")] 
        public string Type { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }
    }

}
