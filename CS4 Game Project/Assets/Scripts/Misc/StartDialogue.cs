using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public DialoguePresent dialogue;

    public void ExecuteDialogue()
    {
        DialogueManager.Instance.Initialize(dialogue);
    }
}
