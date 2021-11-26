using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : TooltipObject
{
    public string action;

    public virtual void Interact()
    {
        //Debug.Log("Interacted with " + title);

        if(OnInteracted != null)
        {
            OnInteracted(this);
        }        
    }    

    public delegate void InteractibleObjectEvent(InteractibleObject _obj);
    public InteractibleObjectEvent OnInteracted;
}
