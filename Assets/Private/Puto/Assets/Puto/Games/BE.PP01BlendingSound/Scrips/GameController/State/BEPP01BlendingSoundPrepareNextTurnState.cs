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
    public class BEPP01BlendingSoundPrepareNextTurnState : FSMState
    {
        private BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundPrepareNextTurnStateData prepareNextTurnStateData = (BEPP01BlendingSoundPrepareNextTurnStateData)data;
            cts = new();
            DoWork(prepareNextTurnStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency)data;
        }

        private void DoWork(BEPP01BlendingSoundPrepareNextTurnStateData prepareNextTurnStateData)
        {
            List<int> listIndexUnderline = prepareNextTurnStateData.answerTurn.GetIndexes();
            foreach (var ind in listIndexUnderline)
            {
                GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                BaseImage imageUnderline = underLine.GetComponentInChildren<BEPP01Image>();
                imageUnderline.transform.parent.GetComponent<Button>().enabled = true;
                imageUnderline.SetColor(Color.red);
            }
          
            dependency.ButtonEllie.enabled = true;
            for (int i = 0; i < dependency.ListButtonAnswer.Count; i++)
            {
                dependency.ListButtonAnswer[i].enabled = true;
            }

            TriggerFinsihPrepareNextTurn();
        }
        private void TriggerFinsihPrepareNextTurn()
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.NEXTTURN_STATE_FINISH, null);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts.Cancel();
            cts.Dispose();
        }
    }

    public class BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency
    {
        public RectTransform BoxAnswer { get; set; }
        public List<Button> ListButtonAnswer { get; set; }
        public RectTransform boxUnderline { set; get; }
        public Button ButtonEllie { set; get; }
        public List<GameObject> listLine { get; set; }

    }
    public class BEPP01BlendingSoundPrepareNextTurnStateData
    {
        public BEPP01Text answerTurn { get; set; }
        public int numberPhonicFilled { get; set; }
        public List<BEPP01AnswerData> listAnswer { get; set; }
        public int Turn { get; set; }

    }
}