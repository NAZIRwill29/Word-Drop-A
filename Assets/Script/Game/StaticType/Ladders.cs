using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour
{
    public GameObject[] arrLadder;
    public GameObject ladderUse;
    [SerializeField] private Animator laddersCompleteAnim;
    public int activeLadderNo, ladderLimit;
    public float ladderToClimb;
    private bool isTouched;
    public bool isCompleted;

    void Start()
    {
        ladderToClimb = arrLadder.Length;
        ladderLimit = arrLadder.Length;
    }

    //USED () - in ladder btn
    public void AddActiveLadders(bool isFromGroundManager)
    {
        if (activeLadderNo < ladderLimit)
        {
            if (!isFromGroundManager)
            {
                activeLadderNo++;
                SetActiveLadders();
                EnableCompleteLadders(true);
            }
            else
            {
                ladderToClimb -= 0.5f;
                ladderLimit = Mathf.FloorToInt(ladderToClimb);
                EnableCompleteLadders(true);
            }
        }
    }

    //set ladder state
    public void SetActiveLadders()
    {
        for (int i = 0; i < activeLadderNo; i++)
        {
            arrLadder[i].SetActive(true);
        }
    }

    //make it complete if active ladder is full constructed
    private void EnableCompleteLadders(bool isEnable)
    {
        if (!isEnable)
        {
            //stop glowing
            laddersCompleteAnim.SetTrigger("static");
            return;
        }
        if (activeLadderNo == ladderLimit)
        {
            isCompleted = true;
            //start glowing
            laddersCompleteAnim.SetTrigger("glow");
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isCompleted)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        //check if collide with player or ground
        if (coll.tag == "Player")
        {
            if (!isTouched)
            {
                isTouched = true;
                coll.SendMessage("Climb", ladderToClimb);
                EnableCompleteLadders(false);
            }
        }
    }
}
