using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AudioSource monsterAudioSource;
    //     0           1          2        3
    //  damage    damageAnger  slime    attack    
    [SerializeField] private AudioClip[] monsterAudioClip;
    private bool isImmune, isHasBegin, isMonsterBeginEvent;
    public bool isForeverChangeState, isNoSlowDown, isImmuneInAnger, isImmuneHit;
    public string objType = "Monster";
    private Damage dmg;
    public int damage = 1;
    [SerializeField] private int hpChange = 2;
    [SerializeField] private float speed = 0.07f, fallBackDist = 0.8f;
    private Vector3 originPos;
    private float originSpeed, tempSpeed;
    //private float lastAttack;
    //      0       1            2
    //   normal   slower    speedy/rage
    public Sprite[] monsterSprite;
    public SpriteRenderer monsterSR;
    public Animator monsterAnim, hitEffectAnim;
    //      0        1     
    //  normal    animation
    public int monsterNo;
    public float objHeight;
    public bool isSlowByObj;
    //for push backward when hitted
    private bool isPushByPlayer, isPushByObj, isPushByObjPlayer;
    private int pushPlayerNum, pushObjNum, pushObjPlayerNum, multPushForce, multPlayerLvl;
    //[SerializeField] private float addPushForce = 0.0002f;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        //used when need to increase or decrease speed
        originSpeed = speed;
        tempSpeed = speed;
        dmg = new Damage
        {
            damageAmount = damage,
            objType = objType
        };
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        multPlayerLvl = GameManager.instance.playerData.levelPlayer - 1;
    }

    //50 frame per sec
    void FixedUpdate()
    {
        if (!GameManager.instance.isStartGame || GameManager.instance.isPauseGame)
            return;
        if (isHasBegin)
        {
            if (isPushByPlayer)
                PushByPlayer();
            if (isPushByObj)
                PushByObj();
            if (isPushByObjPlayer)
                PushByObjPlayer();
            MonsterChase();
        }
        else
        {
            if (!isMonsterBeginEvent)
                StartCoroutine(MonsterBeginEvent());
        }
    }

    private IEnumerator MonsterBeginEvent()
    {
        isMonsterBeginEvent = true;
        yield return new WaitForSeconds(0.5f);
        //StartPushByPlayer();
        transform.position -= new Vector3(0, fallBackDist, 0);
        isHasBegin = true;
    }

    //monster chase player by rising
    private void MonsterChase()
    {
        transform.position += new Vector3(0, Time.deltaTime * speed, 0);
    }

    //use after player revive
    public void AfterRevive()
    {
        //calm down monster
        hpChange = 2;
        SetMonsterState();
        transform.position -= new Vector3(0, 6, 0);
    }

    //monster damage by thing - push backward
    public void ObjHit(Damage dmg1)
    {
        if (isImmuneHit)
            return;
        if (isImmune && dmg1.objType == "char")
        {
            //Debug.Log("immune char");
            return;
        }
        PlaySoundDamage();
        multPushForce = dmg1.damageAmount;
        //transform.position -= new Vector3(0, fallBackDist / 20 * dmg1.damageAmount, 0);
        //differentiate btw obj from player and dropObj
        if (dmg1.objType == "char" || dmg1.objType == "blockPath")
        {
            StartPushByObj();
            hpChange += Random.Range(0, 1);
        }
        else
        {
            StartPushByObjPlayer();
            hpChange += 1;
        }
        SetMonsterState();
        //make hit anim
        hitEffectAnim.SetTrigger("hit");
    }

    //monster recovery by thing - make monster anger - increase speed
    public void ObjMakeRage(Damage dmg1)
    {
        if (isImmune && dmg1.objType != "fence")
        {
            //Debug.Log("immune !fence");
            return;
        }

        PlaySoundDamageAnger();
        multPushForce = 1;
        //transform.position -= new Vector3(0, fallBackDist / 20, 0);
        //differentiate btw obj from player and dropObj
        if (dmg1.objType != "fence")
            StartPushByObj();
        else
            StartPushByObjPlayer();
        hpChange -= 1;
        SetMonsterState();
        //make hit anim
        hitEffectAnim.SetTrigger("hit");
    }

    //monster slow speed by slime
    public void SlowObj(Damage dmg1)
    {
        if (isImmune && dmg1.objType != "slime")
        {
            //Debug.Log("immune !slime");
            return;
        }
        transform.position -= new Vector3(0, fallBackDist / 60, 0);
        //Debug.Log("slow obj");
        //check audio source is playing
        //PlaySoundSlime();
        if (!isSlowByObj)
        {
            PlaySoundSlime();
            hpChange -= 1;
            isSlowByObj = true;
        }
    }

    //push force -------------------------------------------------
    //start push event
    private void StartPushByPlayer()
    {
        isPushByPlayer = true;
        ResetPushByPlayer();
    }
    private void StartPushByObj()
    {
        isPushByObj = true;
        ResetPushByObj();
    }
    private void StartPushByObjPlayer()
    {
        isPushByObjPlayer = true;
        ResetPushByObjPlayer();
    }
    //push backward
    private void PushByPlayer()
    {
        if (pushPlayerNum > 0)
        {
            //Debug.Log("push backward");
            transform.position -= new Vector3(0, fallBackDist / 55, 0);
            pushPlayerNum--;
        }
        else
        {
            ResetPushByPlayer();
            isPushByPlayer = false;
        }
    }
    private void PushByObj()
    {
        if (pushObjNum > 0)
        {
            //Debug.Log("push backward");
            transform.position -= new Vector3(0, fallBackDist / 120 * multPushForce, 0);
            pushObjNum--;
        }
        else
        {
            ResetPushByObj();
            isPushByObj = false;
        }
    }
    private void PushByObjPlayer()
    {
        if (pushObjPlayerNum > 0)
        {
            //Debug.Log("push backward");
            transform.position -= new Vector3(0, fallBackDist / 120 * multPushForce, 0);
            pushObjPlayerNum--;
        }
        else
        {
            ResetPushByObjPlayer();
            isPushByObjPlayer = false;
        }
    }
    private void ResetPushByPlayer()
    {
        pushPlayerNum = 100 + 9 * multPlayerLvl;
    }
    private void ResetPushByObj()
    {
        pushObjNum = 2;
    }
    private void ResetPushByObjPlayer()
    {
        pushObjPlayerNum = 35 + 4 * multPlayerLvl;
    }
    //-------------------------------------------------

    private void SetMonsterState()
    {
        //set sprite, speed, damage monster
        //0 123
        if (hpChange < 0)
        {
            //speedy / rage state
            //decide based on monster no - because some has animation
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[2];
            else
                monsterAnim.SetInteger("state", 2);

            if (!isForeverChangeState)
            {
                if (isImmuneInAnger)
                    isImmune = true;
                ChangeSpeed(tempSpeed + tempSpeed);
                if (monsterNo == 0)
                    StartCoroutine(ResetMonsterStateDelay());
                else
                    monsterAnim.SetInteger("state", 2);
            }
            else
            {
                if (isImmuneInAnger)
                    isImmune = true;
                // if (isImmuneEffect)
                //     isImmune = true;
                ChangeSpeed(tempSpeed + tempSpeed);
            }
        }
        else if (hpChange > 4)
        {
            if (isNoSlowDown)
                return;
            //slower state
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[1];
            else
                monsterAnim.SetInteger("state", 1);

            ChangeSpeed(tempSpeed - tempSpeed / 4);
            // if (isImmuneEffect)
            //     isImmune = true;
            StartCoroutine(ResetMonsterStateDelay());
        }
        else
        {
            //hpChange = 2
            //normal state
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[0];
            else
                monsterAnim.SetInteger("state", 0);

            ChangeSpeed(tempSpeed);
            isImmune = false;
            // if (isImmuneEffect)
            //     isImmune = true;
        }
    }

    //reset state in few seconds
    private IEnumerator ResetMonsterStateDelay()
    {
        //10 second
        yield return new WaitForSeconds(30);
        hpChange = 2;
        SetMonsterState();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.tag == "Player")
        {
            //Debug.Log("attack damage");
            coll.SendMessage("ReceiveDamageHp", dmg);
            StartPushByPlayer();
            //transform.position -= new Vector3(0, fallBackDist, 0);
        }
    }

    //variable
    public void ChangeSpeed(float num)
    {
        speed = num;
    }
    //used in increase difficulty
    public void IncreaseSpeed(float addNum)
    {
        tempSpeed += addNum;
        speed = tempSpeed;
    }

    //play sound -------------------------------------------
    public void PlaySoundDamage()
    {
        // if (monsterAudioSource.isPlaying)
        //     return;
        monsterAudioSource.PlayOneShot(monsterAudioClip[0]);
    }
    public void PlaySoundDamageAnger()
    {
        // if (monsterAudioSource.isPlaying)
        //     return;
        monsterAudioSource.PlayOneShot(monsterAudioClip[1]);
    }
    public void PlaySoundSlime()
    {
        // if (monsterAudioSource.isPlaying)
        //     return;
        monsterAudioSource.PlayOneShot(monsterAudioClip[2]);
        //Debug.Log("play sound slime");
    }
    public void PlaySoundAttack()
    {
        if (monsterAudioSource.isPlaying)
            return;
        monsterAudioSource.PlayOneShot(monsterAudioClip[3]);
    }
    //----------------------------------------------------

}
