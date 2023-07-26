using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//DEPECRATED SCRIPT()
public class LoadingScene : MonoBehaviour
{
    // [SerializeField] private GameObject LoadingScreen, blackScreen;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private StartPlay startPlay;
    [SerializeField] private Image LoadingBarFill;
    public float speed;
    public void LoadLoadingScene()
    {
        //Debug.Log("load scene event");
        //StartCoroutine(LoadSceneAsync(sceneId));
        StartCoroutine(LoadSceneEvent());
    }

    // IEnumerator LoadSceneAsync(int sceneId)
    // {
    //     //LoadingScreen.SetActive(true);

    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

    //     while (!operation.isDone)
    //     {
    //         float progressValue = Mathf.Clamp01(operation.progress / speed);
    //         LoadingBarFill.fillAmount = progressValue;

    //         yield return null;
    //     }
    // }
    private IEnumerator LoadSceneEvent()
    {
        //Debug.Log("load screen start");
        mainMenuUI.ShowLoadingScreen(true);
        //turn off music
        GameManager.instance.gameSettings.TurnOnMusicDueLoading(false);
        yield return new WaitForSeconds(5);
        //exec when isFinishLoadScene = true
        yield return new WaitUntil(() => GameManager.instance.isFinishLoadScene);
        if (GameManager.instance.inGame)
            StartCoroutine(LoadBlackScreen());
        //Debug.Log("load screen end");
        // show inGameUi - only if has
        if (GameManager.instance.inGameUi)
            GameManager.instance.inGameUi.SetupInGameUi(true);
        mainMenuUI.ShowLoadingScreen(false);
        //turn on music
        GameManager.instance.gameSettings.TurnOnMusicDueLoading(true);
    }

    private IEnumerator LoadBlackScreen()
    {
        //Debug.Log("black screen start");
        mainMenuUI.blackScreen2.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        mainMenuUI.blackScreen2.SetActive(false);
        //Debug.Log("black screen end");
        GameManager.instance.isInStage = true;
        if (!GameManager.instance.isTutorialMode)
            startPlay.SetStartPlay();
    }
}
