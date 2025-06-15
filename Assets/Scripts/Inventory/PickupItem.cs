using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    private bool canPickup;

    private void Update()
    {
        if(canPickup && Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameManager.Instance.AddItem(GetComponent<Item>().itemName);
            AudioManager.Instance.PlaySFX(10);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canPickup = false;
        }
    }
}
