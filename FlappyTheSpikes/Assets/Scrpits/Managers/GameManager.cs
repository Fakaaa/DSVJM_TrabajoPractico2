﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MonoBehaviourSingletonScript;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    #region EXPOSED_FIELDS
    [Header("GENERAL INFORMATION")]
    [SerializeField] public int scorePlayer;
    [SerializeField] public int valuePassWall;
    [SerializeField] public int currencyPlayer;
    [SerializeField] public ItemShop colorPlayerSelected;
    [SerializeField, Tooltip("The first on array will always be easy mode, while the last one hard mode")]
    public int [] dificultyBehaviour;
    public bool gamePaused;
    public bool dontPersistShop;

    [Header("SAVE&LOAD INFORMATION")]
    [Space(15)]
    public int recordScore;
    public int colorsSaved;
    public ItemsBoughtData itemsSaved;
    public Dictionary<int,ItemShop> itemsBought = new Dictionary<int, ItemShop>();
    #endregion

    #region PUBLIC ACTIONS
    public UnityAction<int> OnResetGameplay;
    public UnityAction OnQuitGame;
    public UnityAction<string> OnGoMainMenu;
    public UnityAction OnChangeDificulty;
    public UnityAction<TMPro.TextMeshProUGUI, float, float> OnGainPoints;
    #endregion

    #region PRIVATE_FIELDS
    GManagerReference gmReference;
    GameObject initialPlatform;
    UI_PopTexts uiPOPTexts;
    bool initialPlatformActive = true;
    AudioManagerScript.AudioManager audioManagerRef;
    public enum Dificulty
    {
        Easy,
        Medium,
        Hard,
        Nightmare
    }
    private Dificulty actualDificulty = Dificulty.Easy;
    private Dificulty lastDificulty;
    #endregion

    private void Start()
    {
        InitializeEsentials();

        lastDificulty = actualDificulty;

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

        LoadSavedItems();
    }

    private void OnDisable()
    {
        if(!dontPersistShop)
        {
            SaveBoughtItems();
        }
        else
        {
            SaveSystem.ResetItemsData();    //Limpia el archivo data de items con una lista vacia
        }
    }

    private void Update()
    {
        InitializeEsentials();

        DeactivateInitialPlatform();

        if (gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void CheckActualDificultyState()
    {
        if(scorePlayer <= dificultyBehaviour[0])
        {
            actualDificulty = Dificulty.Easy;
        }
        else if(scorePlayer > dificultyBehaviour[0] && scorePlayer <= dificultyBehaviour[1])
        {
            actualDificulty = Dificulty.Medium;
        }
        else if(scorePlayer > dificultyBehaviour[1] && scorePlayer <= dificultyBehaviour[2])
        {
            actualDificulty = Dificulty.Hard;
        }
        else if(scorePlayer > dificultyBehaviour[2] && scorePlayer <= dificultyBehaviour[3])
        {
            actualDificulty = Dificulty.Nightmare;
        }

        if(lastDificulty != actualDificulty)
        {
            lastDificulty = actualDificulty;
            OnChangeDificulty?.Invoke();
            Debug.Log("Dificulty has increased!");
        }
    }

    public Dificulty GetDificulty()
    {
        return actualDificulty;
    }

    public void LoadSavedItems()
    {
        itemsSaved = SaveSystem.LoadItems();
        
        if (itemsSaved == null)
        {
            return;
        }

        colorsSaved = itemsSaved.items.Length;

        if (itemsSaved.items.Length > 0)
        {
            Debug.Log("Hay "+ itemsSaved.items.Length + " items comprados que fueron cargados");

            if(itemsBought != null)
            {
                for (int i = 0; i < itemsSaved.items.Length ; i++)
                {
                    itemsBought[i] = itemsSaved.items[i];
                }
            }
        }
    }

    public void SaveBoughtItems()
    {
        List<ItemShop> allItemsBought = new List<ItemShop>();

        foreach(ItemShop item in itemsBought.Values)
        {
            allItemsBought.Add(item);
        }
        Debug.Log("Se intentaran guardar " + allItemsBought.Count + " comprados");

        if (allItemsBought.Count >= 1)
        {
            SaveSystem.SaveItems(allItemsBought);
            Debug.Log("Todo guardado!");
        }
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

        if (initialPlatform == null)
            initialPlatform = GameObject.FindGameObjectWithTag("InitialPlatform");

        if (uiPOPTexts == null)
            uiPOPTexts = FindObjectOfType<UI_PopTexts>();

        if (gmReference != null && !gmReference.GmRefInitialized)
        {
            gmReference.InitGMReference(this);
            TriggerScore.playerPassWall?.Invoke(scorePlayer, recordScore);
            OnChangeDificulty?.Invoke();
        }
    }

    public void DeactivateInitialPlatform()
    {
        if (!initialPlatformActive || initialPlatform == null)
            return;

        IEnumerator Delay()
        {
            initialPlatformActive = false;
            yield return new WaitForSeconds(2f);
            initialPlatform.SetActive(false);
            yield break;
        }
        if(initialPlatform != null)
            StartCoroutine(Delay());
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

    public void IncreaseCurrency(int currencyGived)
    {
        currencyPlayer += currencyGived;
    }

    public void IncreaseScore()
    {
        scorePlayer += valuePassWall;

        CheckActualDificultyState();

        if (scorePlayer > recordScore)
            SaveNewRecordScore(scorePlayer);

        if (uiPOPTexts == null)
            return;

        if(uiPOPTexts != null)
            OnGainPoints?.Invoke(uiPOPTexts.scoreGived, 180f, .25f);
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
        initialPlatform = null;
        initialPlatformActive = true;
    }
    public void ResetGame(int secondsToWait)
    {
        actualDificulty = Dificulty.Easy;
        lastDificulty = actualDificulty;
        OnResetGameplay?.Invoke(secondsToWait);

        uiPOPTexts = null;
        initialPlatform.SetActive(true);
        initialPlatformActive = true;
    }

    public void QuitGame()
    {
        OnQuitGame?.Invoke();
    }

}
