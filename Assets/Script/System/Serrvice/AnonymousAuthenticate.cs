using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class AnonymousAuthenticate : MonoBehaviour
{
    public string playerId;

    //make anonymous authenticate
    public async void AnonymousAuthenticateEvent()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log(UnityServices.State);
            SetupEvents();
            await SignInAnonymouslyAsync();
            GameManager.instance.isSuccesLogin = true;
            //load game
            GameManager.instance.LoadGameDataFromCloud();
        }
        catch (System.Exception e)
        {
            Debug.Log("error in anonymous authenticate " + e);
        }

    }

    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"PlayerID: {playerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
          {
              Debug.Log("Player session could not be refreshed and expired.");
          };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
