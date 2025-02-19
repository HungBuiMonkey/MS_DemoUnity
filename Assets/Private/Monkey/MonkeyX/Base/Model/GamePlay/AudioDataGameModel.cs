
using System.Collections.Generic;
using UnityEngine;

public class AudioDataGameModel 
{
    public int ID { get; private set; }
    public AudioClip AudioClip { get; private set; }
    public int Duration { get; private set; }
    public List<DataModel.SyncData> SyncData { get; private set; }
    public string VoiceID { get; private set; }
    public int VoiceTypeID { get; private set; }
    public int Score { get; private set; }

    public AudioDataGameModel(int iD, AudioClip audioClip, int duration, List<DataModel.SyncData> syncData, string voiceID, int voiceTypeID, int score)
    {
        ID = iD;
        AudioClip = audioClip;
        Duration = duration;
        SyncData = syncData;
        VoiceID = voiceID;
        VoiceTypeID = voiceTypeID;
        Score = score;
    }
}
