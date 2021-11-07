using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    [SerializeField] public int scorePlayer;
    [SerializeField] public int valuePassWall;
    [SerializeField] public GameObject defeatScreen;

    public bool gamePaused;
    Player player;
    ObstaclesBehaviour obstacles;

    public int recordScore;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        obstacles = FindObjectOfType<ObstaclesBehaviour>();

        player.playerDefeat += ActivateDefeat;

        recordScore = PlayerPrefs.GetInt("RecordScore", 0);

        TriggerScore.playerPassWall?.Invoke(scorePlayer,recordScore);

        AudioManagerScript.AudioManager.Instance.Play("Gameplay");
    }
    private void Update()
    {
        if(gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    private void OnDestroy()
    {
        player.playerDefeat -= ActivateDefeat;
    }
    public void PauseGame()
    {
        gamePaused = true;
    }

    public void ResumeGame()
    {
        gamePaused = false;
    }

    public void IncreaseScore()
    {
        scorePlayer += valuePassWall;

        if (scorePlayer > recordScore)
            SaveNewRecordScore(scorePlayer);
    }

    public void SaveNewRecordScore(int val)
    {
        recordScore = val;
        PlayerPrefs.SetInt("RecordScore", recordScore);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        scorePlayer = 0;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ActivateDefeat()
    {
        if(!gamePaused)
            defeatScreen.SetActive(true);
        else
        {
            ResumeGame();
            defeatScreen.SetActive(true);
        }
    }
}
