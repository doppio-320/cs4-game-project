using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    [Header("Basic Movement")]
    public float movementSpeed = 0.5f;
    public float movementSmoothing = 0.1f;

    [Header("Ground Check")]
    public bool isGrounded = false;
    public float groundCheckDistance = 0.05f;
    public float playerWidth = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rBody;
    private Vector3 movementVelocity;

    private PlayerAnimation playerAnimation;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        if (GameHandler.Instance.pauseState != PauseState.None)
            return;

        GroundCheck();

        CalculateMovement();
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

    private void CalculateMovement()
    {
        if (!isGrounded)
            return;

        Vector3 targetVelocity = new Vector3(GetLateralMovementDirection() * movementSpeed, rBody.velocity.y);
        rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref movementVelocity, movementSmoothing);

        playerAnimation.SetSpeed(rBody.velocity.x);
    }

    private float GetLateralMovementDirection()
    {
        if (Input.GetKey(moveLeftKey))
        {
            playerAnimation.SetDirection(true, false);
            return -1f;            
        }
        else if (Input.GetKey(moveRightKey))
        {
            playerAnimation.SetDirection(false, true);
            return 1f;
        }

        return 0f;
    }
}
