using MonkeyBase.Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkipGame : MKButton
{
    [SerializeField] protected Image buttonImage;
    [SerializeField] protected TextMeshProUGUI textButton;
    protected bool isEnableClick;
   
    public override void OnClick()
    {
        base.OnClick();
        if (!isEnableClick)
            return;
        EventUserPlayGameChanel userEvent = new EventUserPlayGameChanel(EventUserPlayGameChanel.UserEvent.FinishGame, null);
        ObserverManager.TriggerEvent(userEvent);
    }

    protected void ListennerChangeViewButtonFinish(object data = null)
    {
        bool isActive = (bool)data;
        if (buttonImage != null)
            buttonImage.enabled = isActive;
        if (textButton != null)
            textButton.enabled = isActive;
        isEnableClick = isActive;
    }
}
