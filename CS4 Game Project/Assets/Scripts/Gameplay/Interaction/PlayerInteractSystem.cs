using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractSystem : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.F;
    public float reachDistance;
    public GameObject currentTooltip;
    private bool alreadyEnabledInteractiveTooltip;    

    public void AddTooltip(GameObject _obj)
    {
        TooltipHandler.Instance.SetInteractKey(interactKey);

        RemoveTooltip();
        currentTooltip = _obj;

        TooltipHandler.Instance.EnableTooltip(currentTooltip.GetComponent<TooltipObject>());
    }

    private void Update()
    {
        if(currentTooltip != null)
        {
            if(Vector2.Distance(currentTooltip.transform.position, transform.position) <= reachDistance)
            {
                if(currentTooltip.GetComponent<InteractibleObject>() != null)
                {
                    if (!alreadyEnabledInteractiveTooltip)
                    {
                        alreadyEnabledInteractiveTooltip = true;
                        TooltipHandler.Instance.EnableTooltip(currentTooltip.GetComponent<InteractibleObject>());
                    }

                    if (Input.GetKeyDown(interactKey))
                    {
                        currentTooltip.GetComponent<InteractibleObject>().Interact();
                    }
                }
            }
            else
            {
                if (alreadyEnabledInteractiveTooltip)
                {
                    alreadyEnabledInteractiveTooltip = false;
                    TooltipHandler.Instance.EnableTooltip(currentTooltip.GetComponent<TooltipObject>());
                }
            }
        }
    }

    public void RemoveTooltip()
    {
        alreadyEnabledInteractiveTooltip = false;
        currentTooltip = null;
        TooltipHandler.Instance.ResetTooltip();
    }
}
