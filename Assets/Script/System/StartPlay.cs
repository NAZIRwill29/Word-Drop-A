using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlay : MonoBehaviour
{
    public Animator startPlayAnim;
    private Touch touch;

    void Update()
    {
        if (GameManager.instance.isTutorialMode)
            return;
        if (!GameManager.instance.isInStage)
            return;
        if (GameManager.instance.isStartStagePlay)
            return;
        if (Input.touchCount > 0)
            TouchToStart();
    }

    //detect touch to start stage play
    private void TouchToStart()
    {
        //get input
        touch = Input.GetTouch(0);
        GameManager.instance.StartStagePlay();
        startPlayAnim.SetInteger("startPlay", 0);
    }

    public void SetStartPlay()
    {
        //set perk 
        int perkNo = 10;
        //perk 1
        if (GameManager.instance.inGame.isIncreaseDifficulty)
            perkNo = 1;
        //check monster existence
        if (GameManager.instance.inGame.monster)
        {
            //perk 2
            if (GameManager.instance.inGame.monster.isNoSlowDown)
                perkNo = 2;
            //perk 3
            if (GameManager.instance.inGame.monster.isForeverChangeState)
                perkNo = 3;
        }
        startPlayAnim.SetInteger("startPlay", perkNo);
    }
}
