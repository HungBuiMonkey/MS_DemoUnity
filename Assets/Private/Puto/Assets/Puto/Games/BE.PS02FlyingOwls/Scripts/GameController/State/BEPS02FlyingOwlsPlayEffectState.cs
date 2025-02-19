using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsPlayEffectState : FSMState
    {
        private BEPS02FlyingOwlsPlayEffectStateObjectDependency dependency;
        private CancellationTokenSource cts;
    

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS02FlyingOwlsPlayEffectStateData playEffectStateData = (BEPS02FlyingOwlsPlayEffectStateData)data;
            DoWork(playEffectStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEPS02FlyingOwlsPlayEffectStateObjectDependency)data;
        }

        private void DoWork(BEPS02FlyingOwlsPlayEffectStateData data)
        {
            cts = new CancellationTokenSource();
            int countOwlPlaying = 0;
            foreach (var owl in dependency.ListOwls)
            {
                if (owl.IsPlaying())
                {
                    countOwlPlaying++;
                }
            }
            if (data.IsPlayEffect && !BEPS02HandleData.IsPlayingEffect)
            {
                BEPS02HandleData.IsPlayingEffect = true;
                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlFly, cts.Token);
                BEPS02HandleData.PlayEffectOwl(dependency.OwlConfig.sfxOwlHoot, cts.Token);
            } else
            {
                if(countOwlPlaying < 1)
                {
                    SoundManager.Instance.StopFxOneShot();
                    BEPS02HandleData.IsPlayingEffect = false;
                } 
            }
        }


        public override void OnExit()
        {
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }

    }

    public class BEPS02FlyingOwlsPlayEffectStateData
    {
        public bool IsPlayEffect { get; set; }
    }


    public class BEPS02FlyingOwlsPlayEffectStateObjectDependency
    {
        public BEPS02OwlConfig OwlConfig { get; set; }
        public List<BEPS02OwlDrag> ListOwls { get; set; }
    }
}