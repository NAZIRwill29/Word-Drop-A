using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManagement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] backgroundsSR;
    [SerializeField] private Rigidbody2D[] backgroundsRb;
    [SerializeField] private Transform[] backgroundsTm;
    //background must equal size
    [SerializeField] private float[] backgroundHigherPos;
    [SerializeField] private float backgroundObjHeight;
    private int index;
    [SerializeField] private float boundY, spawnPosY;
    [SerializeField] private bool isLoopBackground;
    private float incNum, dragBgOri;
    [SerializeField] private float dragBg;
    // Start is called before the first frame update
    void Start()
    {
        boundY = GameManager.instance.boundary.boundY;
        backgroundObjHeight = backgroundsSR[0].bounds.size.y;
        //get spawn pos by making it at top of all bg = lowbound - bgheight x (total - 1)
        spawnPosY = -boundY + backgroundObjHeight * (backgroundsSR.Length - 1) - 0.035f;
        dragBgOri = dragBg;
        ChangeSpeedBackground(dragBg);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoopBackground)
            return;
        backgroundHigherPos[index] = backgroundObjHeight / 2 + backgroundsTm[index].position.y;
        //check if below bound screen
        if (backgroundHigherPos[index] < -boundY - 0.01f)
        {
            //move to upper spawn pos
            backgroundsTm[index].position = new Vector3(transform.position.x, spawnPosY + backgroundObjHeight / 2, 1);
            index++;
            //check more than bg provided
            if (index >= backgroundsTm.Length)
                index = 0;
        }
    }

    public void FreezeBackgrounds(bool isPause)
    {
        if (isPause)
        {
            foreach (var item in backgroundsRb)
            {
                item.bodyType = RigidbodyType2D.Static;
            }
        }
        else
        {
            foreach (var item in backgroundsRb)
            {
                item.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public void IncreaseSpeedBackground()
    {
        incNum += 0.005f;
        ChangeSpeedBackground(dragBg - incNum);
    }

    private void ChangeSpeedBackground(float dragNum)
    {
        if (dragNum < 0.4f)
            return;
        //Debug.Log("dragNum = " + dragNum);
        foreach (var item in backgroundsRb)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
}
