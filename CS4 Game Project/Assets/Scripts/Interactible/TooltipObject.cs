using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipObject : MonoBehaviour
{
    public string title;

    private PlayerInteraction interactor;

    public void SetInteractor(PlayerInteraction _interactor)
    {
        interactor = _interactor;
    }

    private void OnDestroy()
    {
        if (interactor)
        {
            interactor.RemoveInteractible();
        }        
    }
}
