using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    #region Instance Handling

    private static DialogueHandler instance;

    public static DialogueHandler Instance
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
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public int dialogueProgress;
    public DialoguePreset activePreset;

    private GameObject dialogueUIParent;
    private Text dialogueText;
    private Text nameText;
    private Image backgroundDisplay;

    private void Start()
    {
        dialogueUIParent = transform.Find("DialogueUI").gameObject;

        backgroundDisplay = dialogueUIParent.transform.Find("BackgroundDisplay").gameObject.GetComponent<Image>();
        dialogueText = dialogueUIParent.transform.Find("DialogueParent").Find("DialogueText").gameObject.GetComponent<Text>();
        nameText = dialogueUIParent.transform.Find("SubjectParent").Find("SubjectText").gameObject.GetComponent<Text>();

        dialogueUIParent.SetActive(false);

        if(activePreset != null)
        {
            StartDialogue(activePreset);
        }
    }

    private void Update()
    {
        if (!dialogueUIParent.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProgressDialogue();
        }
    }

    public void StartDialogue(DialoguePreset _diag)
    {
        GameHandler.Instance.SetDialogueState(true);

        activePreset = _diag;
        dialogueProgress = -1;

        dialogueUIParent.SetActive(true);
        backgroundDisplay.enabled = false;

        ProgressDialogue();

        if (OnDialogueStarted != null)
        {
            OnDialogueStarted(activePreset, dialogueProgress);
        }
    }

    private void ProgressDialogue()
    {
        dialogueProgress++;

        if (dialogueProgress == activePreset.dialogueSteps.Count)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = activePreset.dialogueSteps[dialogueProgress].dialogueText;
        nameText.text = activePreset.dialogueSteps[dialogueProgress].personName;
        LayoutRebuilder.ForceRebuildLayoutImmediate(nameText.transform.parent.GetComponent<RectTransform>());//Hack to fix content size refitter not updating 

        if (OnDialogueProgress != null)
        {
            OnDialogueProgress(activePreset, dialogueProgress);
        }
    }

    public void EndDialogue()
    {
        GameHandler.Instance.SetDialogueState(false);

        if (OnDialogueEnded != null)
        {
            OnDialogueEnded(activePreset, -1);
        }        

        activePreset = null;
        dialogueProgress = 0;

        dialogueUIParent.SetActive(false);        
    }

    public delegate void DialogueEvent(DialoguePreset _dialogue, int _progress);
    public DialogueEvent OnDialogueStarted;
    public DialogueEvent OnDialogueEnded;
    public DialogueEvent OnDialogueProgress;
}
