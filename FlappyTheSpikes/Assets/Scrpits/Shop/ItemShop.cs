using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializableColor  //Clase necesaria para serializar colores :/
{

    public float[] colorStore = new float[4] { 1F, 1F, 1F, 1F };
    public Color Color
    {
        get { return new Color(colorStore[0], colorStore[1], colorStore[2], colorStore[3]); }
        set { colorStore = new float[4] { value.r, value.g, value.b, value.a }; }
    }

    public static implicit operator Color(SerializableColor instance)
    {
        return instance.Color;
    }

    public static implicit operator SerializableColor(Color color)
    {
        return new SerializableColor { Color = color };
    }
}

[System.Serializable]
public class ItemShop
{
    #region EXPOSED_FIELDS
    public string name;
    public SerializableColor hisColor;
    public int costToGet;
    public int idInShop;
    public bool alreadyBought;
    public bool selected;
    #endregion

    public ItemShop(ItemShop item)
    {
        hisColor = item.hisColor;
        costToGet = item.costToGet;
        alreadyBought = item.alreadyBought;
        selected = item.selected;
        name = item.name;
        idInShop = item.idInShop;
    }
}

[System.Serializable]
public class ItemsBoughtData
{
    public ItemShop[] items;

    public ItemsBoughtData(List<ItemShop> allItemsBuyed)
    {
        items = new ItemShop[allItemsBuyed.Count];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = allItemsBuyed[i];
        }
    }
}
