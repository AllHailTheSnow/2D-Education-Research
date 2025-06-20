using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleNotification : MonoBehaviour
{
    public float awakeTime;
    private float awakeCounter;
    public TMP_Text theText;

    private void Update()
    {
        if(awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if(awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
