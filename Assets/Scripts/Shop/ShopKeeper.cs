using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;

    public string[] ItemsForSale = new string[40];

    private void Update()
    {
        // Check if the player is in range and the E key is pressed
        if (canOpen && Keyboard.current.eKey.wasPressedThisFrame && !Shop.Instance.shopMenu.activeSelf)
        {
            // Open the shop menu and pass the items for sale
            Shop.Instance.itemsForSale = ItemsForSale;

            Shop.Instance.OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has entered the trigger area
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player has exited the trigger area
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canOpen = false;
        }
    }
}
