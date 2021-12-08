using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnInteract : MonoBehaviour
{
    public DialoguePreset preset;
    private InteractibleObject interactibleObject;

    private void OnEnable()
    {
        interactibleObject = GetComponent<InteractibleObject>();

        interactibleObject.OnInteracted += StartDialogue;        
    }

    void StartDialogue(InteractibleObject _io)
    {
        DialogueHandler.Instance.StartDialogue(preset);        
    }

    private void OnDisable()
    {
        interactibleObject.OnInteracted -= StartDialogue;
    }
}
