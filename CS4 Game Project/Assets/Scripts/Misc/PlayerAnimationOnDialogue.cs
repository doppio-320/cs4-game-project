using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationOnDialogue : MonoBehaviour
{
    public string dialogueID;
    public string animationName;

    private void Start()
    {
        DialogueHandler.Instance.OnDialogueEnded += StartAnimation;
    }

    private void StartAnimation(DialoguePreset _dialogue, int _progress)
    {
        if(_dialogue.dialogueID == dialogueID)
        {            
            PlayerMain.Instance.gameObject.GetComponent<PlayerAnimation>().ForcePlayMiscAnimation(animationName);
        }        
    }
}
