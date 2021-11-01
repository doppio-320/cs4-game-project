using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMaker : InteractibleObject
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Just had a coffee!");
        CursorInteraction.playerInteractSystem.RemoveTooltip();
        Destroy(this);
    }
}
