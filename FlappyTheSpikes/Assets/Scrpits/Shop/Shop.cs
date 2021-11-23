using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Shop : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] public TextMeshProUGUI actualColorDisplay;
    [SerializeField] public TextMeshProUGUI showCurrencyNeded;
    [SerializeField] public TextMeshProUGUI currencyPlayer;
    [SerializeField] public Button buyItem;
    [SerializeField] public Button chooseItem;
    [SerializeField] public PlayerColorPreview previewColor;
    public UnityAction<int, ItemShop> OnBuyItem;
    public UnityAction<ItemShop> OnSelectItem;
    #endregion

    #region PRIVATE_FIELDS
    private ItemShop selectedItem;
    private ItemShop lastItemBought;

    private UnityAction OnNextItemShop;
    private UnityAction OnPrevItemShop;
    private GameManager gmRef;
    Dictionary<int, ItemShop> colorsAviableDic = new Dictionary<int, ItemShop>();
    int actualID = 0;
    #endregion

    void Start()
    {
        gmRef = GameManager.Instance;

        OnNextItemShop += UpdateShop;
        OnPrevItemShop += UpdateShop;

        UpdateShop();
    }

    public void SelectActualIdAsItem()
    {
        if(GetColorFromDic(actualID).alreadyBought)
        {
            if(!GetColorFromDic(actualID).selected)
            {
                selectedItem = GetColorFromDic(actualID);
                GetColorFromDic(actualID).selected = true;
                OnSelectItem?.Invoke(selectedItem);

                previewColor.UpdatePreviewColor(selectedItem.hisColor);
            }
        }

        UpdateShop();
    }

    public void BuyItem()
    {
        if (gmRef.currencyPlayer > GetColorFromDic(actualID).costToGet)
        {
            gmRef.currencyPlayer -= GetColorFromDic(actualID).costToGet;

            lastItemBought = GetColorFromDic(actualID);
            lastItemBought.alreadyBought = true;
            OnBuyItem?.Invoke(actualID, lastItemBought);
        }

        UpdateShop();
    }

    public void SearchNextItemShop()
    {
        if (actualID < colorsAviableDic.Count - 1)
            actualID++;
        else
            actualID = 0;

        OnNextItemShop?.Invoke();
    }

    public void SearchPrevItemShop()
    {
        if (actualID > 0)
            actualID--;
        else
            actualID = colorsAviableDic.Count - 1;

        OnPrevItemShop?.Invoke();
    }

    public void UpdateShop()
    {
        if (GetColorFromDic(actualID) != null)
        {
            if (GetColorFromDic(actualID).alreadyBought)
            {
                chooseItem.gameObject.SetActive(true);
                buyItem.gameObject.SetActive(false);
            }
            else
            {
                chooseItem.gameObject.SetActive(false);
                buyItem.gameObject.SetActive(true);
            }

            currencyPlayer.text = "$" + gmRef.currencyPlayer;
            showCurrencyNeded.text = "$" + GetColorFromDic(actualID).costToGet;
            actualColorDisplay.text = GetColorFromDic(actualID).name;

            previewColor.UpdatePreviewColor(GetColorFromDic(actualID).hisColor);
        }
    }

    public void SaveColorAviableOnDic(int id, ItemShop newColor)
    {
        if(id != -1)
        {
            colorsAviableDic[id] = newColor;
        }
    }

    public ItemShop GetColorFromDic(int id)
    {
        foreach (int actualID in colorsAviableDic.Keys)
        {
            if(id == actualID)
            {
                return colorsAviableDic[actualID];
            }
        }

        return null;
    }
}
