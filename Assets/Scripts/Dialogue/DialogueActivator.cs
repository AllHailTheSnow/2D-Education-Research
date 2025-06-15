using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private string[] lines;
    [SerializeField] private string questToMark;
    [SerializeField] private bool markComplete;
    [SerializeField] private bool shouldActivateQuest;
    [SerializeField] private bool isPerson = true;

    private bool canActivate;

    private void Update()
    {
        if (canActivate && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!DialogueManager.Instance.dialoguePanel.activeSelf)
            {
                DialogueManager.Instance.ShowDialogue(lines, isPerson);
                DialogueManager.Instance.ShouldActiveQuestAtEnd(questToMark, markComplete);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            canActivate = false;
        }
    }

}
