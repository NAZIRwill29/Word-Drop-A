using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpUi : MonoBehaviour
{
    //notification get pop up
    [SerializeField] private CanvasGroup getEventPopUpCG;
    [SerializeField] private GameObject getBookObj, getCoinObj;
    [SerializeField] private TextMeshProUGUI getBookText, getCoinText;

    public void GetItemPopUp(bool isBook, bool isCoin, int booknum, int coinNum)
    {
        StartCoroutine(ShowGetItemPopUp(isBook, isCoin, booknum, coinNum));
    }

    //notification get pop up
    private IEnumerator ShowGetItemPopUp(bool isBook, bool isCoin, int booknum, int coinNum)
    {
        getEventPopUpCG.alpha = 1;
        if (isBook)
        {
            getBookObj.SetActive(true);
            getBookText.text = booknum.ToString();
        }
        if (isCoin)
        {
            getCoinObj.SetActive(true);
            getCoinText.text = coinNum.ToString();
        }
        yield return new WaitForSeconds(1.25f);
        //if (isBook)
        getBookObj.SetActive(false);
        //if (isCoin)
        getCoinObj.SetActive(false);
        getEventPopUpCG.alpha = 0;
    }
}
