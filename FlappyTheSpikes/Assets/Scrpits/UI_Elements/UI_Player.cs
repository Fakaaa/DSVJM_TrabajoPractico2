using TMPro;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI currencyPlayer;
    [SerializeField] TextMeshProUGUI recordScorePause;
    [SerializeField] TextMeshProUGUI recordScoreDefeat;

    private GameManager gmInstance;

    void Start()
    {
        gmInstance = GameManager.Instance;

        if(gmInstance != null)
        {
            gmInstance.OnGiveCurrencyToPlayer += UpdateCurrencyPlayer;
        }

        TriggerScore.playerPassWall += UpdateScore;
    }

    private void OnDestroy()
    {
        if (gmInstance != null)
        {
            gmInstance.OnGiveCurrencyToPlayer -= UpdateCurrencyPlayer;
        }

        TriggerScore.playerPassWall -= UpdateScore;
    }

    public void UpdateScore(int score, int recordScore)
    {
        recordScorePause.text = recordScore.ToString();
        recordScoreDefeat.text = recordScore.ToString();
        this.score.text = score.ToString();
    }

    public void UpdateCurrencyPlayer(int actualCurrency)
    {
        currencyPlayer.text = "$ " + actualCurrency;
    }
}
