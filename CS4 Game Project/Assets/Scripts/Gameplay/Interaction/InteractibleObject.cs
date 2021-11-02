using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractibleObject : TooltipObject
{
    public string actionText = "Use";

    public UnityEvent onInteract;

    public virtual void Interact()
    {
        onInteract.Invoke();
    }
}
