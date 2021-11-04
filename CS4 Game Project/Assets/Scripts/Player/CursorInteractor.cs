using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteractor : MonoBehaviour
{
    private PlayerInteraction interactor;

    void Start()
    {
        interactor = GetComponentInParent<PlayerInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var toolTip = collision.GetComponent<TooltipObject>();
        if (toolTip != null)
        {
            interactor.SetInteractible(collision.gameObject);
            toolTip.SetInteractor(interactor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<TooltipObject>() != null)
        {
            interactor.RemoveInteractible();
        }
    }
}
