using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : TooltipObject
{
    public string actionText = "Use";

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + title);
    }
}
