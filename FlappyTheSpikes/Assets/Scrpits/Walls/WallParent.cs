using UnityEngine;

public class WallParent : MonoBehaviour
{
    [SerializeField] Transform[] wallParts;

    public enum TypeWall { Bot,Top,Parent}

    public Transform GetWall(TypeWall type)
    {
        return wallParts[(int)type].transform;
    }

    public void SetWallPosition(Vector3 pos, TypeWall whatWall)
    {
        wallParts[(int)whatWall].transform.position = pos;
    }

    public Transform GetTransformWallParent()
    {
        return gameObject.transform;
    }
}
