using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSSpaceship : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic skeletonGraphic;
        [SerializeField] private CanvasGroup canvasGroup;
        private string idleState;
        private string blinkState;
        private Spine.AnimationState animationState;
        private bool isAnimationLooping = true;

        public void InitState(string idleState, string blinkState)
        {
            this.idleState = idleState;
            this.blinkState = blinkState;
        }

        public SkeletonGraphic GetSkeleton()
        {
            return skeletonGraphic;
        }
        public void Fade(float value)
        {
            canvasGroup.alpha = value;
        }

        public void StartStateIdle()
        {
            isAnimationLooping = true;
            animationState = skeletonGraphic.AnimationState;
            animationState.Complete += OnAnimationComplete;
            PlayIdle();
        }
        public void StopAnimationLoop()
        {
            isAnimationLooping = false;
        }

        private void OnAnimationComplete(TrackEntry trackEntry)
        {
            if (!isAnimationLooping)
            {
                animationState.Complete -= OnAnimationComplete;
                return;
            }
            if (trackEntry.Animation.Name == idleState)
            {
                PlayBlink();
            }
            else if (trackEntry.Animation.Name == blinkState)
            {
                PlayIdle();
            }
        }

        private void PlayIdle()
        {
            animationState.SetAnimation(0, idleState, false);
        }

        private void PlayBlink()
        {
            animationState.SetAnimation(0, blinkState, false);
        }

    }
}