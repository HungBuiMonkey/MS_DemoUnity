using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    [CreateAssetMenu(fileName = "BEID01MJConfigScriptableObject", menuName = "ScriptableObjects/BEID01IntroDialogMJ/Setting", order = 1)]

    public class BEID01IntroDialogMJConfigScriptableObject : ScriptableObject
    {
        public BEID01MJIntroConfig introConfig;
        public BEID01MJListenConfig listenConfig;
        public BEID01MJPlayConfig playConfig;
    }
    [Serializable]
    public class BEID01MJIntroConfig
    {
        public int milisecondTimeDelay;
        public AudioClip topicSoundMJ;
        public AudioClip popupSound;
    }
    [Serializable]
    public class BEID01MJPlayConfig
    {
        public AudioClip yeahAudio;
        public int milisecondDelayAppearNextButton;
        public float timeFadeOut;
    }

    [Serializable]
    public class BEID01MJListenConfig
    {
        //public AudioClip sfxClickGift;
        public int milisecondTimeDelay500;
        public int milisecondTimeDelay200;
    }
}