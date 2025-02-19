using MonkeyBase.Observer;

public struct ReactNativeChanel : EventListener<ReactNativeChanel>
{
    public TypeReactNativeAction TypeData;
    public object Data;
    public void OnMMEvent(ReactNativeChanel eventType)
    {

    }

    public ReactNativeChanel(TypeReactNativeAction typeData, object data)
    {
        TypeData = typeData;
        Data = data;
    }

    public ReactNativeChanel(TypeReactNativeAction typeData)
    {
        TypeData = typeData;
        Data = null;
    }
}

public enum TypeReactNativeAction
{
    OpenMap = 0,
}