

using System;

namespace Monkey.MonkeyBase.NativeBridge
{
    public abstract class NativeBridge : TPRLSingleton<NativeBridge>
    {
        // Phương thức gửi dữ liệu từ Unity sang app native mà không chờ kết quả trả về
        public abstract void SendDataToNative(string type, string data);

        // Phương thức gửi dữ liệu từ Unity sang app native và chờ kết quả trả về
        public abstract void SendDataToNative(string type, string data, Action<object> callback);

        // Phương thức này sẽ được gọi từ app native khi có kết quả trả về
        public abstract void OnResultFromNative(string data);

        // Phương thức yêu cầu Unity thực hiện một hành động nào đó từ app native
        public abstract void RequestUnityAction(string data);

        public abstract void RegisterActionForRequestFromReactNative(Action<string> actionForRequest);
    }
}
