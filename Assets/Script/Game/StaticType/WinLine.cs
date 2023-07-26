using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLine : MonoBehaviour
{
    private bool isTouched;
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameManager.instance.isPauseGame)
            return;
        //check if collide with player or ground
        if (coll.tag == "Player" || coll.name == "HitBox")
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                coll.SendMessage("Win", true);
            }
        }
    }
}
