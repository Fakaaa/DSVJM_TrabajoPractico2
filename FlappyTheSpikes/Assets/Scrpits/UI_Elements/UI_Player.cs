using TMPro;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI recordScorePause;
    [SerializeField] TextMeshProUGUI recordScoreDefeat;

    void Start()
    {
        TriggerScore.playerPassWall += UpdateScore;
    }

    private void OnDestroy()
    {
        TriggerScore.playerPassWall -= UpdateScore;
    }

    public void UpdateScore(int score, int recordScore)
    {
        recordScorePause.text = recordScore.ToString();
        recordScoreDefeat.text = recordScore.ToString();
        this.score.text = score.ToString();
    }
}
