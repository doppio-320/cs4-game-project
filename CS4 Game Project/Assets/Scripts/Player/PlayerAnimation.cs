using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public float minimumMoveSpeed = 0.075f;
    
    [System.Serializable]
    public struct MiscAnimation
    {
        public string name;
        public float duration;
    }

    public MiscAnimation[] registeredMiscAnimations;

    private float playerSpeed;
    private Animator animator;
    private GameObject visualsObject;
    private PlayerController playerController;

    private float previousAnimatorSpeed;
    private bool isAlreadyPaused;

    private void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;
        animator = visualsObject.GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

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

    public void ForcePlayMiscAnimation(string _anim)
    {        
        for (int i = 0; i < registeredMiscAnimations.Length; i++)
        {
            if(registeredMiscAnimations[i].name == _anim)
            {
                playerController.SetDoingMiscAnimation(registeredMiscAnimations[i].duration);
                animator.SetTrigger("miscAnim_" + _anim);
                animator.SetBool("IsDoingMiscAnimation", true);
                Invoke("ResetFreeAnimation", registeredMiscAnimations[i].duration);
                return;
            }
        }
        Debug.LogError("Did not find a misc animation with the name: " + _anim);
    }

    private void ResetFreeAnimation()
    {
        animator.SetBool("IsDoingMiscAnimation", false);
    }

    public void SetSpeed(float _speed)
    {
        playerSpeed = Mathf.Abs(_speed);
    }
}
