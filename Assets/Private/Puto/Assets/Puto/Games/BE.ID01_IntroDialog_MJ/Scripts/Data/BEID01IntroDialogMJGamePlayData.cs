using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Spine.Unity;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    [Serializable]
    public class BEID01IntroDialogMJGamePlayData
    {
        public List<BEID01IntroDialogMJConversationData> listData;
        public UserEndGameData.Word wordDataCharacter;
        public UserEndGameData.Word wordDataBackground;
        public Sprite character;
        public string characterName;
        public Sprite background;
        public SkeletonDataAsset characterAsset;
    }

    [Serializable]
    public class BEID01IntroDialogMJConversationData
    {
        public UserEndGameData.Word wordData;
        public string text;
        public AudioClip audioClip;
        public Sprite image;
    }


    [Serializable]
    public class BEID01IntroDialogMJConfigGameData
    {
        public BEID01IntroDialogMJModelData[] data;
        public int background;
        public int character;
    }

    [Serializable]
    public class BEID01IntroDialogMJModelData
    {
        public int answer_w;
        public int order;
        public int question_data;
    }
}