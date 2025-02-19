using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJFiveToolTest
{
    [CreateAssetMenu(fileName = "ConfigGame", menuName = "ScriptableObjects/MJ5/ConfigGame")]
    public class ListGameScripableObject : ScriptableObject
    {
        public List<GameConfig> ListGameConfigs;
        public List<ButtonColor> buttonColors;
    }

    [Serializable]
    public class GameConfig
    {
        public string Name;
        public string Code;
        public int ID;
        public string NamePrefabs;
        public bool IsActive;
        public ButtonColorKey colorKey;
    }

    public enum ButtonColorKey
    {
        Live,
        Done,
        POCheck,
        QATest,
        Doing
    }

    [Serializable]
    public class ButtonColor
    {
        public ButtonColorKey colorKey;
        public Color colorBg;
        public Color colorTitle;
        public Color colorID;
    }


}
