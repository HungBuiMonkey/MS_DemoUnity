
using MonkeyBase.Observer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyBase.NativeBridge
{
    public class ReactNativeBridge : NativeBridge
    {
        private Dictionary<string, Action<object>> MethodActions = new Dictionary<string, Action<object>>();
        private Action<string> actionForRequestFromReactNative;
        private int messageID;
       
        public override void SendDataToNative(string type, string data)
        {
            messageID++;
            string dataSendReactNative = ConvertDataToString(messageID, type, data);
            Debug.Log($"Send To ReactNative with data : {dataSendReactNative}");
            NativeAPI.SendData(dataSendReactNative);
        }
        public override void SendDataToNative(string type, string data, Action<object> callback)
        {
            messageID++;
            string strMessageID = messageID.ToString();
            MethodActions.Add(strMessageID, callback);
            string dataSendReactNative = ConvertDataToString(messageID, type, data);
            Debug.Log($"Send To ReactNative with data : {dataSendReactNative}");
            NativeAPI.SendData(dataSendReactNative);
        }
        public override void OnResultFromNative(string data)
        {
            Debug.Log($"Recived To ReactNative with data : {data}");
            ResponseData response = JsonConvert.DeserializeObject<ResponseData>(data);
            string messageID = response.Id;
            if (MethodActions.ContainsKey(messageID))
            {
                Action<Payload> action = MethodActions[messageID];
                Payload messageData = response.Payload;
                action?.Invoke(messageData);
            }
        }

        public override void RequestUnityAction(string data)
        {
            Debug.Log($"Recived To ReactNative with data : {data}");
            actionForRequestFromReactNative?.Invoke(data);
        }

        public override void RegisterActionForRequestFromReactNative(Action<string> actionForRequest)
        {
            actionForRequestFromReactNative = actionForRequest;
        }    

        private string ConvertDataToString(int messageID, string type, string data)
        {
            string strMessageID = messageID.ToString();
            RequestData requestData = new RequestData();
            requestData.id = strMessageID;
            requestData.type = type;
            requestData.payload = data;
            return JsonUtility.ToJson(requestData);
        }
    }

}
