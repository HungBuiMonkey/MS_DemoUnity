using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01Image : BaseImage
    {
        [SerializeField] Image strokeBG;
        [SerializeField] Image insideBG;
        private const string HEXCOLOR_STROKE_CORRECT = "#B5FF00";
        private const string HEXCOLOR_INSIDE_IMAGE_CORRECT = "#74E751";

        private const string HEXCOLOR_STROKE_WRONG = "#D00000";
        private const string HEXCOLOR_INSIDE_IMAGE_WRONG = "#FF5C24";

        private const string HEXCOLOR_STROKE_NORMAL = "#C7B8FF";
        private const string HEXCOLOR_INSIDE_IMAGE_NORMAL = "#8543B3";
        protected override void Awake()
        {
            base.Awake();

        }
        public float SetColor(Image image, Color color, float time)
        {
            Color startColor = image.color;

            StartCoroutine(LerpColorsOverTime(image, startColor, color, time));

            return time;
        }
        public void EnableImage(bool isEnable)
        {
            image.enabled = isEnable;
        }

        public override void SetColor(Color color)
        {
            image.color = color;
        }

        private IEnumerator LerpColorsOverTime(Image image, Color startColor, Color endingColor, float time)
        {
            float inversedTime = 1 / time; // Compute this value **once**
            for (float step = 0.0f; step < 1.0f; step += Time.deltaTime * inversedTime)
            {
                image.color = Color.Lerp(startColor, endingColor, step);
                yield return new WaitForEndOfFrame();
            }
        }
        public void SetColorCorrect()
        {
            strokeBG.color = HexToColor(HEXCOLOR_STROKE_CORRECT);
            insideBG.color = HexToColor(HEXCOLOR_INSIDE_IMAGE_CORRECT);
        }
        public void SetColorWrong()
        {
            strokeBG.color = HexToColor(HEXCOLOR_STROKE_WRONG);
            insideBG.color = HexToColor(HEXCOLOR_INSIDE_IMAGE_WRONG);
        }
        public void SetColorNormal()
        {
            strokeBG.color = HexToColor(HEXCOLOR_STROKE_NORMAL);
            insideBG.color = HexToColor(HEXCOLOR_INSIDE_IMAGE_NORMAL);
        }
        public float FadeColorNormal(float fadeTime)
        {
            SetColor(strokeBG, HexToColor(HEXCOLOR_STROKE_NORMAL), fadeTime);
            return SetColor(insideBG, HexToColor(HEXCOLOR_INSIDE_IMAGE_NORMAL), fadeTime);
        }
        Color HexToColor(string hex)
        {
            Color color = Color.black;
            ColorUtility.TryParseHtmlString(hex, out color);
            return color;
        }

        public override float SetColor(Color color, float time)
        {
            Color startColor = image.color;

            StartCoroutine(LerpColorsOverTime(startColor, color, time));

            return time;
        }
        private IEnumerator LerpColorsOverTime(Color startColor, Color endingColor, float time)
        {
            float inversedTime = 1 / time; // Compute this value **once**
            for (float step = 0.0f; step < 1.0f; step += Time.deltaTime * inversedTime)
            {
                image.color = Color.Lerp(startColor, endingColor, step);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}