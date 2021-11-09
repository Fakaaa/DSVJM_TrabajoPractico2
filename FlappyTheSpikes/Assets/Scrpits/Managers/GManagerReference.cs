using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] public PanelAnimations defeatScreen;
    [HideInInspector] public Player player;
    public bool GmRefInitialized
    {
        get; set;
    }

    public void InitGMReference(GameManager gameManager)
    {
        reference = gameManager;
        GmRefInitialized = true;
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
}
