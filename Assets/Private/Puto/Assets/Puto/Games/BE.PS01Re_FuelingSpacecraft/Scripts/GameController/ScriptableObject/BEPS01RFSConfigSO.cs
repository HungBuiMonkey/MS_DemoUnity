using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    [CreateAssetMenu(fileName = "BEPS01RFSConfigSO", menuName = "ScriptableObjects/BEPS01Re_FuelingSpacecraft/Setting", order = 1)]

    public class BEPS01RFSConfigSO : ScriptableObject
    {
        public AudioClip audioBackground;
        public BEPS01RFSIntroConfig introConfig;
        public BEPS01RFSClickConfig clickConfig;
        public BEPS01RFSSkeletonConfig skeletonConfig;
        public BEPS01RFSGuidingConfig guidingConfig;
        public BEPS01RFSTubeConfig tubeConfig;
        public BEPS01RFSDragResultConfig dragResultConfig;
        public BEPS01RFSNextTurnConfig nextTurnConfig;
    }

    [Serializable]
    public class BEPS01RFSIntroConfig
    {
        public AudioClip[] audiosTopic;
        public int timeDelay;
    }

    [Serializable]
    public class BEPS01RFSTubeConfig
    {
        public Color32 textFade;
        public Color32 textNormal;
        public Color32 textCorrect;
        public Color32 textWrong;
        public BEPS01RFSColor colorNormal;
        public BEPS01RFSColor colorCorrect;
        public BEPS01RFSColor colorWrong;
    }
    [Serializable]
    public class BEPS01RFSSkeletonConfig
    {
        public string tubeCorrect;
        public string tubeWrong;
        public string tubeNormal;
        public string catYellowIdle;
        public string catYellowMoveToSpaceship;
        public string catYellowMoveToStation;
        public string catYellowSitInTheChair;
        public string catGrayIdle;
        public string catGrayMoveToSpaceship;
        public string catGrayMoveToStation;
        public string catGraySitInTheChair;
        public string spaceshipFirstTurnIdle;
        public string spaceshipFirstTurnIdle_Blink;
        public string spaceshipSecondTurnIdle;
        public string spaceshipSecondTurnIdle_Blink;
        public string spaceshipMoveToFirstStation;
        public string spaceshipMoveToSecondStation;
        public string spaceshipMoveToEarth;
        public string[] fireworks;
        public AudioClip sfxCheer;
        public AudioClip sfxImpact;
        public AudioClip sfxMagic;
        public AudioClip sfxSpaceshipOn;
        public AudioClip sfxSpaceshipOff;
        public AudioClip sfxSpaceshipFly_1;
        public AudioClip sfxSpaceshipFly_2;
        public AudioClip sfxSpaceshipFly_3;
    }
    [Serializable]
    public class BEPS01RFSDragResultConfig
    {
        public AudioClip sfxCorrect;
        public AudioClip sfxWrong;
        public int timeDelay;
    }
    [Serializable]
    public class BEPS01RFSGuidingConfig
    {
        public AudioClip sfxAppear;
        public AudioClip sfxClick;
        public AudioClip sfxUnClick;
        public float secondWaitStartGuiding;
        public float secondDelay;
    }
    [Serializable]
    public class BEPS01RFSClickConfig
    {
        public AudioClip sfxClick;
        public AudioClip sfxCat;
        public int timeDelay;
    }
    [Serializable]
    public class BEPS01RFSNextTurnConfig
    {
        public AudioClip sfxCelestialWin ;
        public int timeDelay; 
    }
    [Serializable]
    public class BEPS01RFSColor
    {
        public Color32 background;
        public Color32 box;
    }
}