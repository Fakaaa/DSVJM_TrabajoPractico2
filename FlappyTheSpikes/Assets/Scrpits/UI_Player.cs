using UnityEngine;
using TMPro;

public class UI_Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;

    void Start()
    {
        TriggerScore.playerPassWall += UpdateScore;
    }

    private void OnDestroy()
    {
        TriggerScore.playerPassWall -= UpdateScore;
    }

    public void UpdateScore(int amount)
    {
        score.text = amount.ToString();
    }
}
