using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : MonoBehaviour
{
    //[SerializeField] Player player;
    //sound
    //  0       1       2       
    //play   cancel  navigate 
    [SerializeField] private AudioClip[] mainMenuUIAudioClip;
    public AudioSource mainMenuUIAudioSource;
    public GameObject musicBtnOn, musicBtnOff, SoundBtnOn, SoundBtnOff;
    public GameObject blackScreen, blackScreen2, firstScreen;
    public Slider musicSlider, soundSlider;
    [SerializeField]
    private Animator mainMenuAnim, backgroundAnim, playerInfoWindowAnim, playerInfoBtnAnim, playerLvlBtnAnim;
    public Animator loadingScreenAnim, tipModules;
    //player info window
    public Image playerImg;
    public TextMeshProUGUI lvlText, hpText, abcText, coinText, bookText;
    //book btn ads 
    public Button bookBtn;
    //public Image bookAdsImg;
    //public GameObject[] stageBtnObj;
    public Button[] stageBtnBtn;
    public GameObject[] stageBtnFalse;
    [SerializeField] private ScrollRect levelScrollRect;
    //private bool isLoadingScreenAnimate;
    // Start is called before the first frame update
    void Start()
    {

    }

    //start game - USED IN () = start button
    public void StartButton(string name)
    {
        GameManager.instance.StartGame(name, 0);
    }

    public void StartButtonRun(string name)
    {
        GameManager.instance.StartGame(name, 1);
    }

    //setting - USED IN () = setting button
    public void SettingButton()
    {

    }

    //level window------------------------------
    //USED () - in play btn
    public void LevelWindow()
    {
        //hide stage btn
        foreach (var item in stageBtnBtn)
        {
            //item.SetActive(false);
            item.enabled = false;
        }
        foreach (var item in stageBtnFalse)
        {
            //item.SetActive(false);
            item.SetActive(true);
        }
        //unhide stage btn 
        for (int i = 0; i <= GameManager.instance.passStageNo + 1; i++)
        {
            //prevent from enable non exist btn
            if (i >= stageBtnBtn.Length)
                return;
            // stageBtnObj[i].SetActive(true);
            stageBtnBtn[i].enabled = true;
            stageBtnFalse[i].SetActive(false);
        }
        //TUTORIAL MODE () - ended - hide stage btn
        if (GameManager.instance.isHasTutorial)
        {
            stageBtnBtn[0].enabled = false;
            stageBtnFalse[0].SetActive(true);
        }
        //reset scroll
        levelScrollRect.verticalNormalizedPosition = 1;
    }
    //----------------------------------------

    //player info window----------------------
    //USED () - in player info btn
    public void PlayerInfoWindow()
    {
        //Update player info
        //make show level up option only
        GameManager.instance.player.LevelUp(true);
        if (GameManager.instance.playerData.levelPlayer == 6)
            lvlText.text = "Lv " + GameManager.instance.playerData.levelPlayer + " (MAX)";
        else
            lvlText.text = "Lv " + GameManager.instance.playerData.levelPlayer;
        hpText.text = GameManager.instance.playerData.hp.ToString();
        abcText.text = GameManager.instance.playerData.charMaxNo.ToString();
        coinText.text = GameManager.instance.coin.ToString() + " (" + CoinReqLvlUpText() + ")";
        bookText.text = GameManager.instance.playerData.bookNum + " (" + BookReqLvlUpText() + ")";
        GameManager.instance.gameMenuUi.SetCoinEvent();
        //show if book have free gift ads
        if (!GameManager.instance.isBookAdsUsed)
        {
            bookBtn.enabled = true;
            //bookAdsImg.enabled = true;
            playerInfoWindowAnim.SetTrigger("showAds");
        }
        else
        {
            bookBtn.enabled = false;
            //bookAdsImg.enabled = false;
            playerInfoWindowAnim.SetTrigger("show");
        }
    }
    //show level up requirement text
    private string CoinReqLvlUpText()
    {
        int coinNeed;
        switch (GameManager.instance.playerData.levelPlayer + 1)
        {
            case 2:
                coinNeed = GameManager.instance.coin - 10;
                break;
            case 3:
                coinNeed = GameManager.instance.coin - 30;
                break;
            case 4:
                coinNeed = GameManager.instance.coin - 55;
                break;
            case 5:
                coinNeed = GameManager.instance.coin - 90;
                break;
            case 6:
                coinNeed = GameManager.instance.coin - 140;
                break;
            default:
                return "";
        }
        if (coinNeed - 0 > 0)
            return "+" + coinNeed;
        else
            return coinNeed.ToString();
    }
    private string BookReqLvlUpText()
    {
        int bookNeed;
        switch (GameManager.instance.playerData.levelPlayer + 1)
        {
            case 2:
                bookNeed = GameManager.instance.playerData.bookNum - 1;
                break;
            case 3:
                bookNeed = GameManager.instance.playerData.bookNum - 3;
                break;
            case 4:
                bookNeed = GameManager.instance.playerData.bookNum - 5;
                break;
            case 5:
                bookNeed = GameManager.instance.playerData.bookNum - 8;
                break;
            case 6:
                bookNeed = GameManager.instance.playerData.bookNum - 12;
                break;
            default:
                return "";
        }
        if (bookNeed - 0 > 0)
            return "+" + bookNeed;
        else
            return bookNeed.ToString();
    }
    //USED () - in player lvl btn
    public void LevelUp()
    {
        //levele up
        GameManager.instance.player.LevelUp(false);
        PlayerInfoWindow();
    }
    //USED () - in bookBtn
    //get book by watching ads
    public void BookAdsBtn()
    {
        GameManager.instance.adsMediate.ShowRewarded("book");
        //GameManager.instance.GetBook();
    }
    //----------------------------------------

    //setting window--------------------------
    //music
    public void MusicToggle(bool isWantOn)
    {
        musicBtnOn.SetActive(isWantOn);
        musicBtnOff.SetActive(!isWantOn);
        //on/off music
        GameManager.instance.gameSettings.TurnOnMusicVolume(isWantOn);
    }
    //sound effect
    public void SoundToggle(bool isWantOn)
    {
        SoundBtnOn.SetActive(isWantOn);
        SoundBtnOff.SetActive(!isWantOn);
        //on/off sound
        GameManager.instance.gameSettings.TurnOnSoundVolume(isWantOn);
    }
    //USED IN () - credit button
    public void CreditButton()
    {

    }
    //USED IN () - support button
    public void SupportButton()
    {

    }
    //USED IN () - musicSlide
    public void ChangeMusicVolume()
    {
        GameManager.instance.gameSettings.ChangeMusicVolumeSystem(musicSlider.value);
    }
    //USED IN () - soundSlide
    public void ChangeSoundVolume()
    {
        GameManager.instance.gameSettings.ChangeSoundVolumeSystem(soundSlider.value);
    }

    //update sound setting
    public void UpdateSoundSetting(float musicVolume, float soundVolume, bool isMusicOn, bool isSoundOn)
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        musicBtnOn.SetActive(isMusicOn);
        musicBtnOff.SetActive(!isMusicOn);
        SoundBtnOn.SetActive(isSoundOn);
        SoundBtnOff.SetActive(!isSoundOn);
    }

    //delete user account
    //USED () - in deleteBtn
    public void RequestDeleteAccount()
    {
        GameManager.instance.RequestDeleteUserAccDb();
    }

    //animation-------------------------------------------
    public IEnumerator ShowAnim()
    {
        yield return new WaitForSeconds(0);
        mainMenuAnim.SetTrigger("show");
        backgroundAnim.SetTrigger("show");
        Debug.Log("main menu show");
        GameManager.instance.OnMainMenu();
    }
    public IEnumerator HideAnim()
    {
        yield return new WaitForSeconds(0);
        //mainMenuAnim.SetTrigger("hide");
        backgroundAnim.SetTrigger("hide");
        Debug.Log("main menu hide");
    }
    //---------------------------------------------------

    //set player lvl upgrade notice
    public void SetPlayerUpgradeNotice(bool isUpgradable)
    {
        playerInfoBtnAnim.SetBool("upgradable", isUpgradable);
        playerLvlBtnAnim.SetBool("upgradable", isUpgradable);
    }

    //loading screen
    public void ShowLoadingScreen(bool isShow)
    {
        if (isShow)
        {
            // if (isLoadingScreenAnimate)
            //     return;
            // isLoadingScreenAnimate = true;
            //change tip module randomly
            int tipNo = Random.Range(1, 10);
            tipModules.SetInteger("state", tipNo);
            //show loading screen
            loadingScreenAnim.SetInteger("state", 3);
        }
        else
        {
            // if (!isLoadingScreenAnimate)
            //     return;
            // isLoadingScreenAnimate = false;
            tipModules.SetInteger("state", 0);
            loadingScreenAnim.SetInteger("state", 0);
        }
    }

    //unlock full stage
    public void UnlockFullStage()
    {
        GameManager.instance.passStageNo = 24;
    }

    //play sound -------------------------------------------
    public void PlaySoundPlay()
    {
        if (mainMenuUIAudioSource.isPlaying)
            return;
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[0]);
    }
    public void PlaySoundCancel()
    {
        if (mainMenuUIAudioSource.isPlaying)
            return;
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[1]);
    }
    public void PlaySoundNavigate()
    {
        if (mainMenuUIAudioSource.isPlaying)
            return;
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[2]);
    }
    //----------------------------------------------------
}
