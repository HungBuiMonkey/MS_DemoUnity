using DG.Tweening;
using MonkeyBase.Observer;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundInitState : FSMState
    {
        private BEPP01BlendingSoundInitStateDataObjectDependency dependency;
        private Vector3 vectorScale = new Vector3(1, 1, 1);
        private float sizeSumItems;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundInitStateData initStateData = (BEPP01BlendingSoundInitStateData)data;
            SetData(initStateData);
        }

        public override void SetUp(object data)
        {

            dependency = (BEPP01BlendingSoundInitStateDataObjectDependency)data;
        }

        private void SetData(BEPP01BlendingSoundInitStateData initStateData)
        {
            BEPP01BlendingSoundInitStateEventData bEPP01BlendingSoundInitStateEventData = new();
            bEPP01BlendingSoundInitStateEventData.lstBEPP01TextAlphabet = new();

            if (initStateData.numberPhonicFilled == 0)
            {
                // create underline
                for (int i = 0; i < initStateData.listPhonic.Count; i++)
                {
                   var lineObj =  GameObject.Instantiate(dependency.prefabUnderline, dependency.boxUnderline);
                    BEPP01Text alphabetUnderline = lineObj.GetComponent<BEPP01Text>();
                    sizeSumItems += lineObj.GetComponent<RectTransform>().rect.width;
                    alphabetUnderline.SetText(initStateData.listPhonic[i]);
                    alphabetUnderline.SetData(null, i, new List<int>()); 
                    alphabetUnderline.name = "Underline" + alphabetUnderline.GetText();
                    dependency.listLine.Add(lineObj);
                }


                for (int i = 0; i < dependency.listButtonAnswer.Count; i++)
                {
                    dependency.listButtonAnswer[i].transform.localScale = Vector3.one;
                }

                var spacing = dependency.boxUnderline.GetComponent<HorizontalLayoutGroup>().spacing;
                var widthParent = (float)dependency.board.GetComponent<RectTransform>().rect.width;
                var withContent = (sizeSumItems + spacing * dependency.listLine.Count);
                float sizeCompare = 1;
                if (widthParent < withContent)
                {
                    dependency.boxUnderline.sizeDelta = new Vector2(widthParent, dependency.boxUnderline.sizeDelta.y); 
                    sizeCompare = (float)widthParent / withContent;
                    foreach (var item in dependency.listLine)
                    {
                        item.transform.localScale *= sizeCompare;
                    }

                }
            }
            for (int i = 0; i < initStateData.listAnswer.Count; i++)
            {
                initStateData.listAnswer[i].turn = i;
            }
       
            

            List<BEPP01AnswerData> answerShuffle = new List<BEPP01AnswerData>();
            foreach (var item in initStateData.listAnswer)
            {
                answerShuffle.Add(item);
            }
            FisherYatesShuffle(answerShuffle);
            // set text Answer
            for (int i = 0; i < answerShuffle.Count; i++)
            {
                BEPP01Text textContent = dependency.listButtonAnswer[i].GetComponentInChildren<BEPP01Text>();
                textContent.SetText(answerShuffle[i].strAnswer);
                textContent.SetData(answerShuffle[i],-1, answerShuffle[i].indexes);
                foreach(var ind in answerShuffle[i].indexes)
                {
                    GameObject underLine = dependency.listLine.Find((item) => item.GetComponent<BEPP01Text>().GetIndexLine() == ind);
                    underLine.GetComponent<BEPP01Text>().SetData(answerShuffle[i]);
                }
                textContent.transform.parent.name = "Answer" + textContent.GetText();
                bEPP01BlendingSoundInitStateEventData.lstBEPP01TextAlphabet.Add(textContent);
            }

            for (int i = 0; i < dependency.boxAnswer.childCount; i++)
            {
                if (i > initStateData.listAnswer.Count - 1)
                {
                    dependency.boxAnswer.GetChild(i).gameObject.SetActive(true);

                }
            }

            if (initStateData.listAnswer.Count < dependency.boxAnswer.childCount)
            {

                for (int i = 0; i < dependency.boxAnswer.childCount; i++)
                {
                    if (i > initStateData.listAnswer.Count - 1)
                    {
                        dependency.boxAnswer.GetChild(i).gameObject.SetActive(false);
                        dependency.listButtonAnswer.Remove(dependency.boxAnswer.GetChild(i).GetComponent<Button>());
                        dependency.StudentGroup.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            dependency.boxAnswer.transform.position = dependency.pointAnswerInit.position;
            Button answerCorrect = dependency.listButtonAnswer.Find((item) => item.GetComponentInChildren<BEPP01Text>().GetData().turn == 0);
            bEPP01BlendingSoundInitStateEventData.answerCorrect = answerCorrect.GetComponentInChildren<BEPP01Text>();
           
            TriggerFinishSetData(bEPP01BlendingSoundInitStateEventData);
        }
        private void FisherYatesShuffle(List<BEPP01AnswerData> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                BEPP01AnswerData value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private void TriggerFinishSetData(BEPP01BlendingSoundInitStateEventData bEPP01BlendingSoundInitStateEventData)
        {
            BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new BEPP01BlendingSoundEvent(BEPP01BlendingSoundEvent.INIT_STATE_FINISH, bEPP01BlendingSoundInitStateEventData);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
        }
      
    }

    public class BEPP01BlendingSoundInitStateData
    {
        public string strSentence;
        public int numberPhonicFilled;
        public List<BEPP01AnswerData> listAnswer { get; set; }
        public List<string> listPhonic { get; set; }
    }
    public class BEPP01BlendingSoundInitStateEventData
    {
        public List<BEPP01Text> lstBEPP01TextAlphabet { get; set; }
        public BEPP01Text answerCorrect { get; set; }
    }

    public class BEPP01BlendingSoundInitStateDataObjectDependency
    {
        public List<Button> listButtonAnswer { get; set; }
        public List<GameObject> listLine { get; set; }
        public Transform boxAnswer { get; set; }
        public RectTransform board { get; set; }
        public RectTransform boxUnderline { get; set; }
        public GameObject prefabUnderline { get; set; }
        public Transform StudentGroup { get; set; }
        public RectTransform pointAnswerInit { set; get; }
    }
}