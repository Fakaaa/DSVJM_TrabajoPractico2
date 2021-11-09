using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MonoBehaviourSingletonScript;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    #region EXPOSED_FIELDS
    [SerializeField] public int scorePlayer;
    [SerializeField] public int valuePassWall;
    [SerializeField] public int currencyPlayer;
    [SerializeField] public ItemShop colorPlayerSelected;
    public bool gamePaused;
    public int recordScore;
    #endregion

    #region PUBLIC ACTIONS
    public UnityAction<int> OnResetGameplay;
    public UnityAction OnQuitGame;
    public UnityAction<string> OnGoMainMenu;
    #endregion

    #region PRIVATE_FIELDS
    public Dictionary<int,ItemShop> itemsBought = new Dictionary<int, ItemShop>();
    GManagerReference gmReference;
    AudioManagerScript.AudioManager audioManagerRef;
    #endregion

    private void Start()
    {
        InitializeEsentials();

        OnResetGameplay += ResetScore;

        recordScore = PlayerPrefs.GetInt("RecordScore", 0);

        TriggerScore.playerPassWall?.Invoke(scorePlayer,recordScore);

        audioManagerRef = AudioManagerScript.AudioManager.Instance;

        audioManagerRef.Play("Gameplay");
    }
    private void OnEnable()
    {
        InitializeEsentials();

        recordScore = PlayerPrefs.GetInt("RecordScore", 0);

        TriggerScore.playerPassWall?.Invoke(scorePlayer, recordScore);
    }

    private void OnDisable()
    {
        SaveBoughtItems();
    }

    private void Update()
    {
        InitializeEsentials();

        if (gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SaveBoughtItems()
    {
        //Implementar guardado de items comprados
    }

    public void SaveItemPlayer(int id, ItemShop newItemBought)
    {
        itemsBought[id] = newItemBought;
        Debug.Log("Item comprado y almacenado!");
    }

    private void InitializeEsentials()
    {
        if (gmReference == null)
            gmReference = FindObjectOfType<GManagerReference>();
        
        if(gmReference != null && !gmReference.GmRefInitialized)
        {
            gmReference.InitGMReference(this);
            TriggerScore.playerPassWall?.Invoke(scorePlayer, recordScore);
        }
    }
    public void PauseGame()
    {
        gamePaused = true;
        audioManagerRef.Pause("Gameplay");
    }

    public void ResumeGame()
    {
        gamePaused = false;
        audioManagerRef.Resume("Gameplay");
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

    public void ResetScore(int value)
    {
        value = 0;
        scorePlayer = value;
    }

    public void BackToMainMenu()
    {
        ResumeGame();
        ResetScore(0);
        OnGoMainMenu?.Invoke("MainMenu");
    }
    public void ResetGame(int secondsToWait)
    {
        OnResetGameplay?.Invoke(secondsToWait);
    }

    public void QuitGame()
    {
        OnQuitGame?.Invoke();
    }

}
