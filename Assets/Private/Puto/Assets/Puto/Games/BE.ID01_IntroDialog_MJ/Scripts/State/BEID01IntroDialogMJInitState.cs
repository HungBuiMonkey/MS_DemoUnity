using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MonkeyBase.Observer;
using Cysharp.Threading.Tasks;
using System.Threading;

using System;
using Spine.Unity;
using System.Threading.Tasks;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJInitState : FSMState
    {
        private BEID01IntroDialogMJInitStateDependency dependency;
        private CancellationTokenSource cancellationTokenSource;
        private List<BEID01IntroDialogMJBoxChat> listBoxChat;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEID01IntroDialogMJInitStateData initStateData = (BEID01IntroDialogMJInitStateData)data;
            listBoxChat = new();
            cancellationTokenSource = new();
            DoWork(initStateData);
        }
        private async void DoWork(BEID01IntroDialogMJInitStateData initStateData)
        {
            dependency.CharatersAnim.skeletonDataAsset = initStateData.CharacterAsset;
            dependency.CharatersAnim.allowMultipleCanvasRenderers = true;
            dependency.CharatersAnim.updateWhenInvisible = UpdateMode.OnlyAnimationStatus;
            dependency.CharatersAnim.Clear();
            dependency.CharatersAnim.Initialize(true);
            if (initStateData.CharacterName.Equals("max", StringComparison.OrdinalIgnoreCase)
                || initStateData.CharacterName.Equals("boy", StringComparison.OrdinalIgnoreCase)
                || initStateData.CharacterName.Equals("girl", StringComparison.OrdinalIgnoreCase) 
                || initStateData.CharacterName.Equals("cassie", StringComparison.OrdinalIgnoreCase)
                || initStateData.CharacterName.Equals("pip", StringComparison.OrdinalIgnoreCase))
            {
                SetPivot(dependency.CharatersAnim.GetComponent<RectTransform>(), new Vector2(0.5f, -0.2f));
            }
            else if (initStateData.CharacterName.Equals("mother", StringComparison.OrdinalIgnoreCase)
              || initStateData.CharacterName.Equals("teacher", StringComparison.OrdinalIgnoreCase))
            {
                SetPivot(dependency.CharatersAnim.GetComponent<RectTransform>(), new Vector2(0.5f, -0.5f));
            }
            else
            {
                SetPivot(dependency.CharatersAnim.GetComponent<RectTransform>(), new Vector2(0.5f, -0.95f));
            }

            dependency.BackgroundImage.sprite = initStateData.Background;
            dependency.BackgroundImage.preserveAspect = true;
            var mainCanvasRect = dependency.GameplayCanvas.GetComponent<RectTransform>();
            var listData = initStateData.ListData;
            var container = dependency.ConversationContainer;
            var content = container.GetContentRectTransform();
            container.GetComponent<RectTransform>().anchoredPosition = new Vector2(mainCanvasRect.sizeDelta.x * 0.15f, 0f);
            var heightContainer = mainCanvasRect.sizeDelta.y;
            container.GetComponent<RectTransform>().sizeDelta = new Vector2(mainCanvasRect.sizeDelta.x * 0.7f - 100, heightContainer);


            for (int i = 0; i < listData.Count; i++)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                if (listData[i].text == "")
                {
                    continue;
                }

                var go = GameObject.Instantiate((i % 2 != 0) ? dependency.AnswerPrefab : dependency.QuestionPrefab, content.transform);
                go.transform.SetParent(content);
                BEID01IntroDialogMJBoxChat boxChat = go.GetComponent<BEID01IntroDialogMJBoxChat>();
                BEID01IntroDialogMJBoxChatData data = new();
                data.audio = listData[i].audioClip;
                data.text = listData[i].text;
                data.type = (i % 2 != 0) ? BEID01IntroDialogMJBoxChatType.Answer : BEID01IntroDialogMJBoxChatType.Question;
                boxChat.InitData(data);
                listBoxChat.Add(boxChat);
                boxChat.SetCanvasAlpha(0);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            for (int i = 0; i < listBoxChat.Count; ++i)
            {
                listBoxChat[i].SetCanvasAlpha(1);
                listBoxChat[i].gameObject.SetActive(false);
                //listBoxChat[i].DisableLayout();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            foreach (var item in listBoxChat)
            {
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(container.GetComponent<RectTransform>().sizeDelta.x, item.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
            }
            var nextButton = dependency.NextButton.GetComponent<RectTransform>();
            nextButton.SetParent(content.transform);
            nextButton.gameObject.SetActive(false);
            nextButton.sizeDelta = new Vector2(container.GetComponent<RectTransform>().sizeDelta.x, nextButton.sizeDelta.y);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

            BEID01IntroDialogMJIntroStateData introStateData = new BEID01IntroDialogMJIntroStateData();
            introStateData.ListBoxChat = listBoxChat;

            BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.INTRO_STATE_START, introStateData);
            ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
        }

        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJInitStateDependency)data;
        }
       private void SetPivot(RectTransform rectTransform, Vector2 pivot)
        {
            Vector2 size = rectTransform.rect.size;
            Vector2 deltaPivot = rectTransform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }
        public override void OnExit()
        {
            base.OnExit();
            cancellationTokenSource?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }
    }
    public class BEID01IntroDialogMJInitStateData
    {
        public List<BEID01IntroDialogMJConversationData> ListData { get; set; }
        public Sprite Background { get; set; }
        public string CharacterName { get; set; }
        public SkeletonDataAsset CharacterAsset { get; set; }
    }
    public class BEID01IntroDialogMJInitStateDependency
    {
        public Transform BackgroundParent { get; set; }
        public GameObject CharactersParent { get; set; }
        public SkeletonGraphic CharatersAnim { get; set; }
        public BEID01MJConversationContainer ConversationContainer { get; set; }
        public GameObject AnswerPrefab { get; set; }
        public GameObject QuestionPrefab { get; set; }
        public Canvas GameplayCanvas { get; set; }
        public GameObject NextButton { get; set; }
        public Image BackgroundImage { get; set; }
        public Image CharatersImage { get; set; }
    }
}