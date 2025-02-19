using Cysharp.Threading.Tasks;
using Monkey.MJFiveToolTest;
using Monkey.MonkeyBase.NativeBridge;
using MonkeyBase.Observer;
using Newtonsoft.Json;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using static GameModeController;

public class LessonModeController : MonoBehaviour, EventListener<UserActionChanel>, EventListener<EventUserPlayGameChanel>
{
    [SerializeField] ButtonPanelLessonMode buttonLessonPrefab;
    [SerializeField] int amountLesson;
    [SerializeField] RectTransform contentScrollView;
    private GameObject gamePlayObject;
    [SerializeField] ListGameScripableObject listGameScripableObject;

    [SerializeField] GameObject loadingObject;
    [SerializeField] GameObject canvasSelectLesson;
    [SerializeField] GameObject canvasGamePlay;
    private int idLessonSelected;
    private Dictionary<int, ButtonPanelLessonMode> dicButtonPabelLessonMode = new Dictionary<int, ButtonPanelLessonMode>();

    public const string NameListWordFlowFile = "list_word.json";
    public const string NameConfigFlowFile = "config.json";
    public const string NameListWordRuleFlowFile = "list_word_rule.json";
    public const string NameWordFlowFile = "word.json";

    public void OnMMEvent(UserActionChanel eventType)
    {
        if (eventType.TypeData == TypeUserAction.GameModeCloseSelectGame)
        {
            ReactNativeBridge.Instance.SendDataToNative("CloseUnity", string.Empty);
        }
        else if (eventType.TypeData == TypeUserAction.LessonMode_SelectLesson)
        {
            loadingObject.SetActive(true);
           
            DataLessonResult dataLessonResult = (DataLessonResult)(eventType.Data);
            idLessonSelected = dataLessonResult.game_id;
            if (dataLessonResult.game_id != 0)
            {
                GetDataActiviti(dataLessonResult.activity_path, dataLessonResult.word_path, idLessonSelected);
            }
            else
            {
                GameConfig gameConfig = GetGameDefine(idLessonSelected, listGameScripableObject.ListGameConfigs);
                GameObject prefabsGame = Resources.Load<GameObject>(gameConfig.NamePrefabs);
                gamePlayObject = Instantiate(prefabsGame);
                SetActiveCanvasGamePlay(true);
                loadingObject.SetActive(false);
            }
        }
        else if (eventType.TypeData == TypeUserAction.GameModeCloseGame)
        {
            SetActiveCanvasGamePlay(false);
            GameObject.Destroy(gamePlayObject);
        }
    }

    private async void GetDataActiviti(string extractPath, string wordPath, int idGameSelected)
    {
        string pathListWordJson = extractPath + "/" + NameListWordFlowFile;
        string listWordJson = ReadFileJsonFromAppdataFullPath(pathListWordJson);
        List<ListWordModel> listWord = JsonConvert.DeserializeObject<List<ListWordModel>>(listWordJson);

        string pathWordJson = extractPath + "/" + NameWordFlowFile;
        string wordJson = ReadFileJsonFromAppdataFullPath(pathWordJson);
        WordInforRule wordInforRule = JsonConvert.DeserializeObject<WordInforRule>(wordJson);
        List<WordInforDetail> wordInforDetailList = wordInforRule.wordsRule;

        Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict = await GetDataGamePrimitive(wordInforDetailList, listWord, wordPath);

        string pathConfigJson = extractPath + "/" + NameConfigFlowFile;
        string configJson = ReadFileJsonFromAppdataFullPath(pathConfigJson);


        DataGameModel dataGameModel = new DataGameModel(configJson, dataGamePrimitiveDict);
        dataGameModel.SceenCamera = Camera.main;

        GameConfig gameConfig = GetGameDefine(idGameSelected, listGameScripableObject.ListGameConfigs);

        GameObject prefabsGame = Resources.Load<GameObject>(gameConfig.NamePrefabs);
        gamePlayObject = Instantiate(prefabsGame);
        GameManager gameManager = gamePlayObject.GetComponent<GameManager>();
        gameManager.SetData(dataGameModel);
        SetActiveCanvasGamePlay(true);
        loadingObject.SetActive(false);
    }

    private void SetActiveCanvasGamePlay(bool isActive)
    {
        canvasSelectLesson.SetActive(!isActive);
        canvasGamePlay.SetActive(isActive);
    }

    private GameConfig GetGameDefine(int idGame, List<GameConfig> listGameConfigs)
    {
        GameConfig gameDefine = null;

        for (int count = 0; count < listGameConfigs.Count; count++)
        {
            int idGameData = listGameConfigs[count].ID;
            if (idGame.Equals(idGameData))
            {
                gameDefine = listGameConfigs[count];
                break;
            }
        }
        return gameDefine;
    }

    private async Task<Dictionary<int, DataGamePrimitive>> GetDataGamePrimitive(List<WordInforDetail> wordInforDetailList, List<ListWordModel> listWord, string localPathWord)
    {
        Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict = new Dictionary<int, DataGamePrimitive>();
        for (int count = 0; count < wordInforDetailList.Count; count++)
        {
            int idWord = int.Parse(wordInforDetailList[count].IdWord);
            string localPath = GetLocalFileWord(listWord, idWord, localPathWord);
            DataModel.Word wordData = await GetDataWordById(idWord, localPath);


            List<string> idImagesList = wordInforDetailList[count].Idimages;
            List<ImageDataGameModel> imageDataGameModelsList = await GetListImage(localPath, idImagesList, wordData.image);

            List<string> idAudioList = wordInforDetailList[count].IdAudios;
            List<AudioDataGameModel> audioDataGameModelsList = await GetListAudio(localPath, idAudioList, wordData.audio);

            List<string> idAudioEffectList = wordInforDetailList[count].IdEffects;
            List<AudioEffectDataGameModel> audioEffectsDataGameModelsList = await GetListAudioEffect(localPath, idAudioEffectList, wordData.audio_effect);

            WordColorDataGameModel wordColorDataGameModel = GetWordColor(wordData.color);

            List<string> idVideoList = wordInforDetailList[count].IdVideos;
            List<VideoDataGameModel> videoDataGameModelsList = await GetListVideo(localPath, idVideoList, wordData.video);

            TypeWord typeWord = TypeWord.Word;
            typeWord = (TypeWord)listWord.Find(word => word.id == idWord).type;

            int textID = wordData.text_id;

            List<PhonicDataGameModel> phonicDataGameModelList = GetListPhonicPosition(wordData.phonic_position);

            List<AnimationDataGameModel> animationDataGameModelsList = await GetListAnimation(localPath, idWord, wordData.spine);

            DataGamePrimitive dataGamePrimitive = new DataGamePrimitive(wordInforDetailList[count].text, textID, imageDataGameModelsList,
                audioDataGameModelsList, audioEffectsDataGameModelsList, videoDataGameModelsList,
                phonicDataGameModelList, animationDataGameModelsList, wordColorDataGameModel, typeWord);
            dataGamePrimitiveDict.Add(idWord, dataGamePrimitive);
        }
        return dataGamePrimitiveDict;
    }

    private async Task<List<ImageDataGameModel>> GetListImage(string localPath, List<string> idImagesList, List<DataModel.Image> imageList)
    {
        List<ImageDataGameModel> imageDataModelList = new List<ImageDataGameModel>();
        for (int count = 0; count < idImagesList.Count; count++)
        {
            int idImage = int.Parse(idImagesList[count]);
            for (int temp = 0; temp < imageList.Count; temp++)
            {
                DataModel.Image imageModel = imageList[temp];
                if (idImage.Equals(imageModel.id))
                {
                    Sprite sprite = await GetAssetAsync<Sprite>($"{localPath}.bundle", imageModel.file_path);
                    ImageDataGameModel imageDataGameModel = new ImageDataGameModel(imageModel.id, sprite, imageModel.images_categories_id);
                    imageDataModelList.Add(imageDataGameModel);
                }
            }
        }
        return imageDataModelList;
    }

    private async Task<List<AudioDataGameModel>> GetListAudio(string localPath, List<string> idAudioList, List<DataModel.Audio> audioList)
    {
        List<AudioDataGameModel> audioDataModelList = new List<AudioDataGameModel>();
        for (int count = 0; count < idAudioList.Count; count++)
        {
            int idAudio = int.Parse(idAudioList[count]);
            for (int temp = 0; temp < audioList.Count; temp++)
            {
                DataModel.Audio audioModel = audioList[temp];
                if (idAudio.Equals(audioModel.id))
                {
                    AudioClip audioClip = await GetAssetAsync<AudioClip>($"{localPath}.bundle", audioModel.file_path);
                    AudioDataGameModel audioDataGameModel = new AudioDataGameModel(audioModel.id, audioClip, audioModel.duration,
                        audioModel.sync_data, audioModel.voices_id, audioModel.voices_type_id, audioModel.score);
                    audioDataModelList.Add(audioDataGameModel);
                }
            }
        }
        return audioDataModelList;
    }

    private async Task<List<AudioEffectDataGameModel>> GetListAudioEffect(string localPath, List<string> idAudioList, List<DataModel.Audio> audioList)
    {
        List<AudioEffectDataGameModel> audioEffectDataModelList = new List<AudioEffectDataGameModel>();
        for (int count = 0; count < idAudioList.Count; count++)
        {
            int idAudio = int.Parse(idAudioList[count]);
            for (int temp = 0; temp < audioList.Count; temp++)
            {
                DataModel.Audio audioModel = audioList[temp];
                if (idAudio.Equals(audioModel.id))
                {
                    AudioClip audioClip = await GetAssetAsync<AudioClip>($"{localPath}.bundle", audioModel.file_path);
                    AudioEffectDataGameModel audioEffectDataGameModel = new AudioEffectDataGameModel(audioModel.id, audioClip, audioModel.duration,
                        audioModel.sync_data, audioModel.voices_id, audioModel.voices_type_id, audioModel.score);
                    audioEffectDataModelList.Add(audioEffectDataGameModel);
                }
            }
        }
        return audioEffectDataModelList;
    }
    private WordColorDataGameModel GetWordColor(DataModel.ColorWord color)
    {
        WordColorDataGameModel wordColorDataGameModel = new WordColorDataGameModel(color.text, color.highlight);
        return wordColorDataGameModel;
    }

    private async Task<List<VideoDataGameModel>> GetListVideo(string localPath, List<string> idVideoList, List<DataModel.Video> videoList)
    {
        List<VideoDataGameModel> videoDataModelList = new List<VideoDataGameModel>();
        for (int count = 0; count < idVideoList.Count; count++)
        {
            int idVideo = int.Parse(idVideoList[count]);
            for (int temp = 0; temp < videoList.Count; temp++)
            {
                DataModel.Video videoModel = videoList[temp];
                if (idVideo.Equals(videoModel.id))
                {
                    VideoClip videoClip = await GetAssetAsync<VideoClip>($"{localPath}.bundle", videoModel.link);
                    VideoDataGameModel videoDataGameModel = new VideoDataGameModel(videoModel.id, videoClip, videoModel.duration, videoModel.description,
                        videoModel.tag_title, videoModel.video_category_id, videoModel.interactive);
                    videoDataModelList.Add(videoDataGameModel);
                }
            }
        }
        return videoDataModelList;
    }

    private List<PhonicDataGameModel> GetListPhonicPosition(List<DataModel.PhonicPosition> phonicsPosition)
    {
        List<PhonicDataGameModel> phonicDataGameModelList = new List<PhonicDataGameModel>();

        if (phonicsPosition != null)
        {
            for (int i = 0; i < phonicsPosition.Count; i++)
            {
                DataModel.PhonicPosition phonicPosition = phonicsPosition[i];
                string name = phonicPosition.name;
                string position = phonicPosition.position;
                PhonicDataGameModel phonicDataGameModel = new PhonicDataGameModel(name, position);
                phonicDataGameModelList.Add(phonicDataGameModel);
            }
        }
        return phonicDataGameModelList;
    }

    private async Task<List<AnimationDataGameModel>> GetListAnimation(string localPath, int idWord, List<DataModel.Spine> spineList)
    {
        List<AnimationDataGameModel> videoDataModelList = new List<AnimationDataGameModel>();
        if (spineList.Count > 0)
        {
            SkeletonDataAsset spineAsset = await GetAssetAsync<SkeletonDataAsset>($"{localPath}.bundle", "BE_SkeletonData.asset");
            AnimationDataGameModel animationDataGameModel = new(idWord, spineAsset);
            videoDataModelList.Add(animationDataGameModel);
        }
        return videoDataModelList;
    }
    private async Task<AssetBundle> LoadAssetBundleAsync(string assetBundlePath)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);

        await request;

        if (request.assetBundle == null)
        {
            Debug.LogError($"Không thể tạo AssetBundle từ file tại: {assetBundlePath}");
            return null;
        }

        return request.assetBundle;
    }

    private async Task<T> GetAssetAsync<T>(string assetBundlePath, string nameAsset) where T : UnityEngine.Object
    {
        AssetBundle assetBundle = await LoadAssetBundleAsync(assetBundlePath);

        if (assetBundle == null)
        {
            Debug.LogError($"Không thể tải AssetBundle từ: {assetBundlePath}");
            return null;
        }

        T asset = assetBundle.LoadAsset<T>(nameAsset);
        if (asset != null)
        {
            assetBundle.Unload(false);
            return asset;
        }
        else
        {
            Debug.LogError($"Không thể tìm thấy asset có tên: {nameAsset}");
            assetBundle.Unload(false);
            return null;
        }
    }

    private async Task<DataModel.Word> GetDataWordById(int idWord, string localPath)
    {
        TextAsset textAsset = await GetAssetAsync<TextAsset>($"{localPath}.bundle", $"{idWord}.json");
        string dataJson = textAsset.text;
        if (!string.IsNullOrEmpty(dataJson))
        {
            var word = JsonConvert.DeserializeObject<DataModel.Word>(dataJson.ToString());
            return word;
        }
        return null;
    }

    private string GetLocalFileWord(List<ListWordModel> listWord, int idWord, string extractPath)
    {
        string localFile = string.Empty;
        for (int count = 0; count < listWord.Count; count++)
        {
            ListWordModel listWordModel = listWord[count];
            if (listWordModel.id == idWord)
            {
                localFile = $"{extractPath}/{GetNameFileFromUrl(listWordModel.path, ".zip")}";
                break;
            }
        }

        return localFile;
    }

    private string GetNameFileFromUrl(string url, string extentionFile)
    {
        string[] words = url.Split('/');
        string nameFile = words[words.Length - 1];
        nameFile = nameFile.Replace(extentionFile, "");
        return nameFile;
    }

    private string ReadFileJsonFromAppdataFullPath(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            return "";
        }

        return File.ReadAllText(path);
    }

    private void Start()
    {
        ReactNativeBridge.Instance.RegisterActionForRequestFromReactNative(RegisterAction);
        this.ObserverStartListening<UserActionChanel>();
        this.ObserverStartListening<EventUserPlayGameChanel>();       
    }

    private void OnDestroy()
    {
        this.ObserverStopListening<UserActionChanel>();
        this.ObserverStopListening<EventUserPlayGameChanel>();
    }

    public void OnMMEvent(EventUserPlayGameChanel eventType)
    {
        if (eventType.EventName == EventUserPlayGameChanel.UserEvent.FinishGame)
        {
            SetActiveCanvasGamePlay(false);
            GameObject.Destroy(gamePlayObject);

            LessonStatus dataFinishLessonRequest = new LessonStatus();
            dataFinishLessonRequest.lesson_id = idLessonSelected;
            dataFinishLessonRequest.status = (int)ButtonPanelLessonMode.State.Finish;
            string data = JsonUtility.ToJson(dataFinishLessonRequest);
            ReactNativeBridge.Instance.SendDataToNative("FinishLesson", data, CallBack);
        }
    }

   

    private void CallBack(object data)
    {
        Payload payload = (Payload)data;
        if (payload.Success)
        {
            string jsonResult = payload.Result;
            DataFinishLessonResult dataFinishLessonResult = JsonConvert.DeserializeObject<DataFinishLessonResult>(jsonResult);
            List<LessonStatus> listLesson = dataFinishLessonResult.list_lesson;
            for(int count = 0; count < listLesson.Count; count++)
            {
                LessonStatus lessonStatus = listLesson[count];
                int idLesson = lessonStatus.lesson_id;
                if(dicButtonPabelLessonMode.ContainsKey(idLesson))
                {
                    ButtonPanelLessonMode buttonPanelLessonMode = dicButtonPabelLessonMode[idLesson];
                    ButtonPanelLessonMode.State state = (ButtonPanelLessonMode.State)lessonStatus.status;
                    buttonPanelLessonMode.SetData(state);
                }
            }    
        }
    }

    private void RegisterAction(string data)
    {
        ReceivedData receivedData = JsonConvert.DeserializeObject<ReceivedData>(data);
        if(receivedData.Type.Equals("open_map"))
        {
            DataFinishLessonResult dataFinishLessonResult = JsonConvert.DeserializeObject<DataFinishLessonResult>(receivedData.Payload);
            List<LessonStatus> listLesson = dataFinishLessonResult.list_lesson;
            for (int count = 0; count < listLesson.Count; count++)
            {
                LessonStatus lessonStatus = listLesson[count];
                int idLesson = lessonStatus.lesson_id;
                if (dicButtonPabelLessonMode.Count <= 0)
                {
                    ButtonPanelLessonMode buttonPanelGameMode = Instantiate<ButtonPanelLessonMode>(this.buttonLessonPrefab);
                    buttonPanelGameMode.transform.SetParent(contentScrollView);
                    buttonPanelGameMode.transform.localScale = Vector3.one;
                    buttonPanelGameMode.SetData($"Lesson {count + 1}", idLesson, ButtonPanelLessonMode.State.Lock);
                    dicButtonPabelLessonMode.Add(idLesson, buttonPanelGameMode);
                }
                else
                {
                    if (dicButtonPabelLessonMode.ContainsKey(idLesson))
                    {
                        ButtonPanelLessonMode buttonPanelLessonMode = dicButtonPabelLessonMode[idLesson];
                        ButtonPanelLessonMode.State state = (ButtonPanelLessonMode.State)lessonStatus.status;
                        buttonPanelLessonMode.SetData(state);
                    }
                }    
            }
        }
        else if (receivedData.Type.Equals("orientation"))
        {
            Orientation orientation = JsonConvert.DeserializeObject<Orientation>(receivedData.Payload);
            Orientation(orientation.orientation);
        }    
    }

    private void Orientation(string message)
    {      
        switch (message)
        {
            case "PORTRAIT":

                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case "LANDSCAPE-LEFT":
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            case "LANDSCAPE-RIGHT":
                Screen.orientation = ScreenOrientation.LandscapeRight;
                break;
            default:
                Screen.orientation = ScreenOrientation.AutoRotation;
                return;
        }
    }
}

public class DataFinishLessonResult
{
    public List<LessonStatus> list_lesson;
}
public class LessonStatus
{

    public int lesson_id;
    public int status;
}

public class Orientation
{
    public string orientation;
}

