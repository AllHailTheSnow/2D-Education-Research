using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestMarker : MonoBehaviour
{
    [SerializeField] private string questToMark;
    [SerializeField] private bool markComplete;
    [SerializeField] private bool markOnEnter;
    [SerializeField] private bool deactivateOnMarking;

    private bool canMark;

    private void Update()
    {
        if (canMark && Keyboard.current.eKey.wasPressedThisFrame)
        {
            canMark = false;
            MarkQuest();
        }
    }

    public void MarkQuest()
    {
        if (markComplete)
        {
            QuestManager.Instance.MarkQuestComplete(questToMark);
        }
        else
        {
            QuestManager.Instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canMark = false;
        }
    }
}