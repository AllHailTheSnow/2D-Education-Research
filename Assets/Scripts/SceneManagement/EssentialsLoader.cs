using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIControl;
    public GameObject GameManagers;
    public GameObject audioManager;
    public GameObject battleManager;

    private void Awake()
    {
        if (UIController.Instance == null)
        {
            Instantiate(UIControl);
        }

        if (GameManager.Instance == null)
        {
            Instantiate(GameManagers);
        }

        //if (AudioManager.Instance == null)
        //{
        //    Instantiate(audioManager);
        //}

        if (BattleManager.Instance == null)
        {
            Instantiate(battleManager);
        }
    }
}
