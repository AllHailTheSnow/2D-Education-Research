using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;
    public int activeBattleTarget;
    public TMP_Text targetName;

    public void Press()
    {
        BattleManager.Instance.PlayerMagicAttack(moveName, activeBattleTarget);
    }
}
