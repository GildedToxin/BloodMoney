using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;


    [Header("Misc")]
    public List<Dialogue> dialogueConversations = new List<Dialogue>();
    private Dialogue currentDialogue;
    private int conversationIndex = 0;
    private string repeatedLine;
    private int currentLineIndex = 0;
    private bool DialogueActive = false;
    public bool testBoolCondition = false;
    public bool secondTestBoolCondition = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueActive)
            {
                NextLine();
            }
            else
            {
                switch (conversationIndex)
                {
                    case 0:
                        if (testBoolCondition)
                            StartDialogue(dialogueConversations[0]);
                        else
                            goto default;
                        break;
                    case 1:
                        if (secondTestBoolCondition)
                            StartDialogue(dialogueConversations[1]);
                        else
                            RepeatLine();
                        break;
                    default:
                        if (repeatedLine != null)
                            RepeatLine();
                        else
                            Debug.Log("No conversation available");
                        break;
                }
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        DialogueActive = true;
        currentLineIndex = 0;
        currentDialogue = dialogue;
        DisplayCurrentLine();
    }

    public void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    public void DisplayCurrentLine()
    {
        if(currentLineIndex >= currentDialogue.lines.Length)
        {
            currentLineIndex = currentDialogue.lines.Length - 1;
            return;
        }
        else
            Debug.Log(currentDialogue.lines[currentLineIndex].speakerName + ": " + currentDialogue.lines[currentLineIndex].line);

        // Update UI here
        speakerNameText.text = currentDialogue.lines[currentLineIndex].speakerName;
        dialogueText.text = currentDialogue.lines[currentLineIndex].line;
    }

    public void EndDialogue()
    {
        DialogueActive = false;
        conversationIndex++;
        repeatedLine = currentDialogue.repeatLine.line;
        Debug.Log(currentDialogue.repeatLine.speakerName + ": " + repeatedLine);
    }

    public void RepeatLine()
    {
        Debug.Log(currentDialogue.repeatLine.speakerName + ": " + repeatedLine);
        DisplayCurrentLine();
    }
}
