using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSLoopBackground : MonoBehaviour
    {
        public RectTransform background1;
        public RectTransform background2;
        private float speed = 400f;

        private float backgroundHeight;
        private bool isLooping = false;
        private void Start()
        {
            backgroundHeight = background1.rect.height;

            background2.anchoredPosition = new Vector2(0, -backgroundHeight);
        }

        private void Update()
        {
            if (!isLooping) return;

            // Di chuyển background xuống theo `scrollSpeed`
            background1.anchoredPosition += Vector2.down * speed * Time.deltaTime;
            background2.anchoredPosition += Vector2.down * speed * Time.deltaTime;

            // Kiểm tra nếu background1 di chuyển hết chiều cao của nó, reset lại vị trí
            if (background1.anchoredPosition.y <= -backgroundHeight)
            {
                background1.anchoredPosition = new Vector2(0, background2.anchoredPosition.y + backgroundHeight);
            }

            // Kiểm tra nếu background2 di chuyển hết chiều cao của nó, reset lại vị trí
            if (background2.anchoredPosition.y <= -backgroundHeight)
            {
                background2.anchoredPosition = new Vector2(0, background1.anchoredPosition.y + backgroundHeight);
            }
        }
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed; 
        }
        public void StartLoop()
        {
            isLooping = true;
        }

        public void StopLoop()
        {
            isLooping = false;
        }

    }
}