using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : Singleton<Shop>
{
    [Header("Shop Menus")]
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    [Header("Gold Text")]
    public TMP_Text goldText;
    
    [Header("Items For Sale")]
    public string[] itemsForSale;

    [Header("Buttons")]
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;
    
    [Header("Selected Item LEAVE BLANK")]
    public Item selectedItem;
    
    [Header("BuyText")]
    public TMP_Text buyItemName;
    public TMP_Text buyItemDescription;
    public TMP_Text buyItemValue;

    [Header("SellText")]
    public TMP_Text sellItemName;
    public TMP_Text sellItemDescription;
    public TMP_Text sellItemValue;

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            OpenShop();
        }
#endif
    }

    public void OpenShop()
    {
        // Check if the player is in a shop area
        shopMenu.SetActive(true);
        //open the buy menu by default
        OpenBuyMenu();

        // Set the shop to active
        GameManager.Instance.shopActive = true;

        // Update the gold text
        goldText.text = "¥" + GameManager.Instance.currentGold.ToString();
    }

    public void CloseShop()
    {
        // Close the shop menu
        shopMenu.SetActive(false);
        // set the shop to inactive
        GameManager.Instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();

        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        // loop through the items for sale and set the buttons
        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            // set the button value to the index
            buyItemButtons[i].buttonValue = i;

            // check if the item is for sale
            if (itemsForSale[i] != "")
            {
                // set the button image and text and sprite
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.Instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                // if the item is not for sale, set the button image to inactive
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();

        sellMenu.SetActive(true);
        buyMenu.SetActive(false);

        ShowSellItems();
    }

    private void ShowSellItems()
    {
        GameManager.Instance.SortItems();

        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;

            if (GameManager.Instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = GameManager.Instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;

        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Total: ¥" + selectedItem.value.ToString();

    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;

        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: ¥" + Mathf.FloorToInt(selectedItem.value * .5f).ToString();
    }

    public void BuyItem()
    {
        if(selectedItem != null)
        {
            if(GameManager.Instance.currentGold >= selectedItem.value)
            {
                GameManager.Instance.currentGold -= selectedItem.value;

                GameManager.Instance.AddItem(selectedItem.itemName);
            }
        }

        goldText.text = "¥" + GameManager.Instance.currentGold.ToString();
    }

    public void SellItem()
    {
        if(selectedItem != null)
        {
            GameManager.Instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);

            GameManager.Instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = "¥" + GameManager.Instance.currentGold.ToString();

        ShowSellItems();
    }
}
