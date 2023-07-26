using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : DropObject
{
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameManager.instance.isPauseGame)
            return;
        //check if collide with player or ground
        if (coll.tag == "Ground" || coll.tag == "Monster")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                //put to birth location
                transform.position = originalPos;
            }
        }
        else if (coll.tag == "Player")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                coll.SendMessage("ReceiveBook");
                //put to birth location
                transform.position = originalPos;
            }
        }
    }
}
