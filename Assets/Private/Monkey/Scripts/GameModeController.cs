using BestHTTP;
using Cysharp.Threading.Tasks;
using Monkey.MJFiveToolTest;
using MonkeyBase.Observer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class GameModeController : MonoBehaviour, EventListener<UserActionChanel>
{
    private int idGameSelected;
    [SerializeField] GameObject loadingObject;
    [SerializeField] ListGameScripableObject listGameScripableObject;
    [SerializeField] GameObject canvasSelectGame;
    [SerializeField] GameObject canvasGamePlay;
    private GameObject gamePlayObject;
    private void OnEnable()
    {
        this.ObserverStartListening<UserActionChanel>();
    }
    public void OnMMEvent(UserActionChanel eventType)
    {
        if(eventType.TypeData == TypeUserAction.GameModeCloseSelectGame)
        {

        }
        else if(eventType.TypeData == TypeUserAction.GameMode_SelectGame)
        {
            loadingObject.SetActive(true);
            idGameSelected = (int)eventType.Data;
            if (idGameSelected != 0)
            {
                string linkLocalActivity = PlayerPrefs.GetString(idGameSelected.ToString());
                bool isDownloadedResource = linkLocalActivity != string.Empty;

                if (!isDownloadedResource)
                {
                    Debug.LogError("downloading");
                    string link = $"https://api.dev.monkeyuni.com/platform_go/api/v1/activity?course_id=201&game_ids={idGameSelected}";
                    StartCoroutine(GetRequest(link));
                }
                else
                {
                    Debug.LogError("downloaded");
                    GetDataGame(linkLocalActivity);
                }
            }
            else
            {
                GameConfig gameConfig = GetGameDefine(idGameSelected, listGameScripableObject.ListGameConfigs);
                Debug.LogError(gameConfig.NamePrefabs);
                GameObject prefabsGame = Resources.Load<GameObject>(gameConfig.NamePrefabs);
                gamePlayObject = Instantiate(prefabsGame);
            }    
        }
        else if (eventType.TypeData == TypeUserAction.GameModeCloseGame)
        {
            SetActiveCanvasGamePlay(false);
            GameObject.Destroy(gamePlayObject);
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    JObject gameConfigObj = JObject.Parse(webRequest.downloadHandler.text);
                    if (gameConfigObj.ContainsKey("data"))
                    {
                        string data = gameConfigObj.GetValue("data").ToString();
                        if (!string.IsNullOrEmpty(data))
                        {
                            List<InforActivity> listInforActivity = JsonConvert.DeserializeObject<List<InforActivity>>(data);
                            InforActivity inforActivity = listInforActivity[0];
                            GetDataActivity(inforActivity.linkDownLoad);
                            Debug.LogError(inforActivity.linkDownLoad);
                        }
                    }

                    break;
            }
        }
    }

    private void GetDataActivity(string activityLink)
    {
        string downloadActivityUrl = "https://vnmedia2.monkeyuni.net/";
        string url = downloadActivityUrl + activityLink;
        string savePath = "zips/game/activities";
        string extractPath = "extract/game/activities";
        string extractPathZip = extractPath + "/" + GetNameFileFromUrl(url, ".zip");
        CreateDirectory(savePath);
        CreateDirectory(extractPathZip);

        string localPathFile = $"{Application.persistentDataPath}/{savePath}/{Path.GetFileName(url)}";

        string localPathExtractFile = $"{Application.persistentDataPath}/{extractPath}/{GetNameFileFromUrl(url, ".zip")}";
        Debug.LogError(localPathExtractFile);
        DownloadWithBestHttp(url, localPathFile, localPathExtractFile, DownloadActivitySuccessCallback, DownloadActivityProgressCallback, DownloadActivityErrorCallback);
    }

    private void SetActiveCanvasGamePlay(bool isActive)
    {
        canvasSelectGame.SetActive(!isActive);
        canvasGamePlay.SetActive(isActive);
    }

    private int DOWNLOAD_MAX_RETRY_COUNT = 3;
    public const string NameListWordFlowFile = "list_word.json";
    public const string NameConfigFlowFile = "config.json";
    public const string NameListWordRuleFlowFile = "list_word_rule.json";
    public const string NameWordFlowFile = "word.json";
    private void DownloadWithBestHttp(string url, string localPathFile, string localPathExtractFile, Action<object> successCallback, Action<object> progressCallback, Action<object> errorCallback)
    {
        int retry_count = 0;
        float timeStart = Time.realtimeSinceStartup;

        string filePath = localPathFile;
        string filePathTemp = localPathFile + "_" + UnityEngine.Random.Range(1, 100000) + ".downloading";


        if (!System.IO.File.Exists(filePath))
        {
            var file = new System.IO.FileInfo(filePath);
            file.Directory.Create();
        }
        else
        {
            File.Delete(filePath);
        }
        if (File.Exists(filePathTemp))
        {
            File.Delete(filePathTemp);
        }


        var request = new HTTPRequest(new Uri(url), (req, resp) =>
        {
            var fs = req.Tag as System.IO.FileStream;
            if (fs != null)
                fs.Dispose();
            FileInfo file = null;
            if (File.Exists(filePathTemp))
            {
                file = new FileInfo(filePathTemp);
            }
            if (resp == null || file == null)
            {
                LogMe.Log($"<color=red>Download Failed: {url}</color>");
                if (retry_count < DOWNLOAD_MAX_RETRY_COUNT)
                {
                    retry_count++;
                    DownloadWithBestHttp(url, localPathFile, localPathExtractFile, DownloadActivitySuccessCallback, DownloadActivityProgressCallback, DownloadActivityErrorCallback);
                }
                else
                {
                    errorCallback?.Invoke(req.State);
                }
                return;
            }
            switch (req.State)
            {
                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        if (file != null)
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            file.MoveTo(filePath);
                        }

                        if (!string.IsNullOrEmpty(localPathExtractFile))
                        {
                            Action on_extract_error = () =>
                            {
                                file.Delete();
                            };

                            Action on_extract_success = () =>
                            {
                            };

                            ZipUtil.Extract(localPathFile, localPathExtractFile, on_extract_success, on_extract_error);
                        }
                        successCallback?.Invoke(localPathExtractFile);
                    }
                    else
                    {
                        if (file != null)
                            file.Delete();
                        Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText));
                    }
                    break;
                default:
                    if (retry_count < DOWNLOAD_MAX_RETRY_COUNT)
                    {
                        retry_count++;
                        DownloadWithBestHttp(url, localPathFile, localPathExtractFile, DownloadActivitySuccessCallback, DownloadActivityProgressCallback, DownloadActivityErrorCallback);
                    }
                    else
                    {
                        if (file != null)
                            file.Delete();
                        errorCallback?.Invoke(req.State);
                    }
                    break;
            }

            HttpStatusCode httpStatusCode = HttpStatusCode.OK;

            if (req.State != HTTPRequestStates.Finished)
            {
                HttpWebResponse response = new HttpWebResponse();
                httpStatusCode = (HttpStatusCode)resp.StatusCode;
            }


        });

        request.OnDownloadProgress += (originalRequest, progress, length) =>
        {
            progressCallback?.Invoke(Mathf.CeilToInt(progress * 100f / length));
        };
        request.OnStreamingData += (req, resp, dataFragment, dataFragmentLength) =>
        {
            if (resp.IsSuccess)
            {
                var fs = req.Tag as System.IO.FileStream;
                if (fs == null)
                    req.Tag = fs = new System.IO.FileStream(filePathTemp, System.IO.FileMode.OpenOrCreate);

                fs.Write(dataFragment, 0, dataFragmentLength);
            }
            // Return true if dataFragment is processed so the plugin can recycle it
            return true;
        };

        request.Send();


    }

    private async void DownloadActivitySuccessCallback(object extractPath)
    {
        string pathListWordJson = extractPath + "/" + NameListWordFlowFile;
        string listWordJson = ReadFileJsonFromAppdataFullPath(pathListWordJson);
        List<ListWordModel> listWord = JsonConvert.DeserializeObject<List<ListWordModel>>(listWordJson);
        await DownLoadListWord(listWord);
        
        
        string pathWordJson = extractPath + "/" + NameWordFlowFile;
        string wordJson = ReadFileJsonFromAppdataFullPath(pathWordJson);
        WordInforRule wordInforRule = JsonConvert.DeserializeObject<WordInforRule>(wordJson);
        List<WordInforDetail> wordInforDetailList = wordInforRule.wordsRule;

        Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict = await GetDataGamePrimitive(wordInforDetailList, listWord);

        
        string pathConfigJson = extractPath + "/" + NameConfigFlowFile;
        string configJson = ReadFileJsonFromAppdataFullPath(pathConfigJson);


        DataGameModel dataGameModel = new DataGameModel(configJson, dataGamePrimitiveDict);
        dataGameModel.SceenCamera = Camera.main;
        

        
        GameConfig gameConfig = GetGameDefine(idGameSelected, listGameScripableObject.ListGameConfigs);

        GameObject prefabsGame = Resources.Load<GameObject>(gameConfig.NamePrefabs);
        gamePlayObject = Instantiate(prefabsGame);
        GameManager gameManager = gamePlayObject.GetComponent<GameManager>();
        gameManager.SetData(dataGameModel);       
        loadingObject.SetActive(false);
        SetActiveCanvasGamePlay(true);
        PlayerPrefs.SetString(idGameSelected.ToString(), extractPath.ToString());
        PlayerPrefs.Save();       
    }

    private async void GetDataGame(string extractPath)
    {
        string pathListWordJson = extractPath + "/" + NameListWordFlowFile;
        string listWordJson = ReadFileJsonFromAppdataFullPath(pathListWordJson);
        List<ListWordModel> listWord = JsonConvert.DeserializeObject<List<ListWordModel>>(listWordJson);

        string pathWordJson = extractPath + "/" + NameWordFlowFile;
        string wordJson = ReadFileJsonFromAppdataFullPath(pathWordJson);
        WordInforRule wordInforRule = JsonConvert.DeserializeObject<WordInforRule>(wordJson);
        List<WordInforDetail> wordInforDetailList = wordInforRule.wordsRule;

        Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict = await GetDataGamePrimitive(wordInforDetailList, listWord);


        string pathConfigJson = extractPath + "/" + NameConfigFlowFile;
        string configJson = ReadFileJsonFromAppdataFullPath(pathConfigJson);


        DataGameModel dataGameModel = new DataGameModel(configJson, dataGamePrimitiveDict);
        dataGameModel.SceenCamera = Camera.main;



        GameConfig gameConfig = GetGameDefine(idGameSelected, listGameScripableObject.ListGameConfigs);

        GameObject prefabsGame = Resources.Load<GameObject>(gameConfig.NamePrefabs);
        gamePlayObject = Instantiate(prefabsGame);
        GameManager gameManager = gamePlayObject.GetComponent<GameManager>();
        gameManager.SetData(dataGameModel);
        loadingObject.SetActive(false);
        SetActiveCanvasGamePlay(true);
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
    private void DownloadActivityProgressCallback(object error)
    {

    }
    private void DownloadActivityErrorCallback(object error)
    {

    }

    private void OnDisable()
    {
        this.ObserverStopListening<UserActionChanel>();
    }

    
    private async Task<Dictionary<int, DataGamePrimitive>> GetDataGamePrimitive(List<WordInforDetail> wordInforDetailList, List<ListWordModel> listWord)
    {
        Dictionary<int, DataGamePrimitive> dataGamePrimitiveDict = new Dictionary<int, DataGamePrimitive>();
        for (int count = 0; count < wordInforDetailList.Count; count++)
        {
            int idWord = int.Parse(wordInforDetailList[count].IdWord);
            string localPath = GetLocalFileWord( listWord, idWord);
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

    

    private string GetLocalFileWord(List<ListWordModel> listWord, int idWord)
    {
        string extractPath = "zips/game/words";
        string localFile = string.Empty;
        for(int count = 0; count < listWord.Count; count++)
        {
            ListWordModel listWordModel = listWord[count];
            if (listWordModel.id == idWord)
            {
                localFile = $"{Application.persistentDataPath}/{extractPath}/{GetNameFileFromUrl(listWordModel.path, ".zip")}";
                break;
            }
        }
       
        return localFile;
    }    
    
    private async Task DownLoadListWord(List<ListWordModel> listWord)
    {
        string rootUrl = "https://vnmedia2.monkeyuni.net/App/zip/hdr/word_bundle/ios/";
#if UNITY_IOS
        rootUrl = "https://vnmedia2.monkeyuni.net/App/zip/hdr/word_bundle/ios/";
#elif UNITY_ANDROID
        rootUrl = "https://vnmedia2.monkeyuni.net/App/zip/hdr/word_bundle/android/";
#endif
        string savePath = "zips/game/words";

        CreateDirectory(savePath);
        int amountWord = listWord.Count;
        for (int count = 0; count < listWord.Count; count++)
        {
            ListWordModel listWordModel = listWord[count];
            string url = rootUrl + listWordModel.path_bundle;
            string localPathFile = $"{Application.persistentDataPath}/{savePath}/{Path.GetFileName(url)}";
            DownloadWithBestHttp(url, localPathFile, null, (objectReturn) => { amountWord--; }, null, null);
        }
        await UniTask.WaitUntil(() => amountWord <= 0);
    }    

    [Serializable]
    public class ListWordModel
    {
        public int id;
        public string path;
        public string path_bundle;
        public int type;
        public int version;
    }

    public class WordInforRule
    {
        [JsonProperty("story_id")] public string StoryID { get; set; }
        [JsonProperty("word")] public List<WordInforDetail> wordsRule { get; set; }
    }

    public class WordInforDetail
    {
        [JsonProperty("id")] public string IdWord { get; set; }
        [JsonProperty("text")] public string text { get; set; }
        [JsonProperty("images")] public List<string> Idimages { get; set; }
        [JsonProperty("videos")] public List<string> IdVideos { get; set; }
        [JsonProperty("audios")] public List<string> IdAudios { get; set; }
        [JsonProperty("effects")] public List<string> IdEffects { get; set; }
        [JsonProperty("animations")] public List<string> IdAnimations { get; set; }
    }

    private string GetNameFileFromUrl(string url, string extentionFile)
    {
        string[] words = url.Split('/');
        string nameFile = words[words.Length - 1];
        nameFile = nameFile.Replace(extentionFile, "");
        return nameFile;
    }

    public static void CreateDirectory(string pathToStore)
    {
        var baseStorePath = $"{Application.persistentDataPath}/{pathToStore}";

        if (!Directory.Exists(baseStorePath))
        {
            Directory.CreateDirectory(baseStorePath);
        }
    }

    private string ReadFileJsonFromAppdataFullPath(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            return "";
        }

        return File.ReadAllText(path);
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
}
