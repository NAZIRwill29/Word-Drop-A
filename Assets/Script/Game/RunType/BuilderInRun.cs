using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderInRun : MonoBehaviour
{
    [SerializeField] private Fence[] fences;
    [SerializeField] private Slime[] slimes;
    public int objBuildInGame, objBuildInGameLimit;
    [SerializeField] private int fenceIndex, slimeIndex;

    private void Start()
    {
        //set limit
        objBuildInGameLimit = fences.Length;
    }

    public void BuildObj(int objType)
    {
        //check not more than 5
        // if (objBuildInGame > 5)
        // {
        //     //TOOD () - builder is busy
        //     return;
        // }
        //push monster a little
        GameManager.instance.inGame.monster.transform.position -= new Vector3(0, 0.5f, 0);
        if (objType == 0)
        {
            fences[fenceIndex].ShowObj(true);
            objBuildInGame++;
            fenceIndex++;
        }
        else
        {
            slimes[slimeIndex].ShowObj(true);
            objBuildInGame++;
            slimeIndex++;
        }
    }

    //variable
    public void ChangeIndexNo(string ObjType)
    {
        objBuildInGame--;
        if (ObjType == "fence")
            fenceIndex--;
        else
            slimeIndex--;
    }

    public void PauseGame(bool isPause)
    {
        foreach (var item in fences)
        {
            item.PauseGame(isPause);
        }
        foreach (var item in slimes)
        {
            item.PauseGame(isPause);
        }
    }
}
