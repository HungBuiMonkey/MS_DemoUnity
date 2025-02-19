using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    [CreateAssetMenu(fileName = "BEPS01RFSMookData", menuName = "ScriptableObjects/BEPS01Re_FuelingSpacecraft/MookData", order = 1)]

    public class BEPS01RFSMookData : ScriptableObject
    {
        public List<BEPS01RFSConversationData> listData;
    }
}