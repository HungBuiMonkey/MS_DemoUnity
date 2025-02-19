using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSTestPlayData : MonoBehaviour
    {
        [SerializeField] private BEPS01RFSMookData mookDataSO;
        public bool isPlayGame = false;
        private string strLever = "";
        public void GetLever(string value)
        {
            strLever = value;
        }

        public void OnCloseSelect()
        {
            if (strLever.Contains("Kho"))
            {
                foreach (var item in mookDataSO.listData)
                {
                    item.isDifficult = true;
                }
            }
            else
            {
                foreach (var item in mookDataSO.listData)
                {
                    item.isDifficult = false;
                }
            }
            isPlayGame = true;
            gameObject.transform.parent.localScale = Vector3.zero;
            BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.InitData, null);
        }
    }
}
