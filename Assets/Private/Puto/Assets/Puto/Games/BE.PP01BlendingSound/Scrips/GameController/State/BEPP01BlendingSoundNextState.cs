using Cysharp.Threading.Tasks;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundNextState : FSMState
    {
        private BEPP01BlendingSoundNextStateDependency dependency;
        private CancellationTokenSource cancellationTokenSource;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundNextStateData nextStateData = (BEPP01BlendingSoundNextStateData)data;
            cancellationTokenSource = new();
            CancellationToken token = cancellationTokenSource.Token;
            DoWork(token, nextStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundNextStateDependency)data;
        }

        private void DoWork(CancellationToken token, BEPP01BlendingSoundNextStateData nextStateData)
        {

            for (int i = 0; i < dependency.BoxUnderline.childCount; i++)
            {
                UnityEngine.Object.Destroy(dependency.BoxUnderline.GetChild(i).gameObject);

            }
            foreach (Transform O in dependency.BoxAnswer.transform)
            {
                O.GetComponent<BEPP01Image>().SetColorNormal();
            }

            TriggerFinsihPrepareNext();
        }
        private void TriggerFinsihPrepareNext()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.NEXT_STATE_FINISH, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        public override void OnExit()
        {
            base.OnExit();
            cancellationTokenSource?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

    public class BEPP01BlendingSoundNextStateDependency
    {
        public RectTransform BoxAnswer { get; set; }
        public RectTransform BoxUnderline { get; set; }
    }
    public class BEPP01BlendingSoundNextStateData
    {


    }
}