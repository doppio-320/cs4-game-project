using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NormalPlayerController : MonoBehaviour
{
    public float jumpForce;
    public float movementSpeed;
    public float movementSmoothing;
       
    public bool isGrounded;
    public LayerMask groundLayer;
    private Rigidbody2D rBody;
    private Vector2 movementVelocity;

    public Vector2 MovementVelocity
    {
        get
        {
            return movementVelocity;
        }
    }

    public Transform playerVisual;
    public bool pointedRight;

    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.Instance.isInDialogue || GameHandler.Instance.isPaused)
            return;

        ProcessMovement();
        GroundCheck();
        FlipVisuals();

        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    private void GroundCheck()
    {
        isGrounded = false;
        if(Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer))
        {
            isGrounded = true;
        }
    }

    private void FlipVisuals()
    {
        if(pointedRight)
        {
            playerVisual.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            playerVisual.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ProcessMovement()
    {
        if (!isGrounded)
            return;

        Vector2 targetVelocity = new Vector2(movementSpeed * GetLateralMovement(), rBody.velocity.y);
        rBody.velocity = Vector2.SmoothDamp(rBody.velocity, targetVelocity, ref movementVelocity, movementSmoothing);
    }

    private float GetLateralMovement()
    {
        if(Input.GetKey(leftKey))
        {
            pointedRight = false;
            return -1;
        }
        else if(Input.GetKey(rightKey))
        {
            pointedRight = true;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
        }
    }
}
