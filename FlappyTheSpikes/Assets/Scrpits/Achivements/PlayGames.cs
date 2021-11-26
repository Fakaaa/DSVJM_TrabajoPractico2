using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class PlayGames : MonoBehaviour
{
    string leaderboardID = "CgkIjtqcz7cKEAIQAg";
    [SerializeField] List<MyAchievement> achievements;
    public static PlayGamesPlatform platform;

    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Logged in successfully");
            }
            else
            {
                Debug.Log("Login Failed");
            }
        });
    }

    public void AddScoreToLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportScore(GameManager.Instance.recordScore, leaderboardID, success => { });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowLeaderboardUI();
        }
    }

    public void ShowAchievements()
    {
        GameManager.Instance.gmReference.ParseCommandLineOnConsole("DebugLog: Trying to open google achievements panel. At:" + gameObject.name);
        
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowAchievementsUI();
        }
    }

    public void UnlockAchievement(string nameAchievement)
    {
        string idFinded = string.Empty;

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
            Social.ReportProgress(idFinded, 100f, success => { });
        }
    }
}