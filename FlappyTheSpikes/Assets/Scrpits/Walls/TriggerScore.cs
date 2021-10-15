using UnityEngine;

public class TriggerScore : MonoBehaviour
{
    public delegate void PlayerReachWall(int score);
    public static PlayerReachWall playerPassWall;

    public LayerMask playerMask;


    private void Start()
    {
        playerPassWall?.Invoke(GameManager.Instance.scorePlayer);   //Inicia score
    }

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Contains(playerMask, other.gameObject.layer))
        {
            GameManager.Instance.IncreaseScore();
            playerPassWall?.Invoke(GameManager.Instance.scorePlayer);
        }
    }
}
