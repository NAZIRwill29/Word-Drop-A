using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounds : MonoBehaviour
{
    [SerializeField]
    private Ground[] arrGround;
    public Sprite[] groundSprite1, groundSprite2, groundSprite3;
    public GroundManager groundManager;
    //public PhysicsMaterial2D stickyPsc, normalPsc;
    // Make ground rise
    public void GroundRise(float num)
    {
        transform.position += new Vector3(0, num, 0);
    }

    //change is acttive for all ground
    public void ChangeIsActive(bool isEnable)
    {
        foreach (var item in arrGround)
        {
            item.ChangeIsActive(isEnable);
        }
    }
}
