
using MonkeyBase.Observer;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJFiveToolTest
{
    public class ButtonPanelGameMode : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelName;
        private int idGame;
        protected Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        public void SetData(string name, string code, int id, ButtonColorKey colorKey, List<ButtonColor> buttonColors)
        {
            labelName.text = name;           
            idGame = id;            
            gameObject.SetActive(true);
        }

        public void OnClick()
        {
            UserActionChanel dataUserAction = new UserActionChanel(TypeUserAction.GameMode_SelectGame, idGame);
            ObserverManager.TriggerEvent(dataUserAction);
        }
    }
}
