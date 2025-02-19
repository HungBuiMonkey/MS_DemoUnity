using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSFishSwimmingLane : MonoBehaviour
    {
        [SerializeField] RectTransform pointLeft; 
        [SerializeField] RectTransform pointRight;
        private int index;

        public int Index {
            set { index = value; }
            get { return index; }
        }
        public Transform PointLeft
        {
            get { return pointLeft; }
        }
        public Transform PointRight
        {
            get { return pointRight; }
        }

        public void SetPositionLeftX(float valueX)
        {
            pointLeft.anchoredPosition = new Vector2(-valueX, 0);
        }
        public void SetPositionRightX(float valueX)
        {
            pointRight.anchoredPosition = new Vector2(valueX, 0);
        }

    }
}