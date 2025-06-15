using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject dialoguePanel;
    public GameObject namePanel;

    public string[] dialogueLines;

    public int currentLine;

    private bool justStarted;
    private bool markQuestComplete;
    private bool shouldMarkQuest;
    private string questToMark;

    void Start()
    {
        //set dialogue text to current line
        dialogueText.text = dialogueLines[currentLine];
    }

    void Update()
    {
        //if the dialogue panel is active, check for input
        if (dialoguePanel.activeSelf == true)
        {
            //if the player presses the e key, move to the next line
            if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                //if not the first line, move to the next line
                if (!justStarted)
                {
                    //move to the next line
                    currentLine++;

                    //check if the current line is the last line
                    if (currentLine >= dialogueLines.Length)
                    {
                        //if it is, set the dialogue panel to inactive
                        dialoguePanel.SetActive(false);

                        GameManager.Instance.dialogActive = false;

                        //check if the quest should be marked
                        if (shouldMarkQuest)
                        {
                            //if it is, mark the quest as complete or incomplete
                            shouldMarkQuest = false;

                            if (markQuestComplete)
                            {
                                QuestManager.Instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.Instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        //set the dialogue text to the current line
                        dialogueText.text = dialogueLines[currentLine];
                    }
                }
                else
                {
                    //set just started to false
                    justStarted = false;
                }

            }
        }
    }

    public void ShowDialogue(string[] newLines, bool isPerson)
    {
        dialogueLines = newLines;

        currentLine = 0;

        CheckIfName();

        dialogueText.text = dialogueLines[currentLine];
        dialoguePanel.SetActive(true);

        justStarted = true;

        namePanel.SetActive(isPerson);

        GameManager.Instance.dialogActive = true;
    }

    public void CheckIfName()
    {
        //check if the current line starts with "n-"
        if (dialogueLines[currentLine].StartsWith("n-"))
        {
            //if it does, set the name text to the current line without "n-"
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            //move to the next line
            currentLine++;
        }
    }

    public void ShouldActiveQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }
}
