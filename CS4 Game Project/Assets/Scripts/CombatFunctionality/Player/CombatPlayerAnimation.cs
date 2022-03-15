using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerAnimation : MonoBehaviour
{
    [SerializeField] float minimumMoveSpeed = 0.1f;
    private float playerSpeed;

    private Animator animator;
    private GameObject visualsObject;

    private void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;

        animator = visualsObject.GetComponent<Animator>();
    }

    private void Update()
    {        
        if(playerSpeed >= minimumMoveSpeed)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void CallDash()
    {
        animator.SetTrigger("Dash");
    }

    public void CallChargeJump()
    {
        animator.SetTrigger("ChargeJump");
    }

    public void CallJump()
    {
        animator.SetTrigger("Jump");
    }

    public void SetPlayerDirection(bool _left, bool _right)
    {
        if(_left)
        {
            visualsObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_right)
        {
            visualsObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void SetPlayerSpeed(float _speed)
    {
        playerSpeed = Mathf.Abs(_speed);
    }
}
