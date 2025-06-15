using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;

    [SerializeField] private string questToCheck;

    [SerializeField] private bool activeIfComplete;

    private bool initialCheckDone;

    private void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (QuestManager.Instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
