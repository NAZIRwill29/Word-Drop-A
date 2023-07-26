using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : ObjByPlayer
{
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collide with player or ground
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.tag == "Monster")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                //send message damage to ground
                coll.SendMessage("ObjHit", dmg);
                //Debug.Log("Block Monster");
                //hide and off coliider
                ShowObj(false);
            }
        }
    }
}
