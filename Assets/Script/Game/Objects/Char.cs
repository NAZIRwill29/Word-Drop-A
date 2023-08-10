using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : DropObject
{
    public char alphabet;
    [SerializeField] private SpriteRenderer charSR;
    [SerializeField] private GameObject effect;
    [Tooltip("reverse, blood, shield, fake")]
    [SerializeField] private int reverseCharType;
    //for fake char 
    private int fakeNo;
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
                if (!isReverseObj)
                    GameManager.instance.player.SendMessage("ReceiveChar", alphabet);
                else
                {
                    switch (reverseCharType)
                    {
                        //normal reverse
                        case 0:
                            GameManager.instance.player.SendMessage("ReceiveReverseChar", alphabet);
                            break;
                        //blood
                        case 1:
                            GameManager.instance.player.SendMessage("ReceiveBloodChar", alphabet);
                            break;
                        //shield
                        case 2:
                            GameManager.instance.player.SendMessage("ReceiveShieldChar", alphabet);
                            break;
                        //fake
                        case 3:
                            GameManager.instance.player.SendMessage("ReceiveFakeChar");
                            break;
                        default:
                            break;
                    }
                }
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
        //normal char
        if (!isReverseObj)
        {
            charSR.sprite = GameManager.instance.alphabetSprite[(int)abc - 65];
            effect.SetActive(false);
        }
        else
        {
            //for reserve char
            switch (reverseCharType)
            {
                //normal reverse
                case 0:
                    charSR.sprite = GameManager.instance.reverseAlphabetSprite[(int)abc - 65];
                    break;
                //blood
                case 1:
                    charSR.sprite = GameManager.instance.bloodAlphabetSprite[(int)abc - 65];
                    break;
                //shield
                case 2:
                    charSR.sprite = GameManager.instance.shieldAlphabetSprite[(int)abc - 65];
                    break;
                //fake
                case 3:
                    fakeNo = Random.Range(1, 36);
                    charSR.sprite = GameManager.instance.fakeAlphabetSprite[fakeNo];
                    break;
                default:
                    break;
            }
            //all reverse except fake char white
            if (fakeNo < 27)
                effect.SetActive(true);
            else
                effect.SetActive(false);
        }
    }
}
