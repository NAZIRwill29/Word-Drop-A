using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float speed = 0.05f, originSpeed;
    public float objHeight;
    void Start()
    {
        //save ori speed
        originSpeed = speed;
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isPauseGame)
            WaterRise();
    }

    private void WaterRise()
    {
        transform.position += new Vector3(0, Time.deltaTime * speed, 0);
    }

    //use after player revive
    public void AfterRevive()
    {
        transform.position -= new Vector3(0, 6, 0);
    }

    //water effect when touch player - lifeline 1,2,3
    public void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collide with player or ground
        if (coll.tag == "LifeLine")
        {
            //Debug.Log("LifeLine");
            //slow speed / death
            switch (coll.name)
            {
                case "LifeLine1":
                    GameManager.instance.player.LifeLine(1);
                    break;
                case "LifeLine2":
                    GameManager.instance.player.LifeLine(2);
                    break;
                case "LifeLine3":
                    GameManager.instance.player.LifeLine(3);
                    break;
                case "LifeLine4":
                    GameManager.instance.player.LifeLine(4);
                    break;
                default:
                    break;
            }
        }
    }

    //variable
    public void IncreaseSpeed(float num)
    {
        speed += num;
    }
}
