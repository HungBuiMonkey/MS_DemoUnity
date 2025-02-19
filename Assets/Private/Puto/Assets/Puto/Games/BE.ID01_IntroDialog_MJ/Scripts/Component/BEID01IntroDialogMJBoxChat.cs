using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJBoxChat : MonoBehaviour
    {
        [SerializeField] protected TMP_Text mkText;
        [SerializeField] protected CanvasGroup canvasGroup;

        protected BEID01IntroDialogMJBoxChatData data;
        protected bool reviewClick = false;

        public virtual void InitData(BEID01IntroDialogMJBoxChatData data)
        {

        }

        public virtual void Play(Action callback = null)
        {

        }

        public virtual void SetReviewClick(bool value)
        {

        }

        public void SetCanvasAlpha(int value)
        {
            canvasGroup.alpha = value;
        }

        public virtual void DisableLayout()
        {

        }

        public AudioClip GetAudio()
        {
            return data.audio;
        }
        public virtual Vector3 GetPosition()
        {
            return transform.position;
        }
    }
    public enum BEID01IntroDialogMJBoxChatType
    {
        Answer,
        Question
    }

    [System.Serializable]
    public class BEID01IntroDialogMJBoxChatData
    {
        public string text;
        public AudioClip audio;
        public BEID01IntroDialogMJBoxChatType type;
    }
}