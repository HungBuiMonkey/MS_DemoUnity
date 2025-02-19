using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    [CreateAssetMenu(fileName = "BEPP01BlendingSoundMockDataScriptableObject", menuName = "ScriptableObjects/BEPP01BlendingSound/MockData", order = 1)]
    public class BEPP01BlendingSoundMookDataScriptableObject : ScriptableObject
    {
        public string strSentence;
        public AudioClip audioSentence;
        public List<BEPP01AnswerData> listData;
    }
}