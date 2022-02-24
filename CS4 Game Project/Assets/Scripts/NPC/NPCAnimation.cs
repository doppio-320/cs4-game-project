using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public float minimuzMoveSpeed;

    private GameObject visualsObject;
    private Animator animator;

    private float moveSpeed;     

    private bool isAlreadyPaused;
    private float previousAnimatorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;
        animator = visualsObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameHandler.Instance.pauseState == PauseState.PauseMenu)
        {
            if(!isAlreadyPaused)
            {
                previousAnimatorSpeed = animator.speed;
                animator.speed = 0;
                isAlreadyPaused = true;
            }
        }
        else
        {
            if (isAlreadyPaused)
            {
                isAlreadyPaused = false;
                animator.speed = previousAnimatorSpeed;
            }
        }

        if(moveSpeed >= minimuzMoveSpeed)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void SetNPCDirection(bool _left, bool _right)
    {
        if (_left)
        {
            visualsObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_right)
        {
            visualsObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void SetNPCMovementSpeed(float _speed)
    {
        moveSpeed = Mathf.Abs(_speed);
    }
}
 