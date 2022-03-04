using System.Collections;
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
    [SerializeField] public GameObject postProccessGo;
    [SerializeField] PlayGames googlePlayService;
    [SerializeField] FacuLoggerPlug nativePlugin;
    [SerializeField] FacebookMng facebookManager;

    [SerializeField, Tooltip("The first on array will always be easy mode, while the last one hard mode")]
    [Space(20)]
    public int [] dificultyBehaviour;
    public bool gamePaused;
    [Tooltip("Enable this on executemode to clear the saved items on quit.")]
    public bool clearItemsBuyed;
    public GManagerReference gmReference;
    [HideInInspector] public bool firstTimePostProscess = false;

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
    public UnityAction<int> OnGiveCurrencyToPlayer;
    public UnityAction<TMPro.TextMeshProUGUI, float, float> OnGainPoints;
    public UnityAction OnVibrationChangeState;

    public UnityAction OnPostProccesOn;
    public UnityAction OnPostProccesOff;
    #endregion

    #region PRIVATE_FIELDS
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

    #region PROPERTIES
    public FacebookMng Facebook
    {
        get
        {
            return facebookManager;
        }
    }
    #endregion

    private void Start()
    {
        InitializeEsentials();

        lastDificulty = actualDificulty;

        OnResetGameplay += ResetScore;

        /*if (nativePlugin != null)
        {
            recordScore = nativePlugin.GetSavedScore();
            gmReference.ParseCommandLineOnConsole("Score loaded from plugin: " + recordScore);
        }*/
        recordScore = PlayerPrefs.GetInt("RecordScore", 0);

        TriggerScore.playerPassWall?.Invoke(scorePlayer,recordScore);

        audioManagerRef = AudioManagerScript.AudioManager.Instance;

        audioManagerRef.Play("Gameplay");
    }
    private void OnEnable()
    {
        InitializeEsentials();

        /*if (nativePlugin != null)
        {
            recordScore = nativePlugin.GetSavedScore();
            gmReference.ParseCommandLineOnConsole("Score loaded from plugin: " + recordScore);
        }*/
        recordScore = PlayerPrefs.GetInt("RecordScore", 0);

        TriggerScore.playerPassWall?.Invoke(scorePlayer, recordScore);

        currencyPlayer = PlayerPrefs.GetInt("CurrencyPlayer", 0);

        LoadSavedItems();
    }

    private void OnDisable()
    {
        SaveActualCurrency();

        if (!clearItemsBuyed)
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
            if(googlePlayService != null)
            {
                googlePlayService.UnlockAchievement("GettingStarted");
            }

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

            IncreaseCurrency(200);
            OnGiveCurrencyToPlayer?.Invoke(currencyPlayer);
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
            if(itemsBought != null)
            {
                for (int i = 0; i < itemsSaved.items.Length ; i++)
                {
                    itemsBought[itemsSaved.items[i].idInShop] = itemsSaved.items[i];
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

        if (allItemsBought.Count >= 1)
        {
            SaveSystem.SaveItems(allItemsBought);
        }
    }

    public void SaveItemPlayer(int id, ItemShop newItemBought)
    {
        itemsBought[id] = newItemBought;
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
            OnGiveCurrencyToPlayer?.Invoke(currencyPlayer);
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

    public void SaveActualCurrency()
    {
        PlayerPrefs.SetInt("CurrencyPlayer", currencyPlayer);
        PlayerPrefs.Save();
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
        if(googlePlayService != null)
        {
            googlePlayService.AddScoreToLeaderboard();
        }
        /*if(nativePlugin != null)
        {
            nativePlugin.SaveRecordScore(recordScore);
        }*/
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

    public void ChangeStateVibration()
    {
        OnVibrationChangeState?.Invoke();
        Vibrator.Instance.SwitchVibratorState();
    }

    public void ChangeStatePostProcces()
    {
        postProccessGo.SetActive(!postProccessGo.activeSelf);

        if (postProccessGo.activeSelf)
            OnPostProccesOff?.Invoke();
        else
            OnPostProccesOn?.Invoke();

        if(!firstTimePostProscess)
            firstTimePostProscess = true;
    }

    public void OpenLeaderboards()
    {
        if (googlePlayService == null)
            return;

        googlePlayService.ShowLeaderboard();
    }

    public void OpenAchievements()
    {
        if (googlePlayService == null)
            return;

        googlePlayService.ShowAchievements();
    }

}
