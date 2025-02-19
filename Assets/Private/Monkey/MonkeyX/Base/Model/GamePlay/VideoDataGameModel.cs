
using System.Collections.Generic;
using UnityEngine.Video;

public class VideoDataGameModel
{
    public int ID { get; private set; }

    public VideoClip VideoClip { get; private set; }
    public int Duration { get; private set; }
    public string Description { get; private set; }
    public string TagTitle { get; private set; }
    public int VideoCategoriesID { get; private set; }

    public List<DataModel.VideoInteractive> Interactive { get; private set; }
    public VideoDataGameModel(int iD, VideoClip videoClip, int duration, string description, string tagTitle, int videoCategoriesID, List<DataModel.VideoInteractive> interactive)
    {
        ID = iD;
        VideoClip = videoClip;
        Duration = duration;
        Description = description;
        TagTitle = tagTitle;
        VideoCategoriesID = videoCategoriesID;
        Interactive = interactive;
    }
}
