using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerAnimations : MonoBehaviour
{
    public GameObject idleAnimation;
    public GameObject walkAnimation;
    public GameObject jumpAnimation;

    private NormalPlayerController controller;

    private void Start()
    {
        controller = GetComponent<NormalPlayerController>();
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            if (controller.MovementVelocity.x > 0.1f || controller.MovementVelocity.x < -0.1f)
            {
                idleAnimation.SetActive(false);
                walkAnimation.SetActive(true);
            }
            else
            {
                idleAnimation.SetActive(true);
                walkAnimation.SetActive(false);
            }
            jumpAnimation.SetActive(false);
        }
        else
        {
            jumpAnimation.SetActive(true);
            idleAnimation.SetActive(false);
            walkAnimation.SetActive(false);
        }
    }
}
