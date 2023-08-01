using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public InGame inGame;
    //need 13 grounds
    [SerializeField] private Grounds[] arrGrounds;
    public AudioSource groundManagerAudioSource;
    //  0           1           2         3
    //damage    attack1      damage2    attack2
    [SerializeField] private AudioClip[] groundManagerAudioClip;
    public float riseNum = 0.513f;
    public int currentActiveGroundNo;
    //challenge mode
    private int groundChallengeNo;
    // Start is called before the first frame update
    void Start()
    {

    }

    //build ground event used in ingameui
    public void AddGround()
    {
        //check if reach max will climb and win
        if (currentActiveGroundNo < arrGrounds.Length - 1)
        {
            ChangeIsActiveGrounds(currentActiveGroundNo, false);
            arrGrounds[currentActiveGroundNo + 1].gameObject.SetActive(true);
            ChangeIsActiveGrounds(currentActiveGroundNo + 1, true);
            RiseGrounds(currentActiveGroundNo + 1);
            currentActiveGroundNo++;
            GameManager.instance.inGame.spawn.gameObject.transform.position += new Vector3(0, riseNum, 0);
            GameManager.instance.player.gameObject.transform.position += new Vector3(0, riseNum, 0);
            //standardized the ladders
            GameManager.instance.inGame.ladders.AddActiveLadders(true);
            GameManager.instance.inGame.ladders.ladderUse.transform.position += new Vector3(0, riseNum, 0);
            //heal from water
            GameManager.instance.player.LifeLine(GameManager.instance.playerData.lifeLineBuildTrigger);
            //Challenge MODE ()
            if (inGame.isChallengeStage)
            {
                groundChallengeNo++;
                if (groundChallengeNo > 13)
                {
                    groundChallengeNo = 0;
                    inGame.ChangeItemInChallengeMode();
                }
            }
        }
    }

    //rise grounds
    private void RiseGrounds(int num)
    {
        for (int i = num; i < arrGrounds.Length; i++)
        {
            arrGrounds[i].GroundRise(riseNum);
        }
    }

    //change is active grounds
    private void ChangeIsActiveGrounds(int num, bool isEnable)
    {
        arrGrounds[num].ChangeIsActive(isEnable);
    }

    //play sound -------------------------------------------
    public void PlaySoundDamage()
    {
        // if (groundManagerAudioSource.isPlaying)
        //     return;
        groundManagerAudioSource.PlayOneShot(groundManagerAudioClip[0]);
    }
    public void PlaySoundAttack1()
    {
        // if (groundManagerAudioSource.isPlaying)
        //     return;
        groundManagerAudioSource.PlayOneShot(groundManagerAudioClip[1]);
    }
    public void PlaySoundDamage2()
    {
        // if (groundManagerAudioSource.isPlaying)
        //     return;
        groundManagerAudioSource.PlayOneShot(groundManagerAudioClip[0]);
    }
    public void PlaySoundAttack2()
    {
        if (groundManagerAudioSource.isPlaying)
            return;
        groundManagerAudioSource.PlayOneShot(groundManagerAudioClip[2]);
    }
    //----------------------------------------------------
}
