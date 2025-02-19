using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{

    [CreateAssetMenu(fileName = "BEPS02FlyingOwlsScriptableObject", menuName = "ScriptableObjects/BEPS02FlyingOwls/Setting", order = 1)]
    public class BEPS02FlyingOwlsScriptableObject : ScriptableObject
    {
        public AudioClip audioBackground;
        public BEPS02FlyingOwlsIntroConfig introConfig;
        public BEPS02FlyingOwlsDragConfig dragConfig;
        public BEPS02OwlConfig owlConfig;
        public BEPS02FlyingOwlsGuidingConfig guidingConfig;
    }


    [Serializable]
    public class BEPS02FlyingOwlsIntroConfig
    {
        public List<AudioClip> audiosTopic;
        public int miliSecondDelay;
    }

    [Serializable]
    public class BEPS02FlyingOwlsDragConfig
    {
        public AudioClip sfxCorrect;
        public AudioClip sfxWrong;
        public AudioClip sfxGuiding;
        public AudioClip sfxWin;
        public int miliSecondDelay;
    }

    [Serializable]
    public class BEPS02FlyingOwlsGuidingConfig
    {
        public List<AudioClip> audiosTopic;
        public AudioClip sfxAppear;
        public float secondWaitStartGuiding;
        public float secondDelay;
    }

    [Serializable]
    public class BEPS02OwlConfig
    {
        public Color32 colorSync;
        public Color32 colorNormal;
        public Color32 colorDisable;
        public AudioClip sfxOwlHoot;
        public AudioClip sfxOwlFly;
        public string owlFly;
        public string owlFlyToNormal;
        public string owlNormal;
        public string owlNormalToFly;
        public string owlUserTap;
        public string owlTapCorrect;
        public string owlTapWrong;
        public string owlTapAndHold;
        public string owlDrop;
    }
}
