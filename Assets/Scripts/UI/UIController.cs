using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    private CharacterStats[] playerStats;

    [Header("Main Menu")]
    public GameObject menu;
    public GameObject[] windows;
    public string mainMenuName;

    [Header("Party Screen")]
    public GameObject[] charStatHolder;
    public TMP_Text[] nameText, hpText, mpText, lvlText;
    public Slider[] expSlider;
    public Image[] charImage;

    [Header("Status Screen")]
    public GameObject[] statusButtons;
    public TMP_Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmEqpd, statusArmPwr, statusExp;
    public Image statusImage;

    [Header("Item Screen")]
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public TMP_Text itemName, itemDescription, useButtonText;
    public GameObject itemCharChoiceMenu;
    public TMP_Text[] itemChoiceNames;
    public TMP_Text goldText;

    private void Update()
    {
        //Check if the game is in battle or in dialogue before allowing menu input
        if (BattleManager.Instance.battleActive || DialogueManager.Instance.dialoguePanel.activeInHierarchy)
        {
            return;
        }
        //listen for menu input
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            //Check if the menu is already open
            if (menu.activeSelf)
            {
                //close the menu
                CloseMenu();
            }
            else
            {
                //else open the menu
                menu.SetActive(true);
                UpdateMainStats();
                GameManager.Instance.gameMenuOpen = true;
            }

            AudioManager.Instance.PlaySFX(4);
        }
    }

    /*--------------------------------------------------------------------Update Main Method-------------------------------------------------------------------------*/

    public void UpdateMainStats()
    {
        //Get player stats from GameManager
        playerStats = GameManager.Instance.playerStats;

        //Loop through player stats and update UI elements
        for (int i = 0; i < playerStats.Length; i++)
        {
            //Check if player is active in the scene
            if (playerStats[i].gameObject.activeSelf)
            {
                //Activate character stat holder
                charStatHolder[i].SetActive(true);

                //Update UI elements with player stats values from GameManager
                nameText[i].text = playerStats[i].characterName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Lvl: " + playerStats[i].playerLevel;
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].characterImage;

            }
            else
            {
                //Deactivate character stat holder if player is not active in the scene
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = "¥" + GameManager.Instance.currentGold.ToString();
    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();

        for (int i = 0; i < windows.Length; i++)
        {
            if(i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeSelf);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }

    /*--------------------------------------------------------------------Party Screen Methods-----------------------------------------------------------------------*/

    public void OpenStatus()
    {
        UpdateMainStats();

        StatusChar(0);

        //Loop through player stats and update UI elements
        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeSelf);
            statusButtons[i].GetComponentInChildren<TMP_Text>().text = playerStats[i].characterName;
        }

    }

    public void StatusChar(int selected)
    {
        //set on screen status elements to the selected character status elements
        statusName.text = playerStats[selected].characterName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defense.ToString();

        if (playerStats[selected].equippedWpn != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWpn;
        }
        else
        {
            statusWpnEqpd.text = "None";
        }
        statusWpnPwr.text = playerStats[selected].wpnPwr.ToString();

        if (playerStats[selected].equippedArm != "")
        {
            statusArmEqpd.text = playerStats[selected].equippedArm;
        }
        else
        {
            statusArmEqpd.text = "None";
        }
        statusArmPwr.text = playerStats[selected].armPwr.ToString();

        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentEXP).ToString();
        statusImage.sprite = playerStats[selected].statusImage;
    }

    /*--------------------------------------------------------------------Item Screen Methods------------------------------------------------------------------------*/

    public void ShowItems()
    {
        GameManager.Instance.SortItems();

        //Loop through item buttons and update UI elements
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.Instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.Instance.GetItemDetails(GameManager.Instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.Instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        //check if the item is item
        if (activeItem.isItem)
        {
            //set the use button text to "Use"
            useButtonText.text = "Use";
        }

        //check if the item is weapon or armour
        if (activeItem.isWeapon || activeItem.isArmor)
        {
            //set the use button text to "Equip"
            useButtonText.text = "Equip";
        }

        //set the item name and description text to the active item name and description
        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        //check if the item is not null
        if (activeItem != null)
        {
            //remove the item from the inventory
            GameManager.Instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        //loop through item choice names and update UI elements
        for (int i = 0; i < itemChoiceNames.Length; i++)
        {
            itemChoiceNames[i].text = GameManager.Instance.playerStats[i].characterName;
            itemChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.Instance.playerStats[i].gameObject.activeSelf);
        }

    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharChoice();
    }

    /*--------------------------------------------------------------------Save Methods------------------------------------------------------------------------------*/

    public void SaveGame()
    {
        GameManager.Instance.SaveData();
        QuestManager.Instance.SaveQuestData();
    }

    /*--------------------------------------------------------------------Basic Menu Methods------------------------------------------------------------------------*/

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlaySFX(3);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        menu.SetActive(false);
        GameManager.Instance.gameMenuOpen = false;
    }

    public void MainMenu()
    {
        //Fill in for main menu
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        AudioManager.Instance.StopMusic();


        Destroy(GameManager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(AudioManager.Instance.gameObject);
        Destroy(BattleManager.Instance.gameObject);
        Destroy(CameraController.Instance.gameObject);
        Destroy(QuestManager.Instance.gameObject);
        Destroy(BaseSingleton.Instance.gameObject);
        Destroy(gameObject);
    }
}
