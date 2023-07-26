using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPathPillar : Obstacle
{
    // Update is called once per frame
    private void Update()
    {
        if (isTouched)
            return;
        if (transform.position.y < -6.0f)
        {
            //make trigger once only
            isTouched = true;
            //put to birth location
            transform.position = originalPos;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D coll)
    {

    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collide with player or ground
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.tag == "Player")
        {
            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
