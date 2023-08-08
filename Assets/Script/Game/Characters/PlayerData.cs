using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int hp = 2, hpTemp;
    public float speed = 0.01f;
    //LifeLine
    public int lifeLineTrigger;
    public int lifeLineBuildTrigger = -1;
    //climb number
    public float climbNo, winMoveNo;
    public bool isImmune, isHasWin, isHasDie, isImmuneDamage = true;
    //immune damage - use for in start game
    public int immuneDamageDuration = 150;
    public int immuneDamageCount;
    public int doubleEarnCount, doubleEarnDuration = 200;
    public int addWordPt;
    public Vector3 originPos;
    //player info
    public int levelPlayer = 1, charMaxNo = 10, dieNum, bookNum;
    public int playerMode;
    public string deathScenario;
}
