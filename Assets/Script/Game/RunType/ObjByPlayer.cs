using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjByPlayer : DropObject
{
    [SerializeField] private float xBound = 1.76f;
    [SerializeField] private BuilderInRun builderInRun;
    public Collider2D ObjColl;
    public SpriteRenderer ObjSR;
    public float posY;
    [Tooltip("Only change for slime")]
    public float timeDelayHide = 2;
    [SerializeField] private float hitSlimeNum = 0;
    public bool isInDeploy;

    void Update()
    {
        //safety execution of not trigger when hit monster
        // if (transform.position.y < GameManager.instance.inGame.monster.transform.position.y)
        // {
        //     ShowObj(false);
        // }
    }

    // set paused game
    public override void PauseGame(bool isPause)
    {
        if (isPause)
        {
            dropObjRb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            if (isInDeploy)
                dropObjRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    //call in ingameUi
    public virtual void ShowObj(bool isShow)
    {
        if (isShow)
            StartCoroutine(ShowObjEvent());
        else
        {
            if (objType == "fence")
                StartCoroutine(HideObjEvent("fence"));
            else
                StartCoroutine(HideObjSlimeEvent());
        }
    }
    private IEnumerator ShowObjEvent()
    {
        isTouched = false;
        isInDeploy = true;
        float posX = GameManager.instance.player.transform.position.x;
        //set boundary so the object builded will not out of screen
        if (posX < -xBound)
            posX = -xBound;
        else if (posX > xBound)
            posX = xBound;
        transform.position = new Vector3(posX, posY, 0);
        yield return new WaitForSeconds(0.01f);
        ObjColl.enabled = true;
        ObjSR.enabled = true;
    }
    //use for fence/slime
    private IEnumerator HideObjEvent(string objType)
    {
        isInDeploy = false;
        //Debug.Log("hide fence");
        ObjColl.enabled = false;
        ObjSR.enabled = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
        PauseGame(true);
        builderInRun.ChangeIndexNo(objType);
    }
    //use for slime
    private IEnumerator HideObjSlimeEvent()
    {
        hitSlimeNum++;
        //Debug.Log("slime hit x" + hitSlimeNum);
        if (hitSlimeNum / 30 > timeDelayHide)
        {
            isInDeploy = false;
            isTouched = true;
            //Debug.Log("hide slime");
            GameManager.instance.inGame.monster.isSlowByObj = false;
            yield return new WaitForSeconds(0.1f);
            ObjColl.enabled = false;
            ObjSR.enabled = false;
            transform.position = originalPos;
            PauseGame(true);
            builderInRun.ChangeIndexNo("slime");
            hitSlimeNum = 0;
        }
        yield return new WaitForSeconds(0);
    }

    //variable-----------------------------
    public void ChangeIsInDeploy(bool isTrue)
    {
        isInDeploy = isTrue;
    }
    //---------------------------------------------
}
