using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    [CreateAssetMenu(fileName = "BERV01MookData", menuName = "ScriptableObjects/BERV01FeedTheShark/MookData", order = 1)]

    public class BERV01FTSMookData : ScriptableObject
    {
        public BERV01FTSGamePlayData mookData;
    }
}