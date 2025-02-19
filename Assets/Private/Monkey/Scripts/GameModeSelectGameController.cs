using BestHTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Monkey.MJFiveToolTest
{
    public class GameModeSelectGameController : MonoBehaviour
    {
        [SerializeField] ButtonPanelGameMode buttonPanelGameMode;
        [SerializeField] ListGameScripableObject listGameScripableObject;
        [SerializeField] Transform scrollContent;
        private void Start()
        {
            List<GameConfig> listGameConfigs = listGameScripableObject.ListGameConfigs;
            for (int count = 0; count < listGameConfigs.Count; count++)
            {
                GameConfig gameConfig = listGameConfigs[count];
                if (gameConfig.IsActive)
                {
                    ButtonPanelGameMode buttonPanelGameMode = Instantiate<ButtonPanelGameMode>(this.buttonPanelGameMode);
                    buttonPanelGameMode.transform.SetParent(scrollContent);
                    buttonPanelGameMode.transform.localScale = Vector3.one;
                    buttonPanelGameMode.SetData(gameConfig.Name, gameConfig.Code, gameConfig.ID, gameConfig.colorKey, listGameScripableObject.buttonColors);                    
                }
            }
        }     

    }
}
