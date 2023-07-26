using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int TutorialPhaseNo;
    //trigger tutorial 1
    public void Tutorial1Trigger()
    {
        GameManager.instance.isStartGame = true;
        if (TutorialPhaseNo == 1)
            GameManager.instance.tutorialUI.TutorialEvent(1);
    }
    //tutorial check char no - trigger tutorial 2
    public void Tutorial2Trigger(int num)
    {
        if (TutorialPhaseNo != 2)
            return;
        if (num < 7)
            return;
        GameManager.instance.tutorialUI.TutorialEvent(2);
        //TutorialPhaseNo = 3;
    }

    //tutorial - when swipe up - trigger tutorial 3
    // public void Tutorial3Trigger()
    // {
    //     if (TutorialPhaseNo == 3)
    //         GameManager.instance.tutorialUI.Tutorial3();
    // }

    // public void TutorialEnd()
    // {
    //     GameManager.instance.gameMenuUi.TutorialEnd();

    // }
}
