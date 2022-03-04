using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GManagerReference : MonoBehaviour
{
    private GameManager reference;
    public GameManager ReferenceGM
    {
        get
        {
            return reference;
        }
    }
    #region EXPOSED_FIELDS
    [SerializeField] public PanelAnimations defeatScreen;
    [SerializeField] public GameObject uiShop;
    [SerializeField] public CameraLerper lerperCamera;
    [SerializeField] public FacebookMng fbManager;
    public DebugConsole theConsole;
    public UnityAction OnGoShopMainMenu;
    public UnityAction OnExitShopMainMenu;
    #endregion

    #region PRIVATE_FIELDS
    [HideInInspector] public Player player;
    #endregion

    public bool GmRefInitialized
    {
        get; set;
    }

    public void InitGMReference(GameManager gameManager)
    {
        reference = gameManager;
        fbManager = gameManager.Facebook;
        GmRefInitialized = true;

        if(lerperCamera != null)
            lerperCamera.OnLerpTargetEnded = ActivateShop;

        LocalizePlayer();
    }

    private void OnEnable()
    {
        GmRefInitialized = false;
    }

    private void OnDisable()
    {
        if (player == null)
            return;

        player.playerDefeat -= ActivateDefeat;
    }

    private void LocalizePlayer()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if(player != null)
            player.playerDefeat += ActivateDefeat;
    }

    public void PauseGame()
    {
        ReferenceGM.PauseGame();
    }

    public void ResumeGameplay()
    {
        ReferenceGM.ResumeGame();
    }

    public void ResetGameplay(int seconds)
    {
        ReferenceGM.ResetGame(seconds);
    }

    public void QuitMainMenu()
    {
        ReferenceGM.BackToMainMenu();
    }

    public void QuitGame()
    {
        ReferenceGM.QuitGame();
    }

    public void GoShopMainMenu()
    {
        OnGoShopMainMenu?.Invoke();
    }

    public void ExitShopMainMenu()
    {
        DisableShop();
        OnExitShopMainMenu?.Invoke();
    }

    private void ActivateShop()
    {
        uiShop.SetActive(true);
    }

    private void DisableShop()
    {
        uiShop.SetActive(false);
    }

    public void ActivateDefeat()
    {
        if (defeatScreen == null)
            return;

        if (!ReferenceGM.gamePaused)
            defeatScreen.ExcuteOpenAnimation();
        else
        {
            ReferenceGM.ResumeGame();
            defeatScreen.ExcuteOpenAnimation();
        }
    }

    public void ChangeStateVibration()
    {
        ReferenceGM.ChangeStateVibration();
    }

    public void ChangeStatePostProcces()
    {
        ReferenceGM.ChangeStatePostProcces();
    }

    public void OpenLeaderboards()
    {
        ReferenceGM.OpenLeaderboards();
    }

    public void OpenAchievements()
    {
        ReferenceGM.OpenAchievements();
    }

    #region FACEBOOK
    public void FacebookLogin()
    {
        fbManager.LoginFacebook();
    }

    public void FacebookLogout()
    {
        fbManager.LogOut();
    }

    public void FacebookShare()
    {
        fbManager.Share();
    }

    public void FacebookGameRequest()
    {
        fbManager.FacebookGameRequest();
    }

    public void FacebookOpenFeed()
    {
        fbManager.FeedFacebook();
    }
    #endregion

    public void ParseCommandLineOnConsole(string command)
    {
        if(theConsole != null)
            theConsole.ParseLineOnConsole(command);
    }
}
