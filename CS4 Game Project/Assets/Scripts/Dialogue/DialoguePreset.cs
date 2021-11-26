using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue")]
public class DialoguePreset : ScriptableObject
{
    public string dialogueID;
    public bool hasImages = true;
    public List<DialogueStep> dialogueSteps;
}

[System.Serializable]
public class DialogueStep
{
    public string personName;
    public string dialogueText;
    public Image dialogueImage;
}
