using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Monkey.MJFiveToolTest
{
    public struct UserActionChanel : EventListener<UserActionChanel>
    {
        public TypeUserAction TypeData;
        public object Data;
        public void OnMMEvent(UserActionChanel eventType)
        {

        }

        public UserActionChanel(TypeUserAction typeData, object data)
        {
            TypeData = typeData;
            Data = data;
        }

        public UserActionChanel(TypeUserAction typeData)
        {
            TypeData = typeData;
            Data = null;
        }
    }

    public enum TypeUserAction
    {
        GameMode = 0,
        GameMode_SelectGame = 1,
        GameModeSelect_Activity = 2,  
        
        GameModeCloseGame = 3,
        GameModeCloseActivity = 4,
        GameModeCloseSelectGame = 5,
        GameModeCloseScene = 6,
        LessonMode_SelectLesson = 7
    }
}
