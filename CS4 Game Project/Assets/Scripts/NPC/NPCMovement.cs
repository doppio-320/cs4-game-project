using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public float stoppingDistance = 0.5f;

    [Header("Basic Movement")]
    public float movementSpeed = 0.5f;
    public float movementSmoothing = 0.1f;

    [Header("Ground Check")]
    public bool isGrounded = false;
    public float groundCheckDistance = 0.05f;
    public float playerWidth = 0.5f;
    public LayerMask groundLayer;

    private bool isDoingCustomAnimation = false;

    private Rigidbody2D rBody;
    private Vector3 movementVelocity;

    private NPCAnimation npcAnimation;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();

        npcAnimation = GetComponent<NPCAnimation>();
    }

    private void Update()
    {
        if (GameHandler.Instance.pauseState == PauseState.PauseMenu)
            return;

        GroundCheck();

        CalculateMovement();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void GroundCheck()
    {
        isGrounded = false;

        if (Physics2D.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer) ||
            Physics2D.Raycast(transform.position - new Vector3(playerWidth / 2f, 0f, 0f), Vector3.down, groundCheckDistance, groundLayer) ||
            Physics2D.Raycast(transform.position + new Vector3(playerWidth / 2f, 0f, 0f), Vector3.down, groundCheckDistance, groundLayer))
        {
            isGrounded = true;
        }
    }

    /*private void OnDrawGizmosSelected()
    {
        if (target)
        {
            var midpoint = (new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z) + new Vector3(target.position.x, target.position.y + 2f, transform.position.z)) / 2f;
            UnityEditor.Handles.Label(midpoint, $"DIST: {Mathf.Abs(transform.position.x - target.transform.position.x)} / STOP: {stoppingDistance}");
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), new Vector3(target.position.x, target.position.y + 1f, transform.position.z));
        }
    }*/

    private void CalculateMovement()
    {
        Vector3 targetVelocity = new Vector3(GetLateralMovementDirection() * movementSpeed, rBody.velocity.y);
        rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref movementVelocity, movementSmoothing);

        npcAnimation.SetNPCMovementSpeed(rBody.velocity.x);
    }

    public void SetTarget(Transform _transform)
    {
        target = _transform;
    }

    public bool TargetIsWithinStoppingDistance()
    {
        return Mathf.Abs(transform.position.x - target.position.x) < stoppingDistance;
    }

    private float GetLateralMovementDirection()
    {
        if (!target)
            return 0f;

        if (isDoingCustomAnimation || TargetIsWithinStoppingDistance())
            return 0f;        

        if (target.position.x < transform.position.x)
        {
            npcAnimation.SetNPCDirection(true, false);
            return -1f;
        }
        else if (target.position.x > transform.position.x)
        {
            npcAnimation.SetNPCDirection(false, true);
            return 1f;            
        }

        return 0f;
    }
}
