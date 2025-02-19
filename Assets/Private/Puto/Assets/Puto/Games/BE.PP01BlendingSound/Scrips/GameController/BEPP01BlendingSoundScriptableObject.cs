using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    [CreateAssetMenu(fileName = "BEPP1BlendingSoundScriptableObject", menuName = "ScriptableObjects/BEPP01BlendingSound/Setting", order = 1)]
    public class BEPP01BlendingSoundScriptableObject : ScriptableObject
    {
        public BEPP01BlendingSoundIntroConfig introDataConfig;
        public BEPP01BlendingSoundGuidingConfig guidingDataConfig;
        public BEPP01BlendingSoundClickAnswerConfig clickAnswerDataConfig;
    }
    [Serializable]
    public class BEPP01BlendingSoundIntroConfig
    {
        public int miliSecondStartDelay;
        public float secondTimeZoom;
        public AudioClip audioHandup;
        public AudioClip audioGuiding;
    }
    [Serializable]
    public class BEPP01BlendingSoundGuidingConfig
    {
        public AudioClip audioGuiding;
    }
   
    [Serializable]
    public class BEPP01BlendingSoundClickAnswerConfig
    {
        public AudioClip audioCorrect;
        public AudioClip audioWrong;
        public AudioClip audioHandDown;
    }
}