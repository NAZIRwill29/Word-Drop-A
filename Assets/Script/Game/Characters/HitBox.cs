using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    //  0           1          2      
    //player      monster   little
    [Tooltip("player, monster, little")]
    [SerializeField]
    private int parentType = 0;
    [Tooltip("no need if not player")]

    //player-----------------------
    public void ReceiveDamage(Damage dmg)
    {
        if (parentType == 0)
            GameManager.instance.player.ReceiveDamage(dmg);
    }
    public void ReceiveDamageHp(Damage dmg)
    {
        if (parentType == 0)
            GameManager.instance.player.ReceiveDamageHp(dmg);
    }
    public void ReceiveChar(char abc)
    {
        if (parentType == 0)
            GameManager.instance.player.ReceiveChar(abc);

    }
    public void ReceiveBook()
    {
        if (parentType == 0)
            GameManager.instance.player.ReceiveBook();
    }
    public void ReceiveCoin(int coin)
    {
        if (parentType == 0)
            GameManager.instance.player.ReceiveCoin(coin);
    }
    public void Climb(int num)
    {
        if (parentType == 0)
            GameManager.instance.player.Climb(num);
    }
    public void Win(bool isStaticGameMode)
    {
        //Debug.Log("hit win line");
        //only call for drowned game mode
        if (parentType == 0)
            GameManager.instance.player.Win(true);
    }

    //monster------------------------
    public void ObjHit(Damage dmg)
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.ObjHit(dmg);
    }
    public void ObjMakeRage(Damage dmg)
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.ObjMakeRage(dmg);
    }
    public void SlowObj(Damage dmg)
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.SlowObj(dmg);
    }


}
