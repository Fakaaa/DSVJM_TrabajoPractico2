using System;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayGames : MonoBehaviour
{
    string leaderboardID = "CgkIjtqcz7cKEAIQAg";
    [SerializeField] List<MyAchievement> achievements;
    public static PlayGamesPlatform platform;

    void Start()
    {
        Debug.Log("Entro start PLAY GAMES.");

        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success => { });

        //if (success)
        //{
        //    Debug.Log("Logged in successfully");
        //    GameManager.Instance.gmReference.ParseCommandLineOnConsole("Logged in successfully on play services.");
        //}
        //else
        //{
        //    Debug.Log("Login Failed");
        //    GameManager.Instance.gmReference.ParseCommandLineOnConsole("Login Failed on play services.");
        //}
    }

    public void AddScoreToLeaderboard()
    {
        Debug.Log("Entro AddScoreToLeaderboard");

        if (Social.Active.localUser.authenticated)
        {
            Debug.Log("Entro AddScoreToLeaderboard, entro IF");

            Social.ReportScore(GameManager.Instance.recordScore, leaderboardID, success => { });
        }
    }

    public void ShowLeaderboard()
    {
        Debug.Log("Entro showLeaderboard");

        if (Social.Active.localUser.authenticated)
        {
            Debug.Log("Entro showLeaderboard, entro IF");
            platform.ShowLeaderboardUI();
        }
    }

    public void ShowAchievements()
    {
        Debug.Log("Entro ShowAchievements");

        if (Social.Active.localUser.authenticated)
        {
            platform.ShowAchievementsUI();

            Debug.Log("Entro ShowAchievements, entro IF");

            GameManager.Instance.gmReference.ParseCommandLineOnConsole("DebugLog: Open google achievements panel. At:" + gameObject.name);
        }
        else
        {
            GameManager.Instance.gmReference.ParseCommandLineOnConsole("DebugLog: Failed to open panel of achievements. At:" + gameObject.name);
        }
    }

    public void UnlockAchievement(string nameAchievement)
    {
        string idFinded = string.Empty;
        Debug.Log("Entro UnlockAchievement");

        foreach (MyAchievement achievement in achievements)
        {
            if(achievement.name == nameAchievement)
            {
                idFinded = achievement.idAchievement;
                return;
            }
        }

        if (Social.Active.localUser.authenticated)
        {
            Debug.Log("Entro UnlockAchievement, entro IF");
            Social.ReportProgress(idFinded, 100f, success => { });
        }
    }
}