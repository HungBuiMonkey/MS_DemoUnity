using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    [CreateAssetMenu(fileName = "BEPS02FlyingOwlsMookDataScriptableObject", menuName = "ScriptableObjects/BEPS02FlyingOwls/MockData", order = 1)]
    public class BEPS02FlyingOwlsMookDataScriptableObject : ScriptableObject
    {
        public List<BEPS02PlayConversationData> listData;
    }
}