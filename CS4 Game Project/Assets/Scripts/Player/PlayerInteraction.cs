using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float playerHeight;
    public float reachDistance = 2f;
    public KeyCode interactKey = KeyCode.F;
    public GameObject currentSelected;
    private GameObject cursorInteractor;

    private bool setInteractibleAlready;

    private void Start()
    {
        cursorInteractor = transform.Find("CursorInteract").gameObject;

        playerHeight = GetComponent<PlayerMain>().playerHeight;
    }

    private void Update()
    {
        if (GameHandler.Instance.pauseState != PauseState.None)
            return;

        cursorInteractor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!currentSelected)
            return;

        var interactible = currentSelected.GetComponent<InteractibleObject>();

        if (interactible != null)
        {
            if (Vector3.Distance(transform.position + Vector3.up * playerHeight, currentSelected.transform.position) <= reachDistance)
            {
                if (!setInteractibleAlready)
                {
                    TooltipHandler.Instance.SetTooltip(interactible, interactKey);
                    setInteractibleAlready = true;
                }

                if (Input.GetKeyDown(interactKey))
                {
                    interactible.Interact();
                }
            }
            else
            {
                if (setInteractibleAlready)
                {
                    TooltipHandler.Instance.EndTooltip();
                    TooltipHandler.Instance.SetTooltip(currentSelected.GetComponent<TooltipObject>());
                    setInteractibleAlready = false;
                }
            }
        }
    }

    public void SetInteractible(GameObject _object)
    {
        currentSelected = _object;
        TooltipHandler.Instance.SetTooltip(currentSelected.GetComponent<TooltipObject>());
    }

    public void RemoveInteractible()
    {
        currentSelected = null;
        TooltipHandler.Instance.EndTooltip();
        setInteractibleAlready = false;
    }
}
