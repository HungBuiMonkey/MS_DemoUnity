
using MonkeyBase.Observer;
using UnityEngine;
using UnityEngine.UI;

public class MKButton : BaseButton
{
    public override void Enable(bool isEnable)
    {
       
        button.interactable = isEnable;
    }

    public override void OnClick()
    {
        
        UserInputChanel buttonData = new UserInputChanel(UserInputChanel.BUTTON_CLICK, gameObject.name);
        ObserverManager.TriggerEvent<UserInputChanel>(buttonData);
    }

   
}
