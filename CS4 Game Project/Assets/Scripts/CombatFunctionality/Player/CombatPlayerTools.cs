using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerTools : MonoBehaviour
{
    [Header("Dashing")]
    public int maximumDashesStored;
    public float finalDashCooldown;
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
        if (dashCooldown <= 0f)
        {
            if(availableDashes == 0)
            {
                availableDashes = maximumDashesStored;
            }
            else if(availableDashes > 0 && availableDashes != maximumDashesStored)
            {
                dashCooldown = finalDashCooldown;
                availableDashes = 0;
            }
        }
        else
        {
            dashCooldown -= Time.deltaTime;
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
            }
            else
            {
                dashCooldown = secondDashTimeMargin;
            }
            return true;
        }
        return false;
    }
}
