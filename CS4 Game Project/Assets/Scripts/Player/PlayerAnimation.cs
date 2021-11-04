using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public float minimumMoveSpeed = 0.075f;

    private float playerSpeed;
    private Animator animator;
    private GameObject visualsObject;

    private float previousAnimatorSpeed;
    private bool isAlreadyPaused;

    private void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;
        animator = visualsObject.GetComponent<Animator>();

        previousAnimatorSpeed = animator.speed;
    }

    private void Update()
    {
        if(GameHandler.Instance.pauseState != PauseState.None)
        {
            if (!isAlreadyPaused)
            {
                previousAnimatorSpeed = animator.speed;
                animator.speed = 0;
                isAlreadyPaused = transform;
            }            
        }
        else
        {
            isAlreadyPaused = false;
            animator.speed = previousAnimatorSpeed;
        }

        if(playerSpeed >= minimumMoveSpeed)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void SetDirection(bool _left, bool _right)
    {
        if (_left)
        {
            visualsObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_right)
        {
            visualsObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void SetSpeed(float _speed)
    {
        playerSpeed = Mathf.Abs(_speed);
    }
}
