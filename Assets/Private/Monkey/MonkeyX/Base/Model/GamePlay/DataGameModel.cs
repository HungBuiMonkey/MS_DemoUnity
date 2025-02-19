

using System.Collections.Generic;
using UnityEngine;

public class DataGameModel
{
    public string JsonConfig { get; private set; }
    public Camera SceenCamera { get; set; }
    public Dictionary<int, DataGamePrimitive> DataGamePrimitiveDict { get; private set; }
    public object DataMore { get; set; }
    public DataGameModel(string jsonConfig, Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict)
    {
        JsonConfig = jsonConfig;
        DataGamePrimitiveDict = dataGamePrimitiveDict;
    }
}

public class DataGamePrimitive
{
    public string Text { get; private set; }
    public int TextID { get; private set; }
    public List<ImageDataGameModel> ImageDataGameModelsList { get; private set; }
    public List<AudioDataGameModel> AudioDataGameModelsList { get; private set; }
    public List<AudioEffectDataGameModel> AudioEffectsGameModelsList { get; private set; }
    public List<VideoDataGameModel> VideoDataGameModelsList { get; private set; }
    public List<PhonicDataGameModel> PhonicPositions { get; private set; }
    public List<AnimationDataGameModel> AnimationDataGameModelsList { get; private set; }
    public WordColorDataGameModel WordColorDataGameModel { get; private set; }
    public TypeWord Type { get; private set; }
  
    public DataGamePrimitive(string text, List<ImageDataGameModel> imageDataGameModelsList, List<AudioDataGameModel> audioDataGameModelsList, TypeWord typeWord)
    {
        Text = text;
        ImageDataGameModelsList = imageDataGameModelsList;
        AudioDataGameModelsList = audioDataGameModelsList;
        Type = typeWord;
    }
    public DataGamePrimitive(string text, int textID, List<ImageDataGameModel> imageDataGameModelsList, 
        List<AudioDataGameModel> audioDataGameModelsList, List<AudioEffectDataGameModel> audioEffectsGameModelsList,
        List<VideoDataGameModel> videoDataGameModelsList,List<PhonicDataGameModel> phonicPositions, 
        List<AnimationDataGameModel> animationDataGameModelsList, WordColorDataGameModel wordColorDataGameModel,
        TypeWord typeWord)
    {
        Text = text;
        TextID = textID;
        ImageDataGameModelsList = imageDataGameModelsList;
        AudioDataGameModelsList = audioDataGameModelsList;
        AudioEffectsGameModelsList = audioEffectsGameModelsList;
        VideoDataGameModelsList = videoDataGameModelsList;
        PhonicPositions = phonicPositions;
        AnimationDataGameModelsList = animationDataGameModelsList;
        WordColorDataGameModel = wordColorDataGameModel;
        Type = typeWord;
    }
}