using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02WoodenItem : MonoBehaviour
    {
        [SerializeField] Image woodenLong;
        [SerializeField] Image woodenShort;

        public void SetColor(Color32 color)
        {
            woodenLong.color = color;
            woodenShort.color = color;
        }
    }
}