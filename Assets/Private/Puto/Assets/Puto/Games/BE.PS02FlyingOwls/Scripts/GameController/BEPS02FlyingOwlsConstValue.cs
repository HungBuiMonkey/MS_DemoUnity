using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
  
    public enum BEPS02FlyingOwlsState
    {
        InitData,
        IntroGame,
        PlayGame,
        PlayEffect,
        DragResult,
        DragCorrect,
        DragWrong,
        ClickObject,
        DraggingObject,
        NextTurnGame,
        OutroGame,
        FinishGame
    }

    public enum BEPS02FlyingOwlsUserInput
    {
        Click,
        UnClick,
        Dragging,
        DragMatching,
        DragUnMatching
    }

 
}