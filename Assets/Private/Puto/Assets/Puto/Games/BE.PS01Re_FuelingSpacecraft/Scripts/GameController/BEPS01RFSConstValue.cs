using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public enum BEPS01RFSState
    {
        InitData,
        IntroGame,
        PlayGame,
        DragResult,
        DragCorrect,
        DragWrong,
        ClickObject,
        DraggingObject,
        NextTurnGame,
        EndGame,
        FinishGame
    }

    public enum BEPS01RFSUserInput
    {
        Click,
        SkipGuiding,
        SkipIntro,
        UnClick,
        Dragging,
        DragMatching,
        DragUnMatching,
        ClickCat,
    }
}