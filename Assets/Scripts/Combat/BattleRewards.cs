using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleRewards : Singleton<BattleRewards>
{
    public TMP_Text xpText;
    public TMP_Text itemText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned;

    public bool markQuestComplete;
    public string questToMark;

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            OpenRewardsScreen(100, new string[] { "Iron Sword", "Iron Armour" });
        }
#endif
    }

    public void OpenRewardsScreen(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;

        xpText.text = "You've earned " + xpEarned + " experience!";
        itemText.text = "";

        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

    public void CloseRewardsScrren()
    {
        for(int i = 0; i < GameManager.Instance.playerStats.Length; i++)
        {
            if (GameManager.Instance.playerStats[i].gameObject.activeSelf)
            {
                if (GameManager.Instance.playerStats[i].currentHP > 0)
                {
                    GameManager.Instance.playerStats[i].AddEXP(xpEarned);
                }                
            }
        }

        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.Instance.AddItem(rewardItems[i]);
        }

        rewardScreen.SetActive(false);
        GameManager.Instance.battleActive = false;

        if(markQuestComplete)
        {
            QuestManager.Instance.MarkQuestComplete(questToMark);
        }
    }
}
