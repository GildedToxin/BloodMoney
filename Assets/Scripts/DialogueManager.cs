using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;

    [Header("Text Settings")]
    [SerializeField] private float textSpeed = 0.03f;


    [Header("Misc")]
    public List<Dialogue> dialogueConversations = new List<Dialogue>(); // change based on enum in switch statement
    private Dialogue currentDialogue;
    //private int conversationIndex = 0;  // replace with enum, change enum based on conditions (switch case or if statements)
    private conversationType currentConversationType;
    private string repeatedLine;
    private int currentLineIndex = 0;
    private int currentDisplayingText = 0;
    private bool DialogueActive = false;
    public bool testBoolCondition = false;
    public bool secondTestBoolCondition = false;
    private bool conversationStarted = false;
    private bool repeatLine = false;
    private bool canContinue = true;


    void Update()
    {
        // Player input to start and progress dialogue.
        if (Input.GetKeyDown(KeyCode.E) && canContinue)
        {
            canContinue = false;
            dialoguePanel.SetActive(true);
            ActivateText();
        }

        // Set current conversation using an Enum based on conditions
        if (testBoolCondition)
        {
            currentConversationType = conversationType.FirstDayConvo01;
        }
        if (secondTestBoolCondition)
        {
            currentConversationType = conversationType.FirstDayConvo02;
        }

        // Switch statement to set current conversation based on Enum value, mostly to prevent issues with setting more than one convo
        switch (currentConversationType)
        {
            case conversationType.FirstDayConvo01:
                currentDialogue = dialogueConversations[0];
                break;
            case conversationType.FirstDayConvo02:
                currentDialogue = dialogueConversations[1];
                break;
            default:
                Debug.LogError("Invalid conversation type!");
                break;
        }
    }

    // Logic for starting and progressing dialogue
    public void ActivateText() 
    {
        if (!conversationStarted)
        {
            currentLineIndex = 0;
            conversationStarted = true;
            StartCoroutine(AnimateText());
        }
        else
        {
            if (currentLineIndex < currentDialogue.lines.Length - 1)
            {
                currentLineIndex++;
                StartCoroutine(AnimateText());
            }
            else if (currentLineIndex == currentDialogue.lines.Length - 1)
            {
                EndDialogue();
                return;
            }
        }
    }

    // Stores each letter one at a time from the scriptable object and displays them in the dialogue box for a text scroll effect
    IEnumerator AnimateText()
    {
        string currentText = currentDialogue.lines[currentLineIndex].line;
        string repeatedText = currentDialogue.repeatLine.line;
        if (!repeatLine)
        {
            for (int i = 0; i < currentText.Length + 1; i++)
            {
                dialogueText.text = currentText.Substring(0, i);
                yield return new WaitForSeconds(textSpeed);
            }
            canContinue = true;
        }
        else
        {
            for (int i = 0; i < repeatedText.Length + 1; i++)
            {
                dialogueText.text = repeatedText.Substring(0, i);
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }

    // All end of dialogue logic should go here
    public void EndDialogue()
    {
        conversationStarted = false;
        canContinue = true;
        dialoguePanel.SetActive(false);
    }
}

public enum conversationType
{
    None,
    FirstDayConvo01,
    FirstDayConvo02,
    SecondDayConvo01,
    ThirdDayConvo01,
    FourthDayConvo01,
    FifthDayConvo01,
    SixthDayConvo01,
    SeventhDayConvo01,
    EighthDayConvo01,
    NinthDayConvo01,
    TenthDayConvo01,
}
