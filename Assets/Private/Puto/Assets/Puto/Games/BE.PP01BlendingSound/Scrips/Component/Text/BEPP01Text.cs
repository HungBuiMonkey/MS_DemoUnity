using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01Text : BaseText
    {
        protected ContentSizeFitter contentSizeFitter;
        [SerializeField] TMP_Text text;
        [SerializeField] RectTransform underline;
        private List<int> indexes;
        private int indexLine;
        private BEPP01AnswerData data;
        protected override void Awake()
        {
            base.Awake();
            contentSizeFitter = GetComponent<ContentSizeFitter>();

        }
        public override void SetText(string content)
        {
            labeText.text = content;
            text.ForceMeshUpdate();
        }

        void OnEnable()
        {
            StartCoroutine(IEOnEnable());
        }

        public void SetData(BEPP01AnswerData data, int indexLine, List<int> indexes)
        {
            this.data = data;
            this.indexes = indexes;
            this.indexLine = indexLine;
        }
        public void SetData(BEPP01AnswerData data)
        {
            this.data = data;
        }
        public BEPP01AnswerData GetData()
        {
            return data;
        }

        public List<int> GetIndexes()
        {
            return indexes;
        }
        public int GetIndexLine()
        {
            return indexLine;
        }

        IEnumerator IEOnEnable()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);
            yield return new WaitForEndOfFrame();
            //ActiveLayout(false);
        }

        private void ActiveLayout(bool isActive)
        {
            if (contentSizeFitter != null)
                contentSizeFitter.enabled = isActive;
        }

        public override void SetText(string content, float maxWidth)
        {
            ActiveLayout(true);
            StartCoroutine(IESetText(content, maxWidth));
        }

        IEnumerator IESetText(string content, float maxWidth)
        {
            labeText.text = content;
            yield return new WaitForEndOfFrame();
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform.sizeDelta.x > maxWidth)
            {
                rectTransform.sizeDelta = new Vector2(maxWidth, 0);
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
            else
                contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        public string GetText()
        {
            return labeText.text;
        }
        public void FadeText(float toAlpha, float time, Action callback = null)
        {
            DOTween.To(() => labeText.color.a, alpha =>
            {
                var color = labeText.color;
                color.a = alpha;
                labeText.color = color;
            }, toAlpha, time).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        public void ChangeColorText()
        {
            labeText.color = Color.green;
        }
        public void ResetColorText()
        {
            labeText.color = Color.white;
        }
    }
}