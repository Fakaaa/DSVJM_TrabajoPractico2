using System.Collections.Generic;

using UnityEngine;

using Facebook.Unity;

public class FacebookMng : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private string fbLogingState = string.Empty;
    #endregion

    #region PRIVATE_METHODS
    private void Awake()
    {
        if(!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Couldn't initialize");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Update()
    {
        if(FB.IsInitialized)
        {
            if(FB.IsLoggedIn)
            {
                fbLogingState = "Loged in!";
            }
            else
            {
                fbLogingState = "Loged out";
            }
        }
    }
    #endregion

    #region EXPOSED_METHODS
    public void LoginFacebook()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions);
    }
    public void LogOut()
    {
        FB.LogOut();
    }
    public void Share()
    {
        FB.ShareLink(new System.Uri("https://fakaa.itch.io"), "See my other games!",
            "Here are for example a chess game made it with raylib!",new System.Uri("https://fakaa.itch.io/chess-time"));
    }
    public void FeedFacebook()
    {
        if (FB.IsLoggedIn)
        {
            FB.FeedShare();
        }
    }
    public void FacebookGameRequest()
    {
        FB.AppRequest("Come play with me this awesome game! Is not like the averge flappy games!");
    }
    public string GetLogedState()
    {
        return fbLogingState;
    }
    #endregion
}