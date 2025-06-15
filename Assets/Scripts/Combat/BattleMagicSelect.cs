using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleMagicSelect : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public TMP_Text nameText;
    public TMP_Text costText;

    public void Press()
    {
        if (BattleManager.Instance.activeBattlers[BattleManager.Instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.Instance.magicMenu.SetActive(false);
            BattleManager.Instance.OpenTargetMenu(spellName);
            BattleManager.Instance.activeBattlers[BattleManager.Instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            BattleManager.Instance.battleNotice.theText.text = "Not Enough MP!";
            BattleManager.Instance.battleNotice.Activate();
            BattleManager.Instance.magicMenu.SetActive(false);
        }
    }
}
