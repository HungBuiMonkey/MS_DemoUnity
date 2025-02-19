
using Monkey.MonkeyBase.NativeBridge;
using MonkeyBase.Observer;
using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJFiveToolTest
{
    public class ButtonPanelLessonMode : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelName;
        [SerializeField] private Image image;
        [SerializeField] private string dataFake;
        public enum State
        {
            Lock = 1,
            Current = 2,
            Finish = 3
        }

        private int idLesson;
        protected Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        public void SetData(string name, int id, State state)
        {
            labelName.text = name;
            idLesson = id;
            if (state == State.Lock)
            {
                image.color = Color.white;
            }
            else if (state == State.Current)
            {
                image.color = Color.red;
            }
            else if (state == State.Finish)
            {
                image.color = Color.green;
            }
            gameObject.SetActive(true);
        }

        public void SetData(State state)
        {
            if (state == State.Lock)
            {
                image.color = Color.white;
            }
            else if (state == State.Current)
            {
                image.color = Color.red;
            }
            else if (state == State.Finish)
            {
                image.color = Color.green;
            }
            gameObject.SetActive(true);
        }

        public void OnClick()
        {           
            DataLessonRequest dataLesson = new DataLessonRequest();
            dataLesson.lesson_id = idLesson;
            string data = JsonUtility.ToJson(dataLesson);
            ReactNativeBridge.Instance.SendDataToNative("GetDataLesson", data, CallBack);

        }

        private void CallBack(object data)
        {
            Payload payload = (Payload)data;
            if (payload.Success)
            {
                string jsonResult = payload.Result;
                DataLessonResult dataLessonResult = JsonUtility.FromJson<DataLessonResult>(jsonResult);
                UserActionChanel dataUserAction = new UserActionChanel(TypeUserAction.LessonMode_SelectLesson, dataLessonResult);
                ObserverManager.TriggerEvent(dataUserAction);
            }
        }
    }

    public class DataLessonRequest
    {
        public int lesson_id;
    }

    public class DataLessonResult
    {
        public string activity_path;
        public string word_path;
        public int game_id;
    }
}
