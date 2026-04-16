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
    public List<GameObject> infoPanels = new List<GameObject>();

    [Header("Text Settings")]
    [SerializeField] private float textSpeed = 0.03f;

    [Header("Misc")]
    public List<Dialogue> dialogueConversations = new List<Dialogue>(); // change based on enum in switch statement
    private Dialogue currentDialogue;
    private conversationType currentConversationType;
    private int completedConversations = 0; // may be used for tracking completed conversations for conditions
    private string repeatedLine;
    private int currentLineIndex = 0;
    private int currentDisplayingText = 0;
    private bool DialogueActive = false;
    public bool conversationStarted = false;
    private bool repeatLine = false;
    private bool canContinue = true;

    // if true during a conversation, after conversation ends it will briefly pause before setting next conversation
    // Pause is used for info panels, audio stings, etc. It will be progressed like normal dialogue input
    private bool pauseConvo = false;
    public bool extraActive = false;

    public AudioClip chimeSFX;
    public AudioClip infoSFX;

    void Update()
    {
        // Player input to start and progress dialogue.
        if (Input.GetKeyDown(KeyCode.E) && canContinue && FindAnyObjectByType<owner>().isLookedAt && !extraActive)
        {
            if (extraActive)
            {
                for (int i = 0; i < infoPanels.Count; i++)
                {
                    infoPanels[i].SetActive(false);
                }
                extraActive = false;
                StartConversation();
                return;
            }
            else
            {
                if (currentConversationType != conversationType.None)
                {
                    StartConversation();
                }
            }
        }

        // Set current conversation using an Enum based on conditions
        SetCurrentConversation();

        // Switch statement to set current conversation based on Enum value, mostly to prevent issues with setting more than one convo
        switch (currentConversationType)
        {
            case conversationType.FirstDayConvo01:
                currentDialogue = dialogueConversations[0];
                // set the speakerText
                speakerNameText.text = currentDialogue.lines[currentLineIndex].speakerName;
                pauseConvo = true;
                break;
            case conversationType.FirstDayConvo02:
                currentDialogue = dialogueConversations[1];
                pauseConvo = true;
                break;
            case conversationType.FirstDayConvo03:
                currentDialogue = dialogueConversations[2];
                pauseConvo = true;
                break;
            case conversationType.FirstDayConvo04:
                currentDialogue = dialogueConversations[3];
                pauseConvo = true;
                break;
            case conversationType.FirstDayConvo05:
                currentDialogue = dialogueConversations[4];
                pauseConvo = false;
                break;
            case conversationType.SecondDayConvo01:
                currentDialogue = dialogueConversations[5];
                pauseConvo = true;
                break;
            case conversationType.SecondDayConvo02:
                currentDialogue = dialogueConversations[6];
                pauseConvo = true;
                break;
            case conversationType.SecondDayConvo03:
                currentDialogue = dialogueConversations[7];
                pauseConvo = false;
                break;
            case conversationType.ThirdDayConvo01:
                currentDialogue = dialogueConversations[8];
                pauseConvo = true;
                break;
            case conversationType.ThirdDayConvo02:
                currentDialogue = dialogueConversations[9];
                pauseConvo = true;
                break;
            case conversationType.ThirdDayConvo03:
                currentDialogue = dialogueConversations[10];
                pauseConvo = false;
                break;
            case conversationType.FourthDayConvo01:
                currentDialogue = dialogueConversations[11];
                pauseConvo = true;
                break;
            case conversationType.FourthDayConvo02:
                currentDialogue = dialogueConversations[12];
                pauseConvo = true;
                break;
            case conversationType.FourthDayConvo03:
                currentDialogue = dialogueConversations[13];
                pauseConvo = false;
                break;
            case conversationType.FifthDayConvo01:
                currentDialogue = dialogueConversations[14];
                pauseConvo = true;
                break;
            case conversationType.FifthDayConvo02:
                currentDialogue = dialogueConversations[15];
                pauseConvo = true;
                break;
            case conversationType.FifthDayConvo03:
                currentDialogue = dialogueConversations[16];
                pauseConvo = false;
                break;
            case conversationType.SixthDayConvo01:
                currentDialogue = dialogueConversations[17];
                pauseConvo = true;
                break;
            case conversationType.SixthDayConvo02:
                currentDialogue = dialogueConversations[18];
                pauseConvo = true;
                break;
            case conversationType.SixthDayConvo03:
                currentDialogue = dialogueConversations[19];
                pauseConvo = false;
                break;
            case conversationType.SeventhDayConvo01:
                currentDialogue = dialogueConversations[20];
                pauseConvo = true;
                break;
            case conversationType.SeventhDayConvo02:
                currentDialogue = dialogueConversations[21];
                pauseConvo = false;
                break;
            case conversationType.EighthDayConvo01:
                currentDialogue = dialogueConversations[22];
                pauseConvo = true;
                break;
            case conversationType.EighthDayConvo02:
                currentDialogue = dialogueConversations[23];
                pauseConvo = false;
                break;
            case conversationType.NinthDayConvo01:
                currentDialogue = dialogueConversations[24];
                pauseConvo = true;
                break;
            case conversationType.NinthDayConvo02:
                currentDialogue = dialogueConversations[25];
                pauseConvo = false;
                break;
            case conversationType.TenthDayConvo01:
                currentDialogue = dialogueConversations[26];
                pauseConvo = true;
                break;
            case conversationType.TenthDayConvo02:
                currentDialogue = dialogueConversations[27];
                pauseConvo = false;
                break;
            default:
                Debug.Log("Invalid conversation type!");
                break;
        }
    }

    public void StartConversation()
    {
        canContinue = false;
        dialoguePanel.SetActive(true);
        ActivateText();
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

    IEnumerator WaitForAudioSting()
    {
        yield return new WaitForSeconds(2f); // length of audio sting
        StartConversation();
        extraActive = false;
    }
    IEnumerator WaitForInfoPanel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // length of audio sting
        HidePanel();
        StartConversation();

    }
    public void HidePanel()
    {
        for (int i = 0; i < infoPanels.Count; i++)
        {
            infoPanels[i].SetActive(false);
        }
        extraActive = false;
    }

    // All end of dialogue logic should go here
    public void EndDialogue()
    {
        conversationStarted = false;
        canContinue = true;
        dialoguePanel.SetActive(false);
        completedConversations++;

        // Used for extra info panels or audio stings between dialogue
        if (pauseConvo)
        {
            InfoPanel();
        }
    }

    public void InfoPanel() // Per day info panels and extras between dialogue
    {
        // Example of showing an info panel after a conversation ends, can be used for audio cues, etc.
        switch (currentConversationType)
        {
            case conversationType.FirstDayConvo01:
                extraActive = true;
                Debug.Log("Audio Sting plays here");
                // does StartConversation() after a delay
                StartCoroutine(WaitForAudioSting());
                break;
            case conversationType.FirstDayConvo02:
                extraActive = true;
                infoPanels[0].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(5f));
                break;
            case conversationType.FirstDayConvo03:
                extraActive = true;
                infoPanels[1].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.FirstDayConvo04:
                extraActive = true;
                infoPanels[2].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.SecondDayConvo01:
                extraActive = true;
                infoPanels[3].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.SecondDayConvo02:
                extraActive = true;
                infoPanels[4].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.ThirdDayConvo01:
                extraActive = true;
                infoPanels[5].SetActive(true);
                    AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.ThirdDayConvo02:
                extraActive = true;
                infoPanels[6].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.FourthDayConvo01:
                extraActive = true;
                infoPanels[7].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.FourthDayConvo02:
                extraActive = true;
                infoPanels[8].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.FifthDayConvo01:
                extraActive = true;
                infoPanels[9].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.FifthDayConvo02:
                extraActive = true;
                infoPanels[10].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.SixthDayConvo01:
                extraActive = true;
                infoPanels[11].SetActive(true);
                AudioPool.Instance.PlayClip2D(infoSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.SixthDayConvo02:
                extraActive = true;
                infoPanels[12].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.SeventhDayConvo01:
                extraActive = true;
                infoPanels[13].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(4f));
                break;
            case conversationType.EighthDayConvo01:
                extraActive = true;
                infoPanels[14].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.NinthDayConvo01:
                extraActive = true;
                infoPanels[15].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
            case conversationType.TenthDayConvo01:
                extraActive = true;
                infoPanels[16].SetActive(true);
                AudioPool.Instance.PlayClip2D(chimeSFX);
                StartCoroutine(WaitForInfoPanel(2f));
                break;
        }
    }

    public void SetCurrentConversation() // this could probably be optimized lmao
    {
        if (GameManager.Instance.currentDay == 0) // Day 1 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.FirstDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.FirstDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.FirstDayConvo03;
                    break;
                case 3:
                    currentConversationType = conversationType.FirstDayConvo04;
                    break;
                case 4:
                    currentConversationType = conversationType.FirstDayConvo05;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 1) // Day 2 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.SecondDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.SecondDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.SecondDayConvo03;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 2) // Day 3 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.ThirdDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.ThirdDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.ThirdDayConvo03;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 3) // Day 4 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.FourthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.FourthDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.FourthDayConvo03;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 4) // Day 5 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.FifthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.FifthDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.FifthDayConvo03;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 5) // Day 6 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.SixthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.SixthDayConvo02;
                    break;
                case 2:
                    currentConversationType = conversationType.SixthDayConvo03;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 6) // Day 7 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.SeventhDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.SeventhDayConvo02;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 7) // Day 8 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.EighthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.EighthDayConvo02;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 8) // Day 9 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.NinthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.NinthDayConvo02;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }

        if (GameManager.Instance.currentDay == 9) // Day 10 conversation conditions
        {
            switch (completedConversations)
            {
                case 0:
                    currentConversationType = conversationType.TenthDayConvo01;
                    break;
                case 1:
                    currentConversationType = conversationType.TenthDayConvo02;
                    break;
                default:
                    Debug.Log("Ran out of conversations");
                    break;
            }
        }
    }
}

public enum conversationType
{
    None,
    FirstDayConvo01,
    FirstDayConvo02,
    FirstDayConvo03,
    FirstDayConvo04,
    FirstDayConvo05,
    SecondDayConvo01,
    SecondDayConvo02,
    SecondDayConvo03,
    ThirdDayConvo01,
    ThirdDayConvo02,
    ThirdDayConvo03,
    FourthDayConvo01,
    FourthDayConvo02,
    FourthDayConvo03,
    FifthDayConvo01,
    FifthDayConvo02,
    FifthDayConvo03,
    SixthDayConvo01,
    SixthDayConvo02,
    SixthDayConvo03,
    SeventhDayConvo01,
    SeventhDayConvo02,
    EighthDayConvo01,
    EighthDayConvo02,
    NinthDayConvo01,
    NinthDayConvo02,
    TenthDayConvo01,
    TenthDayConvo02,
}
