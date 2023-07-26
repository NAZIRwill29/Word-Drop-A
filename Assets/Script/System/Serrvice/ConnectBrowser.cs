using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBrowser : MonoBehaviour
{
    //req delete user acc url
    public void ReqDelUseAccUrl(string playerId)
    {
        OpenUrl("mailto:wordnazservice@gamil.com?subject=Request%20Delete%20User%20Account&body=player%20id%20%3A%20"
         + playerId + "%0Atime%3A%20T" + System.DateTime.Now.ToString("MM/dd/yyyy"));
    }

    //open url
    public void OpenUrl(string urlName)
    {
        //open url in browser
        Application.OpenURL(urlName);
    }
}
