using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string conversationName; // Name of the conversation, used for organization
    public DialogueLine[] lines; // Array of lines in the conversation
    public DialogueLine repeatLine; // Line that is repeated after the conversation is over
}

[System.Serializable]
public struct DialogueLine // Struct for easy creation of dialogue lines in the inspector
{
    public string speakerName; // Name of the speaker, used to determine which portrait to show and for display purposes
    public string line; // The actual line of dialogue that will be displayed
}


