using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDialogueEvent : MonoBehaviour
{    
    public DialogueEventType eventType;
    public string dialogueID;
    public int destroyOnProgress;

    //Lousy feature embedded here instead: Optionally disable the object and renable [010222]
    [Header("Temporary Disable")]
    public bool tempDisable;
    public float timeDisable;

    void Start()
    {        
        if (eventType == DialogueEventType.Ended)
        {
            DialogueHandler.Instance.OnDialogueEnded += (_dialogue, _progress) =>
            {
                if(_dialogue.dialogueID == dialogueID)
                {                    
                    Destroy();
                }                
            };
        }
        if (eventType == DialogueEventType.Progress)
        {
            DialogueHandler.Instance.OnDialogueProgress += (_dialogue, _progress) =>
            {
                if(_progress == destroyOnProgress)
                {
                    if (_dialogue.dialogueID == dialogueID)
                    {
                        Destroy();
                    }
                }                
            };
        }
        if (eventType == DialogueEventType.Started)
        {
            DialogueHandler.Instance.OnDialogueStarted += (_dialogue, _progress) =>
            {
                if (_dialogue.dialogueID == dialogueID)
                {
                    Destroy();
                }
            };
        }
    }

    private void Destroy()
    {
        if(tempDisable && timeDisable > 0f)
        {
            Invoke("ReEnable", timeDisable);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if(OnDestroyTriggered != null)
        {
            OnDestroyTriggered();
        }
    }

    private void ReEnable()
    {
        gameObject.SetActive(true);
        
        if(OnReEnabled != null)
        {
            OnReEnabled();
        }
    }

    public delegate void DestroyOnDialogueEventHandler();
    public DestroyOnDialogueEventHandler OnDestroyTriggered;
    public DestroyOnDialogueEventHandler OnReEnabled;

    public enum DialogueEventType
    {
        Started,
        Progress,
        Ended
    }
}
