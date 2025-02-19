using Monkey.MJFiveToolTest;
using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private TypeUserAction typeUserAction;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UserActionChanel dataUserAction = new UserActionChanel(typeUserAction);
        ObserverManager.TriggerEvent(dataUserAction);
    }
}
