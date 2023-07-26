using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //SOLUTION () - resolve the 2 audio listener error 
    //public static GameObject cameraInstance;
    public Animator camAnim;
    //public GameObject playerObj;
    [SerializeField] private float elevation = 1.45f;
    public bool isStopFollow;

    // Update is called once per frame
    void LateUpdate()
    {
        if (isStopFollow)
            return;
        //if (GameManager.instance.isStartGame)
        FollowPlayer();
    }

    //follow player
    private void FollowPlayer()
    {
        transform.position = new Vector3(0, GameManager.instance.player.transform.position.y + elevation, -10);
    }

    public void CamShake()
    {
        int randomNo = Random.Range(0, 2);
        switch (randomNo)
        {
            case 0:
                camAnim.SetTrigger("shake");
                break;
            case 1:
                camAnim.SetTrigger("shake1");
                break;
            case 2:
                camAnim.SetTrigger("shake2");
                break;
            default:
                break;
        }
    }

    //rise camera follow ground
    // public void RiseCamera(float num)
    // {
    //     transform.position += new Vector3(0, num, 0);
    // }
}
