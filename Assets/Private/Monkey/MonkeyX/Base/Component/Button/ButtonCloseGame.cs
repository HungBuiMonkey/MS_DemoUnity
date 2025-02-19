

using MonkeyBase.Observer;

public class ButtonCloseGame : MKButton
{
    public override void OnClick()
    {
        base.OnClick();
        EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.CloseGame, null);
        ObserverManager.TriggerEvent(userEvent);
    }
}
