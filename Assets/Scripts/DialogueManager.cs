using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DialogueManager : MonoBehaviour
{
    public List<ScriptableObject> dialogueConversations = new List<ScriptableObject>();

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool conversationActive = false;
    private bool RepeatDialogue = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (conversationActive)
                NextLine();
            else
            {
                StartDialogue((Dialogue)dialogueConversations[0]); // Start the first dialogue for testing purposes
                conversationActive = true;
            }
                
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;

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
        DialogueLine line = currentDialogue.lines[currentLineIndex];

        Debug.Log(line.speakerName + ": " + line.line);

        // Update UI elements here
    }

    public void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        currentDialogue = null;
        conversationActive = false;
    }
}
