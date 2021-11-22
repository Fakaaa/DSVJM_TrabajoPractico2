using UnityEngine;

[System.Serializable]
public class ItemShop : MonoBehaviour
{
    #region EXPOSED_FIELDS
    public Color hisColor;
    public int costToGet;
    public bool alreadyBought;
    public bool selected;
    #endregion

    public ItemShop(ItemShop item)
    {
        hisColor = item.hisColor;
        costToGet = item.costToGet;
        alreadyBought = item.alreadyBought;
        selected = item.selected;
    }
}
