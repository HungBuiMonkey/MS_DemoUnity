using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public enum BERV01State
    {
        InitData,
        IntroGame,
        PlayGame,
        ClickObject,
        OutroGame,
        FinishGame
    }

    public enum BERV01UserInput
    {
        ClickFish,
        ClickCorrect,
        ClickWrong,
        FishSwimToPlace,
        SkipGuiding,
    }
    public enum BERV01FishState
    {
        SharkBite,
        SwimToPlace,
    }
    public enum BERV01TypeFish
    {
        Short = 0,
        Long = 1,
    }
    public enum BERV01FishDirection
    {
        Left = 0,
        Right = 1,
    }
}