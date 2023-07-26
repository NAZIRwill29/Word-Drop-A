using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPath : Obstacle
{
    protected override void OnCollisionEnter2D(Collision2D coll)
    {
        //check if collide with player or ground
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.collider.tag == "Monster")
        {
            if (isTouched)
                return;
            //make trigger once only
            isTouched = true;
            //send message damage to ground
            coll.collider.SendMessage("ObjHit", dmg);
            //put to birth location
            transform.position = originalPos;

        }
        else if (coll.collider.tag == "Player")
        {
            if (isTouched)
                return;
            //make trigger once only
            isTouched = true;
            coll.collider.SendMessage("ReceiveDamageHp", dmg);
            //put to birth location
            transform.position = originalPos;
        }
    }
}
