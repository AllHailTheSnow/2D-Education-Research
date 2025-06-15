using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP, affectMP, affectStr;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;

    public void Use(int charToUseOn)
    {

        CharacterStats selectedCharacter = GameManager.Instance.playerStats[charToUseOn];

        if(isItem)
        {
            if(affectHP)
            {
                selectedCharacter.currentHP += amountToChange;

                if (selectedCharacter.currentHP > selectedCharacter.maxHP)
                {
                    selectedCharacter.currentHP = selectedCharacter.maxHP;
                    //Debug.Log(selectedCharacter.characterName + " has " + selectedCharacter.currentHP + " HP");
                }

                if(BattleManager.Instance.battleActive)
                {
                    //Debug.Log(selectedCharacter.characterName + " has " + selectedCharacter.currentHP + " HP");
                    charToUseOn = BattleManager.Instance.currentTurn;
                    BattleManager.Instance.activeBattlers[charToUseOn].currentHP += amountToChange;

                    if (BattleManager.Instance.activeBattlers[charToUseOn].currentHP > BattleManager.Instance.activeBattlers[charToUseOn].maxHP)
                    {
                        BattleManager.Instance.activeBattlers[charToUseOn].currentHP = BattleManager.Instance.activeBattlers[charToUseOn].maxHP;
                    }
                }
            }

            if(affectMP)
            {
                selectedCharacter.currentMP += amountToChange;

                if(selectedCharacter.currentMP > selectedCharacter.maxMP)
                {
                    selectedCharacter.currentMP = selectedCharacter.maxMP;
                }

                if (BattleManager.Instance.battleActive)
                {
                    //Debug.Log(selectedCharacter.characterName + " has " + selectedCharacter.currentHP + " HP");
                    charToUseOn = BattleManager.Instance.currentTurn;
                    BattleManager.Instance.activeBattlers[charToUseOn].currentMP += amountToChange;

                    if (BattleManager.Instance.activeBattlers[charToUseOn].currentMP > BattleManager.Instance.activeBattlers[charToUseOn].maxMP)
                    {
                        BattleManager.Instance.activeBattlers[charToUseOn].currentMP = BattleManager.Instance.activeBattlers[charToUseOn].maxMP;
                    }
                }
            }

            if (affectStr)
            {
                selectedCharacter.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            if (selectedCharacter.equippedWpn != "")
            {
                GameManager.Instance.AddItem(selectedCharacter.equippedWpn);
            }

            selectedCharacter.equippedWpn = itemName;
            selectedCharacter.wpnPwr = weaponStrength;
        }

        if (isArmor)
        {
            if (selectedCharacter.equippedArm != "")
            {
                GameManager.Instance.AddItem(selectedCharacter.equippedArm);
            }

            selectedCharacter.equippedArm = itemName;
            selectedCharacter.armPwr = armorStrength;
        }

        GameManager.Instance.RemoveItem(itemName);
    }
}
