using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSDashBoxItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        private int id;
        private bool isSelected;
        private void Start()
        {
            isSelected = false;
        }

        public void InitData(int id)
        {
            this.id = id;
        }

        public int GetId()
        {
            return id;
        }
        public bool GetSelected()
        {
            return isSelected;
        }
        public void SetSelected(bool value)
        {
            isSelected = value;
        }
        public void Fade(float value)
        {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, value);
        }
        public float GetAlpha()
        {
            return image.color.a;
        }
    }
}