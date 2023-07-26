using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeUpDownAction : MonoBehaviour
{
    //[SerializeField]    private Player player;
    private Touch touch;
    public float bound = 125;
    private float swipeForce;
    public bool isActionInvalid;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame || GameManager.instance.isPauseGame)
            return;
        if (GameManager.instance.playerData.isHasWin)
            return;
        if (GameManager.instance.playerData.isHasDie)
            return;
        //TUTORIAL MODE ()
        if (GameManager.instance.isTutorialMode)
            return;
        if (Input.touchCount > 0)
        {
            //get input
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //Debug.Log("swipe up");
                swipeForce = touch.deltaPosition.y;
                if (swipeForce > bound)
                {
                    //make it once only
                    if (!isActionInvalid)
                    {
                        isActionInvalid = true;
                        //Debug.Log("word menu appear");
                        GameManager.instance.gameMenuUi.gameMenuUiAnim.SetTrigger("actionMenu");
                        GameManager.instance.mainMenuUI.PlaySoundNavigate();
                        GameManager.instance.gameMenuUi.SetActionMenu();
                        GameManager.instance.canvasGroupFunc.ModifyCG(GameManager.instance.inGameUi.inGameUICG, 0, false, false);
                        GameManager.instance.PauseGame(true);
                    }
                }
            }
        }
    }

    public void ChangeIsActionInvalid(bool isTrue)
    {
        isActionInvalid = isTrue;
    }
}
