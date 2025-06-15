using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleItemTargetMenu : MonoBehaviour
{
    public TMP_Text playerName;
    public int activeBattleTarget;

    public void Press()
    {
        //BattleManager.Instance.UseItem(activeBattleTarget);
    }
}
