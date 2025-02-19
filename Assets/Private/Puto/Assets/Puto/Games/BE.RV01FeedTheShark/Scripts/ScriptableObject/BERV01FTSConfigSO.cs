using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    [CreateAssetMenu(fileName = "BERV01ConfigSO", menuName = "ScriptableObjects/BERV01FeedTheShark/Setting", order = 1)]
    public class BERV01FTSConfigSO : ScriptableObject
    {
        public AudioClip audioBackground;
        public BERV01FTSIntroConfig introConfig;
        public BERV01FTSClickConfig clickConfig;
        public BERV01FTSGuidingConfig guidingConfig;
        public BERV01FTSOutroConfig outroConfig;
    }

    [Serializable]
    public class BERV01FTSIntroConfig
    {
        public AudioClip audioCTAWord;
        public AudioClip audioCTAPhonic;
        public int timeDelay;
    }

    [Serializable]
    public class BERV01FTSClickConfig
    {
        public AudioClip sfxCorrect;
        public AudioClip sfxWrong;
        public AudioClip sfxChomp;
    }
    [Serializable]
    public class BERV01FTSGuidingConfig
    {
        public AudioClip sfxAppear;
        public AudioClip sfxClick;
        public AudioClip sfxUnClick;
        public int timeWaitStartGuiding;
        public int timeDelay;
    }

    [Serializable]
    public class BERV01FTSOutroConfig
    {
        public AudioClip sfxWin;
        public int timeDelay;
    }
}