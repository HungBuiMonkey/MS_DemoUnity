using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02TestSelectLever : MonoBehaviour
    {
        public BEPS02FlyingOwlsMookDataScriptableObject mookData;
        public bool isPlayGame = false;
        public List<Button> listButton;
        private string strLever = "";


        public void GetLever(string value)
        {
            strLever = value;
        }

        public void OnCloseSelect()
        {
            if (strLever.Contains("Kho"))
            {
                foreach(var item in mookData.listData)
                {
                    item.isDifficult = true;
                }
            }
            else
            {
                foreach (var item in mookData.listData)
                {
                    item.isDifficult = false;
                }
            }
            isPlayGame = true;
            BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.InitData, null);
            gameObject.transform.parent.localScale = Vector3.zero;
        }
    }
}