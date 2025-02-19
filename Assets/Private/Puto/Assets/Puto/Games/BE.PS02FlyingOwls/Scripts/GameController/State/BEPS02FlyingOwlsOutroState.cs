using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsOutroState : FSMState
    {
        private BEPS02FlyingOwlsOutroStateObjectDependency dependency;
        private CancellationTokenSource cts;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);

            DoWord();
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsOutroStateObjectDependency)data;
        }

        private async void DoWord()
        {
            cts = new CancellationTokenSource();
            dependency.GuidingHand.ResetGuiding();
            try
            {
                dependency.UIParticle.gameObject.SetActive(false);
                dependency.BackgroundFadeOut.DOColor(Color.black, 0.35f).SetEase(Ease.Linear);
                await UniTask.Delay(350, cancellationToken: cts.Token);
                BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.FinishGame, null);
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }

        }
       
        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }

    }

    public class BEPS02FlyingOwlsOutroStateObjectDependency
    {
        public Image BackgroundFadeOut { get; set; }
        public BEPS02Guiding GuidingHand { get; set; }
        public UIParticle UIParticle { get; set; }
        public ParticleSystem EffectEndGame { get; set; }
    }
}