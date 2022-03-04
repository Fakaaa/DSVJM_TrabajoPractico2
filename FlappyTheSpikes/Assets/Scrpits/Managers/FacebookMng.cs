using System.Collections.Generic;

using UnityEngine;

using Facebook.Unity;

public class FacebookMng : MonoBehaviour
{
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
        FB.ShareLink(new System.Uri("https://www.youtube.com"), "See a lot of videos for free!",
            "Here are for example a lot of math videos!",new System.Uri("https://www.youtube.com/user/julioprofe"));
    }
    public void FeedFacebook()
    {
        if (FB.IsLoggedIn)
        {
            FB.LogInWithPublishPermissions();
        }
    }
    public void FacebookGameRequest()
    {
        FB.AppRequest("Come plaay with me this awesome game! Is not like the averge flappy games!");
    }
    #endregion
}