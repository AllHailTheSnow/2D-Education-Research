using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public TMP_Text amountText;
    public int buttonValue;

    public void Press()
    {
        if(UIController.Instance.menu.activeSelf)
        {
            if (GameManager.Instance.itemsHeld[buttonValue] != "")
            {
                UIController.Instance.SelectItem(GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[buttonValue]));
            }
        }

        if(Shop.Instance.shopMenu.activeSelf)
        {
            if(Shop.Instance.buyMenu.activeSelf)
            {
                Shop.Instance.SelectBuyItem(GameManager.Instance.GetItemDetails(Shop.Instance.itemsForSale[buttonValue]));
            }

            if (Shop.Instance.sellMenu.activeSelf)
            {
                Shop.Instance.SelectSellItem(GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[buttonValue]));
            }
        }

        if (BattleManager.Instance.itemMenu.activeSelf)
        {
            BattleManager.Instance.SelectItem(GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[buttonValue]));
        }
    }
}
