using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public static DialogueManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public GameObject dialogueUIPrefab;
    public GameObject dialogueUI;

    public int dialogueProgress;
    public Image backgroundImage;
    public Text nameText;
    public Text dialogueText;

    public DialoguePresent activeDialogue;

    public UnityEvent onDialogueStarted;
    public UnityEvent onDialogueFinished;

    private void Start()
    {
        if (!GameObject.Find("DialogueUI"))
        {
            dialogueUI = Instantiate(dialogueUIPrefab);            
        }
        DontDestroyOnLoad(dialogueUI);

        if (activeDialogue != null)
        {
            Initialize(activeDialogue);
        }
    }

    public void Initialize(DialoguePresent _diag)
    {
        onDialogueStarted.Invoke();

        GameHandler.Instance.isInDialogue = true;

        dialogueUI.SetActive(true);

        activeDialogue = _diag;
        dialogueProgress = 0;
        ProgressDiag();
    }

    public void ProgressDiag()
    {        
        if(activeDialogue.dialogue[dialogueProgress].background != null)
        {
            backgroundImage.sprite = activeDialogue.dialogue[dialogueProgress].background;
            backgroundImage.enabled = true;
        }
        else
        {
            backgroundImage.sprite = null;
            backgroundImage.enabled = false;
        }
        nameText.text = activeDialogue.dialogue[dialogueProgress].personName;
        dialogueText.text = activeDialogue.dialogue[dialogueProgress].talk;

        dialogueProgress++;          
    }

    public EventHandler<DialogueEventArguments> DialogueEnded;

    public void ExitDialogue()
    {
        DialogueEnded.Invoke(this, new DialogueEventArguments() { ID = activeDialogue.ID });
        activeDialogue = null;

        dialogueProgress = 0;
        dialogueUI.SetActive(false);

        GameHandler.Instance.isInDialogue = false;

        onDialogueFinished.Invoke();
    }

    public class DialogueEventArguments : EventArgs
    {
        public string ID { set; get; }
    }

    private void Update()
    {
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape) && dialogueUI.activeSelf)
        {
            if(dialogueProgress == activeDialogue.dialogue.Count)
            {
                ExitDialogue();
            }
            else
            {
                ProgressDiag();
            }            
        }        
    }
}
