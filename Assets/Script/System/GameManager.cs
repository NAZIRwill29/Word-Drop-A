using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public static GameObject canvasMainInstance;
    public CameraManager cameraManager;
    public CanvasGroupFunc canvasGroupFunc;
    public Boundary boundary;
    public Data gameData = new Data() { musicVolume = 1, soundVolume = 1 };
    //save
    public CloudSave cloudSave;
    //authenticate
    public AnonymousAuthenticate anonymousAuthenticate;
    //ads mediate
    public AdsMediate adsMediate;
    public ConnectBrowser connectBrowser;
    //loading scene
    public LoadingScene loadingScene;
    public MainMenuUI mainMenuUI;
    public GameMenuUi gameMenuUi;
    public GameSettings gameSettings;
    public GameObject dontDestroyGameObject;
    public PlayerData playerData;
    public Player player;
    //  0       1             2         3
    //human   car/boat    airplane    helicopter  
    [SerializeField] private Player[] players;
    //  0     1       2       3       4     
    //car1  car2    car3    boat1   boat2   
    [SerializeField] private Sprite[] playerSprite;
    public SwipeRigthLeftMove swipeRigthLeftMove;
    public SwipeUpDownAction swipeUpDownAction;
    public TutorialUI tutorialUI;
    //game
    public InGame inGame;
    public InGameUi inGameUi;
    public Tutorial tutorial;
    public Sprite[] alphabetSprite, reverseAlphabetSprite;
    //data to be saved
    public int passStageNo;
    public int coin, diamond, shieldBought, charBought;
    //public int coin, diamond, skinIndexBought;
    //variable
    public bool isHasFirstOpen;
    public bool isStartGame;
    public bool isPauseGame = true, isTutorialMode, isHasTutorial, isFinishLoadScene, isStartStagePlay, isInStage;
    //isInStage;
    private bool isCanShowAds, isStartPlayTime;
    public bool isPremiumPlan, isBookAdsUsed = true, isSuccesLogin, isSuccessLoadCloud;
    public List<int> skinIndexBought;
    public float playTime;

    //awake
    void Awake()
    {
        //Debug.Log("awake");
        //check if have instance
        if (GameManager.instance != null)
        {
            SetMainMenu(true);
            //Debug.Log("awake1");
            return;
        }
        else
        {
            //get current date
            gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
            //initialize ads
            adsMediate.InitAdsMediate();
            anonymousAuthenticate.AnonymousAuthenticateEvent();
            instance = this;
            //Debug.Log("awake2");
            SetMainMenu(false);
        }
        Debug.Log("isStartGame = " + isStartGame);
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        if (isStartPlayTime)
            playTime++;
    }

    private void SetMainMenu(bool isInstance)
    {
        if (isInstance)
        {
            //set stage if instance != null
            //yield return StartCoroutine(TurnOnStageButton());
            //DestroyGameObject();
            //yield return StartCoroutine(UpdatePlayerInfoInStart());
            Destroy(dontDestroyGameObject);
        }
        else
        {
            //set stage if instance = null 
            //- turn on stage button based on player achievement
            LoadData();
            //yield return StartCoroutine(TurnOnStageButton());
            //yield return StartCoroutine(UpdatePlayerInfoInStart());
            if (!isHasFirstOpen)
                StartCoroutine(CloseFirstScene());
        }
        OnMainMenu();
    }

    //coroutine for setStage
    // private void DestroyGameObject()
    // {
    //     //Destroy(gameObject);
    //     Destroy(dontDestroyGameObject);
    //     //Debug.Log("destroy game object");
    //     //yield return new WaitForSeconds(0);
    // }
    private void LoadData()
    {
        //yield return new WaitForSeconds(0);
        //Debug.Log("load stageNo = " + stagePassedNo);
        SceneManager.sceneLoaded += LoadState;
    }

    // private IEnumerator UpdatePlayerInfoInStart()
    // {
    //     yield return new WaitForSeconds(0);
    //     //TODO () - update player info
    // }

    private IEnumerator CloseFirstScene()
    {
        yield return new WaitForSeconds(0.15f);
        mainMenuUI.firstScreen.SetActive(false);
        isHasFirstOpen = true;
    }

    //on main menu
    public void OnMainMenu()
    {
        isStartGame = false;
        isPauseGame = true;
        player.FinishGame();
        //show level up notice
        player.LevelUp(true);
        //SceneManager.LoadScene("MainMenu");
        mainMenuUI.blackScreen.SetActive(true);
        //change music based on gameSettings
        gameSettings.ChangeMusicBackground(false, 0);
    }

    //back to home
    public void BackToHome()
    {
        //hide inGameUi
        if (inGameUi)
            inGameUi.SetupInGameUi(false);
        mainMenuUI.blackScreen.SetActive(true);
        player.GameMode(2);
        StartCoroutine(InBackToHome());
        Debug.Log("back to home");
    }
    private IEnumerator InBackToHome()
    {
        loadingScene.LoadLoadingScene();
        yield return StartCoroutine(InBackToHomeEvent());
        yield return StartCoroutine(mainMenuUI.ShowAnim());
    }
    private IEnumerator InBackToHomeEvent()
    {
        //for load scene
        isFinishLoadScene = false;
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSeconds(0.1f);
    }

    //TODO - make start game
    public void StartGame(string name, int mode)
    {
        mainMenuUI.blackScreen2.SetActive(true);
        loadingScene.LoadLoadingScene();
        adsMediate.LoadInterstitial();
        StartCoroutine(InStartGame(name, mode));
        Debug.Log("start game");
    }
    private IEnumerator InStartGame(string name, int mode)
    {
        //not hide main menu ui on game - avoid repeat many time
        if (!inGame)
            yield return StartCoroutine(mainMenuUI.HideAnim());
        yield return StartCoroutine(InStartGameEvent(name, mode));
    }
    private IEnumerator InStartGameEvent(string name, int mode)
    {
        //for load scene
        isFinishLoadScene = false;
        SceneManager.LoadScene(name);
        gameSettings.UpdateMenuVolumeSetting();
        yield return new WaitForSeconds(0.1f);
        mainMenuUI.blackScreen.SetActive(false);
        //mainMenuUI.blackScreen2.SetActive(false);
        //change music based on inGame
        gameSettings.ChangeMusicBackground(true, 0);
    }

    //pause game
    public void PauseGame(bool isPause)
    {
        isPauseGame = isPause;
        inGame.PauseGame(isPause);
        player.PauseGame(isPause);
    }

    //Start stage play - used after finish loading
    public void StartStagePlay()
    {
        Debug.Log("start stage play");
        isStartGame = true;
        isStartStagePlay = true;
        inGame.StartStagePlay();
        PauseGame(false);
    }

    //finish game/ finish stage
    public void FinishGame(bool isBackToHome)
    {
        player.FinishGame();
        PauseGame(true);
        SaveState(true, false);
        if (isBackToHome)
            BackToHome();
    }

    //show interstitial ads
    public void ShowInterstitial()
    {
        //check premium plan - no ads
        if (isPremiumPlan)
            return;
        //check ads cycle
        if (isCanShowAds)
        {
            adsMediate.ShowInterstitial();
            isCanShowAds = false;
        }
        else
        {
            Debug.Log("show ads");
            isCanShowAds = true;
        }
    }

    public void Death(bool isReal)
    {
        gameMenuUi.Death(isReal);
        PauseGame(true);
    }

    //continue after death
    public void ContinueAfterDeath(bool isAds)
    {
        if (!isAds)
        {
            if (playerData.bookNum > 0)
            {
                player.AddBookNum(-1);
                Revive();
            }
            else
                Debug.Log("book is empty");
        }
        else
        {
            adsMediate.ShowRewarded("revive");
        }
    }

    //continue next stage - start game in game play
    public void ContinueNextStage()
    {
        SaveState(true, false);
        mainMenuUI.blackScreen2.SetActive(true);
        adsMediate.LoadInterstitial();
        StartGame(inGame.nextStageName, inGame.nextStageMode);
        Debug.Log("continue game");
    }

    //revive player
    public void Revive()
    {
        player.Revive();
        gameMenuUi.Revive();
        SaveState(true, false);
        PauseGame(false);
    }

    //game data----------------------------------------
    //call every scene
    public void SaveState(bool isConditionSave, bool isNoCloud)
    {
        try
        {
            //save variable
            gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
            gameData.playTime = playTime;
            gameData.passStageNo = passStageNo;
            gameData.bookNumCollect = playerData.bookNum;
            gameData.playerLevel = playerData.levelPlayer;
            gameData.coin = coin;
            //gameData.isMusicOn = gameSettings.isMusicOn;
            //gameData.isSoundOn = gameSettings.isSoundOn;
            gameData.musicVolume = gameSettings.musicVolume;
            gameData.soundVolume = gameSettings.soundVolume;
            //TODO () - uncomment when finish tutorial
            gameData.isHasTutorial = isHasTutorial;
            gameData.isPremiumPlan = isPremiumPlan;
            gameData.diamond = diamond;
            gameData.skinIndexBought = skinIndexBought;
            gameData.isBookAdsUsed = isBookAdsUsed;
            gameData.shieldBought = shieldBought;
            gameData.charBought = charBought;
            SavePrefFile();
            Debug.Log("Save state");
            if (!isNoCloud)
                SaveGameDataInCloud(isConditionSave);
        }
        catch (System.Exception e)
        {
            Debug.Log("error in save state " + e.Message);
        }
    }
    private void SavePrefFile()
    {
        //transform instance to json
        string json = JsonUtility.ToJson(gameData);
        //method to write string to a file
        /*Application.persistentDataPath - give you a folder where you can save data that 
        will survive between application reinstall or update and append to it the filename savefile.json*/
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    //call when open game
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        try
        {
            //return;
            gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
            //get path of saved data
            string path = Application.persistentDataPath + "/savefile.json";
            //check if exist
            if (File.Exists(path))
            {
                Debug.Log("Load state");
                //read content
                string json = File.ReadAllText(path);
                //transform into SaveData instance
                Data dataLoad = JsonUtility.FromJson<Data>(json);
                //compare play duration - if longer then exec
                if (dataLoad.playTime > gameData.playTime)
                    SetGameData(dataLoad, false);
                //DebugAllData();
                //play time
                playTime = gameData.playTime;
                //start record playTime
                isStartPlayTime = true;
                passStageNo = gameData.passStageNo;
                player.SetBookNum(gameData.bookNumCollect);
                Debug.Log("in LoadState : level = " + gameData.playerLevel);
                player.SetPlayerLevel(gameData.playerLevel);
                coin = gameData.coin;
                gameSettings.ChangeMusicVolumeSystem(gameData.musicVolume);
                gameSettings.ChangeSoundVolumeSystem(gameData.soundVolume);
                //gameSettings.TurnOnMusicVolume(gameData.isMusicOn);
                //gameSettings.TurnOnSoundVolume(gameData.isSoundOn);
                //TODO () - uncomment when finish tutorial
                isHasTutorial = gameData.isHasTutorial;
                isPremiumPlan = gameData.isPremiumPlan;
                diamond = gameData.diamond;
                skinIndexBought = gameData.skinIndexBought;
                isBookAdsUsed = gameData.isBookAdsUsed;
                shieldBought = gameData.shieldBought;
                charBought = gameData.charBought;
                if (!inGame)
                    //show level up notice
                    player.LevelUp(true);
                //SaveState(true);
                //make call only once only
                SceneManager.sceneLoaded -= LoadState;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

    }
    //request delete user account in database
    public void RequestDeleteUserAccDb()
    {
        string playerId = anonymousAuthenticate.playerId;
        connectBrowser.ReqDelUseAccUrl(playerId);
    }

    public void ResetData()
    {
        //gameData.isSoundOn = true;
        //gameData.isMusicOn = true;
        gameData.playTime = 0;
        gameData.soundVolume = 1;
        gameData.musicVolume = 1;
        gameData.dateNow = "";
        gameData.savedDate = "";
        gameData.passStageNo = 0;
        gameData.bookNumCollect = 0;
        gameData.playerLevel = 1;
        gameData.coin = 0;
        gameData.isHasTutorial = false;
        gameData.isPremiumPlan = false;
        gameData.diamond = 0;
        gameData.skinIndexBought.Clear();
        gameData.shieldBought = 0;
        gameData.charBought = 0;
    }
    private void DebugAllData()
    {
        //Debug.Log("isSoundOn = " + gameData.isSoundOn);
        //Debug.Log("isMusicOn = " + gameData.isMusicOn);
        Debug.Log("soundVolume = " + gameData.soundVolume);
        Debug.Log("musicVolume = " + gameData.musicVolume);
        Debug.Log("dateNow = " + gameData.dateNow);
        Debug.Log("savedDate = " + gameData.savedDate);
        Debug.Log("passStageNo = " + gameData.passStageNo);
        Debug.Log("bookNumCollect = " + gameData.bookNumCollect);
        Debug.Log("playerLevel = " + gameData.playerLevel);
        Debug.Log("coin = " + gameData.coin);
    }
    //---------------------------------------------------

    //cloud--------------------------------------------------------------
    //save game data
    public void SaveGameDataInCloud(bool isConditionSave)
    {
        if (!isSuccesLogin)
            return;
        if (!isSuccessLoadCloud)
            return;
        //gameData.savedDate = "";
        Debug.Log("save : dateNow = " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", savedDate = " + gameData.savedDate);
        //make it able to save disregard condition
        if (isConditionSave)
        {
            //save once a day
            if (System.DateTime.Now.ToString("MM/dd/yyyy") != gameData.savedDate || gameData.savedDate == "")
            {
                gameData.savedDate = gameData.dateNow;
                cloudSave.SaveComplexDataCloud(gameData);
            }
            else
                Debug.Log("falied save data in cloud due save duration");
        }
        else
        {
            gameData.savedDate = gameData.dateNow;
            cloudSave.SaveComplexDataCloud(gameData);
        }
        SaveState(true, true);
    }
    //load game data
    public void LoadGameDataFromCloud()
    {
        if (!isSuccesLogin)
            return;
        Debug.Log("load : dateNow = " + System.DateTime.Now.ToString("MM/dd/yyyy") + ", savedDate = " + gameData.savedDate);
        //load once a day
        if (System.DateTime.Now.ToString("MM/dd/yyyy") != gameData.savedDate)
        {
            //reset bookAdsUsed
            isBookAdsUsed = false;
            //laod all
            cloudSave.LoadComplexDataCloud();
        }
        else
        {
            Debug.Log("falied load data from cloud due save duration");
            isSuccessLoadCloud = false;
        }
    }
    //---------------------------------------------------------------

    //on scene loaded - call every time load scene
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        try
        {
            //reset in stage
            isInStage = false;
            //reset start stage play
            isStartStagePlay = false;
            Debug.Log("OnSceneLoaded");
            SaveState(true, false);
            //Debug.Log("OnSceneLoaded");
            gameSettings.MusicSystem();
            gameSettings.SoundSystem();
            mainMenuUI.UpdateSoundSetting(gameSettings.musicVolume, gameSettings.soundVolume, gameSettings.isMusicOn, gameSettings.isSoundOn);
            if (GameObject.Find("InGame"))
            {
                inGame = GameObject.Find("InGame").GetComponent<InGame>();
                //LOADING ()
                PauseGame(true);
            }
            if (GameObject.Find("InGameUI"))
            {
                inGameUi = GameObject.Find("InGameUI").GetComponent<InGameUi>();
                ChangePlayer();
                // if (isStartGame)
                gameMenuUi.SetGameMenuUIMode(inGameUi.isRun);
            }
            //reset player
            player.RemoveAllChar();
            player.LifeLine(0);
            if (GameObject.Find("Tutorial"))
            {
                //start tutorial
                tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
                isTutorialMode = true;
                player.ManagePlayerLevel();
                tutorial.TutorialPhaseNo = 1;
                tutorial.Tutorial1Trigger();
            }
            else
                isTutorialMode = false;
            //for load scene
            isFinishLoadScene = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //decide player - exec in run only
    private void ChangePlayer()
    {
        if (!inGameUi.isRun)
        {
            player = players[0];
            TurnOnPlayer(0);
            player.StartGame(0);
        }
        else
        {
            Debug.Log("change vehicle");
            switch (inGame.playerVehicleIndex)
            {
                case 0:
                    //car 1
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[0];
                    Debug.Log("change sprite");
                    break;
                case 1:
                    //car 2
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[1];
                    break;
                case 2:
                    //car 3
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[2];
                    break;
                case 3:
                    //boat 1
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[3];
                    break;
                case 4:
                    //boat 2
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[4];
                    break;
                case 5:
                    //airplane
                    player = players[2];
                    TurnOnPlayer(2);
                    break;
                case 6:
                    //helicopter
                    player = players[3];
                    TurnOnPlayer(3);
                    break;
                default:
                    break;
            }
            player.StartGame(1);
        }
    }
    //turn on/off player obj
    private void TurnOnPlayer(int index)
    {
        foreach (var item in players)
        {
            item.gameObject.SetActive(false);
        }
        players[index].gameObject.SetActive(true);
    }

    //update stage progress
    public void UpdateStageProgress()
    {
        //prevent from take low pass stages
        if (passStageNo < inGame.currentStageNo)
            passStageNo = inGame.currentStageNo;
        else
        {
            isBookAdsUsed = false;
            //when pass new next stage - save cloud
            SaveState(false, false);
        }


    }

    //get book
    public void GetBook()
    {
        isBookAdsUsed = true;
        SaveState(true, false);
        player.ReceiveBook();
        mainMenuUI.PlayerInfoWindow();
    }

    //record playtime
    // public IEnumerator RecordTimeRoutine()
    // {
    //     //TimeSpan timeSpan;
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(1);
    //         playTime++;
    //     }
    // }

    //set variable------------------------------------------
    public void AddCoin(int num)
    {
        coin += num;
    }
    public void SetSavedDate(string date1)
    {
        gameData.savedDate = date1;
    }
    //use in load state and reset state
    public void SetGameData(Data data, bool isWantSave)
    {
        // gameData = data;
        gameData.savedDate = data.savedDate;
        gameData.bookNumCollect = data.bookNumCollect;
        gameData.coin = data.coin;
        gameData.isPremiumPlan = data.isPremiumPlan;
        gameData.playTime = data.playTime;
        gameData.passStageNo = data.passStageNo;
        gameData.playerLevel = data.playerLevel;
        gameData.musicVolume = data.musicVolume;
        gameData.soundVolume = data.soundVolume;
        gameData.isPremiumPlan = data.isPremiumPlan;
        gameData.isBookAdsUsed = data.isBookAdsUsed;
        gameData.isHasTutorial = data.isHasTutorial;
        gameData.diamond = data.diamond;
        gameData.skinIndexBought = data.skinIndexBought;
        gameData.shieldBought = data.shieldBought;
        gameData.charBought = data.charBought;
        //TODO - set data for all in game
        if (!isWantSave)
            SceneManager.sceneLoaded += LoadState;
    }
    //-------------------------------------------------------
}

