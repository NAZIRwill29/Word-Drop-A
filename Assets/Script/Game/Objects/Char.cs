using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : DropObject
{
    public char alphabet;
    [SerializeField] private SpriteRenderer charSR;
    [SerializeField] private GameObject effect;
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
                if (isReverseObj)
                    //send message damage to ground
                    coll.SendMessage("ObjHit", dmg);
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
                GameManager.instance.player.SendMessage("ReceiveChar", alphabet);
                //coll.SendMessage("ReceiveChar", alphabet);
                //put to birth location
                transform.position = originalPos;
            }
        }
    }

    public void SetAlphabet(char abc)
    {
        alphabet = abc;
        //Debug.Log(abc + "char index = " + (int)abc);
        if (!isReverseObj)
        {
            charSR.sprite = GameManager.instance.alphabetSprite[(int)abc - 65];
            effect.SetActive(false);
        }
        else
        {
            //for reserve char
            charSR.sprite = GameManager.instance.reverseAlphabetSprite[(int)abc - 65];
            effect.SetActive(true);
        }
    }
}
