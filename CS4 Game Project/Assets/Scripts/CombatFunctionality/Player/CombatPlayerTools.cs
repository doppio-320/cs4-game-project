using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerTools : MonoBehaviour
{
    [Header("Dashing")]
    public int maximumDashesStored;
    public float finalDashCooldown;
    public float failedConnectPenaltyMultiplier = 1f;
    public float secondDashTimeMargin;

    /*[Header("Dashing runtime")]*/
    private int availableDashes;
    private float dashCooldown;

    private void Start()
    {
        availableDashes = maximumDashesStored;
        dashCooldown = 0f;
    }

    private void Update()
    {
        if (GameHandler.Instance.pauseState != PauseState.None && GameHandler.Instance.pauseState != PauseState.Cutscene)
            return;

        if (dashCooldown <= 0f)
        {
            if(availableDashes == 0)
            {
                availableDashes = maximumDashesStored;
                PlayerCombatToolsDisplay.Instance.SetDashRemainingDashes(availableDashes);
                PlayerCombatToolsDisplay.Instance.SetDashAvailable(true);
            }
            else if(availableDashes > 0 && availableDashes != maximumDashesStored)
            {
                dashCooldown = finalDashCooldown * failedConnectPenaltyMultiplier;
                availableDashes = 0;
                PlayerCombatToolsDisplay.Instance.SetDashAvailable(false);
            }
        }
        else
        {
            dashCooldown -= Time.deltaTime;
            if(availableDashes == 0)
            {
                PlayerCombatToolsDisplay.Instance.SetDashCooldown(dashCooldown);
            }            
        }
    }

    public bool UseDash()
    {
        if (availableDashes == 0)
        {
            return false;
        }
        else if (availableDashes > 0)
        {
            availableDashes--;
            if (availableDashes == 0)
            {
                dashCooldown = finalDashCooldown;
                PlayerCombatToolsDisplay.Instance.SetDashAvailable(false);
            }
            else
            {
                dashCooldown = secondDashTimeMargin;
            }
            PlayerCombatToolsDisplay.Instance.SetDashRemainingDashes(availableDashes);
            return true;
        }
        return false;
    }
}
