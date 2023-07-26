using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    [SerializeField] protected Vector3 originalPos;
    public Rigidbody2D dropObjRb;
    public int damage = 1;
    [Tooltip("char, bomb, fence, slime")]
    public string objType;
    protected Damage dmg;
    public bool isReverseObj;
    [SerializeField] protected bool isTouched;
    protected virtual void Start()
    {
        dmg = new Damage
        {
            damageAmount = damage,
            objType = objType
        };
    }

    // set paused game
    public virtual void PauseGame(bool isPause)
    {
        if (isPause)
        {
            dropObjRb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            dropObjRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    //variable
    public void ChangeIsTouched(bool isEnable)
    {
        isTouched = isEnable;
    }
}
