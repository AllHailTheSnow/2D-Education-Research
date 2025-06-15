using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats[] playerStats;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    public int currentGold;

    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;
    public bool shopActive;
    public bool battleActive;

    private void Start()
    {
        SortItems();
    }

    private void Update()
    {
        //Check if game menu is open, dialog is active, fading between areas, or shop is active
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            //If any of the above are true, player cannot move
            PlayerController.Instance.canMove = false;
        }
        else
        {
            //If none of the above are true, player can move
            PlayerController.Instance.canMove = true;
        }
    }

    /*--------------------------------------------------------------------Item Screen Methods------------------------------------------------------------------------*/

    public Item GetItemDetails(string itemToGrab)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }

        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while(itemAfterSpace)
        {
            itemAfterSpace = false;
            for(int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }

        if(foundSpace)
        {
            bool itemExists = false;

            for(int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }

            if(itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " does not exist!");
            }
        }

        UIController.Instance.ShowItems();

    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for(int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        if(foundItem)
        {
            numberOfItems[itemPosition]--;

            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }

            UIController.Instance.ShowItems();
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }
    }

    /*--------------------------------------------------------------------Save and Load Methods----------------------------------------------------------------------*/

    public void SaveData()
    {
        //Saves the current scene and player poistion in the scene
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.Instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.Instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.Instance.transform.position.z);

        //Save the player info of each character
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeSelf)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_currentEXP", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_currentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_maxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_currentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_maxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_defense", playerStats[i].defense);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_wpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_armPwr", playerStats[i].armPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_equippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_equippedArm", playerStats[i].equippedArm);
        }

        //Saves the inventory info
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    public void LoadData()
    {
        //Loads the player position
        PlayerController.Instance.transform.position = new Vector3(
            PlayerPrefs.GetFloat("Player_Position_x"), 
            PlayerPrefs.GetFloat("Player_Position_y"), 
            PlayerPrefs.GetFloat("Player_Position_z"));

        //Loads the player information for each character
        for (int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_currentEXP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_currentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_maxHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_currentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_maxMP");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_strength");
            playerStats[i].defense = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_defense");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_wpnPwr");
            playerStats[i].armPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_armPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_equippedWpn");
            playerStats[i].equippedArm = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_equippedArm");
        }

        //Loads the inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
