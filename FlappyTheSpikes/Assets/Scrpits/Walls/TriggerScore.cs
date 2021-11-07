using UnityEngine;
using UnityEngine.Events;

public class TriggerScore : MonoBehaviour
{
    public static UnityAction<int, int> playerPassWall;
    public LayerMask playerMask;

    public bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    private void OnTriggerExit(Collider other)
    {
        if(Contains(playerMask, other.gameObject.layer))
        {
            GameManager.Instance.IncreaseScore();
            playerPassWall?.Invoke(GameManager.Instance.scorePlayer, GameManager.Instance.recordScore);
        }
    }
}
