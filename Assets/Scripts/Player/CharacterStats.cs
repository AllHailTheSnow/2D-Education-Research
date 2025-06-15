using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStats : MonoBehaviour
{
    //Character stats for leveling
    public string characterName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 300;
    public int baseMP = 20;

    //Character stats for combat
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLevelBonus;
    public int strength;
    public int defense;
    public int wpnPwr;
    public int armPwr;
    public string equippedWpn;
    public string equippedArm;
    public Sprite characterImage;
    public Sprite statusImage;

    private void Start()
    {
        //initialize exp to next level array
        expToNextLevel = new int[maxLevel];
        //set first element of exp to next level array to base exp
        expToNextLevel[1] = baseEXP;
        //initialize mp level bonus array
        mpLevelBonus = new int[maxLevel];
        //set first element of mp level bonus array to 20
        mpLevelBonus[1] = baseMP;

        //loop through exp to next level array and calculate exp to next level based on previous level
        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.CeilToInt(expToNextLevel[i - 1] * 1.05f);
        }

        //loop through mp level bonus array and calculate mp level bonus based on previous level
        for (int j = 2; j < mpLevelBonus.Length; j++)
        {
            mpLevelBonus[j] = Mathf.CeilToInt(mpLevelBonus[j - 1] * 1.05f);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            AddEXP(10000);
        }
#endif
    }

    public void AddEXP(int expToAdd)
    {
        //add exp to players current exp
        currentEXP += expToAdd;

        //if player level is less than max level
        if (playerLevel < maxLevel)
        {
            //if current exp is greater than exp to next level
            if (currentEXP > expToNextLevel[playerLevel])
            {
                //subtract exp to next level from current exp
                currentEXP -= expToNextLevel[playerLevel];
                //increment player level
                playerLevel++;

                //determine whether to increase strength or defense based on player level
                if (playerLevel % 2 == 0)
                {
                    int rand = Random.Range(1, 3);
                    strength += rand;
                }
                else
                {
                    defense++;
                }

                //increase max hp and mp based on player level and mp level bonus
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                maxMP += mpLevelBonus[playerLevel];
                currentMP = maxMP;
            }
        }

        //if player level is greater than or equal to max level set current exp to 0
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
