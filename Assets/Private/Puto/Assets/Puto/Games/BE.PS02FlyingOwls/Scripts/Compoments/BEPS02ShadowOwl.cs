using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02ShadowOwl : MonoBehaviour
    {
        private bool isDraged = false;

        public bool IsDraged {
            set { isDraged = value; }
            get { return isDraged; }
        
        }
    }
}