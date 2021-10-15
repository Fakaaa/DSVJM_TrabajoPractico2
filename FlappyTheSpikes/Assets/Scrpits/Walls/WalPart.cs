using UnityEngine;

public class WalPart : MonoBehaviour
{
    public void SetWallPartPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public Vector3 GetPositionWallPart()
    {
        return transform.position;
    }
}
