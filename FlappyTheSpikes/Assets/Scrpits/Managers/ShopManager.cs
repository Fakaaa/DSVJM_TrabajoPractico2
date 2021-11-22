using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private List<ItemShop> existenItems;
    [SerializeField] private Shop shopCanvas;
    #endregion

    #region PRIVATE_FIELDS
    private GameManager gmReference;
    private List<int> indicesUnbuyedItems = new List<int>();
    private List<int> indicesBuyedItems = new List<int>();
    #endregion

    void Start()
    {
        gmReference = GameManager.Instance;
        
        InitializeShop();

        shopCanvas.OnBuyItem += gmReference.SaveItemPlayer;
        shopCanvas.OnSelectItem += ChooseItemShop;
    }

    void InitializeShop()
    {
        FilterBoughtItems();

        if (gmReference.itemsBought.Count > 0)
        {
            for (int i = 0; i < indicesBuyedItems.Count; i++)
            {
                shopCanvas.SaveColorAviableOnDic(indicesBuyedItems[i], gmReference.itemsBought[indicesBuyedItems[i]]);
            }
            for (int i = 0; i < indicesUnbuyedItems.Count; i++)
            {
                shopCanvas.SaveColorAviableOnDic(indicesUnbuyedItems[i], existenItems[indicesUnbuyedItems[i]]);
            }
        }
        else
        {
            for (int i = 0; i < existenItems.Count; i++)
            {
                shopCanvas.SaveColorAviableOnDic(i, existenItems[i]);
            }
        }

        indicesUnbuyedItems.Clear();
        indicesBuyedItems.Clear();
    }

    public void FilterBoughtItems()
    {
        if (gmReference.itemsBought.Count < 1)
            return;

        foreach (int idItem in gmReference.itemsBought.Keys)
        {
            for (int j = 0; j < existenItems.Count; j++)
            {
                if(gmReference.itemsBought[idItem].name != existenItems[j].name)
                {
                    indicesUnbuyedItems.Add(j);
                }
                else
                {
                    indicesBuyedItems.Add(idItem);
                }
            }
        }
    }

    public void ChooseItemShop(ItemShop selectedItem)
    {
        if(gmReference != null)
        {
            if(gmReference.colorPlayerSelected != null)
                gmReference.colorPlayerSelected.selected = false;

            gmReference.colorPlayerSelected = selectedItem;
        }
    }
}
