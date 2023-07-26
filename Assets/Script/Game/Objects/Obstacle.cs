using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DropObject
{
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        //check if collide with player or ground
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.collider.tag == "Ground")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                if (isReverseObj)
                    //send message damage to ground
                    coll.collider.SendMessage("ObjHit", dmg);
                //put to birth location
                transform.position = originalPos;
            }
        }
        else if (coll.collider.tag == "Player")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                coll.collider.SendMessage("ReceiveDamage", dmg);
                //put to birth location
                transform.position = originalPos;
            }
        }
        else if (coll.collider.tag == "Monster")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                if (isReverseObj)
                    //send message damage to ground
                    coll.collider.SendMessage("ObjMakeRage", dmg);
                //put to birth location
                transform.position = originalPos;
            }
        }
    }
}
