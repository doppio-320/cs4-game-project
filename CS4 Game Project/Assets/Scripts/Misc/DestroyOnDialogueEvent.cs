using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDialogueEvent : MonoBehaviour
{    
    public DialogueEventType eventType;
    public string dialogueID;
    public int destroyOnProgress;

    void Start()
    {
        if (eventType == DialogueEventType.Ended)
        {
            DialogueHandler.Instance.OnDialogueEnded += (_dialogue, _progress) =>
            {
                Destroy(gameObject);
            };
        }
        if (eventType == DialogueEventType.Progress)
        {
            DialogueHandler.Instance.OnDialogueProgress += (_dialogue, _progress) =>
            {
                if(_progress == destroyOnProgress)
                {
                    Destroy(gameObject);
                }                
            };
        }
        if (eventType == DialogueEventType.Started)
        {
            DialogueHandler.Instance.OnDialogueStarted += (_dialogue, _progress) =>
            {
                Destroy(gameObject);
            };
        }
    }

    public enum DialogueEventType
    {
        Started,
        Progress,
        Ended
    }
}
