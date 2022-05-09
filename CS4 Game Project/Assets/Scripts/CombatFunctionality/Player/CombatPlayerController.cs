using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerController : MonoBehaviour
{
    [Header("Movement Input")]
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W | KeyCode.Space;
    public KeyCode dashKey = KeyCode.E;    
    public bool isDashing = false;
    public bool isDashingRight = true;
    public bool attackingInhibit = false;

    [Header("Movement Data")]        
    public float movementSpeed = 1.5f;
    public float movementSmoothing = 0.025f;

    [Header("Advanced Dash")]
    public float dashDistance = 3.5f;
    public float dashTime = 0.45f;
    public float dashPrewarm = 0.13f;
    /*[SerializeField]*/ private float currentPrewarm = 0f;

    [Header("Advanced Jump")]
    public float jumpHeight = 3f;
    public float chargeJumpTime = 0.2f;
    public AnimationCurve jumpChargeGraph;
    /*[SerializeField]*/ private float currentJumpCharge = 0f;
    /*[SerializeField]*/ private bool chargingJump = false;
    /*[SerializeField]*/ private bool jumpReleased = false;

    [Header("Ground Check")]
    public bool isGrounded = false;
    public bool lungeInhibit = false;
    public float groundCheckDistance = 0.05f;
    public float playerWidth = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rBody;
    private Vector3 movementVelocity;

    private bool forceSavePosition = false;
    private Vector2 savedPosition;

    private CombatPlayerAnimation combatPlayerAnim;
    private CombatPlayerFighting combatPlayerFight;
    private CombatPlayerHealth combatPlayerHealth;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();

        combatPlayerAnim = GetComponent<CombatPlayerAnimation>();
        combatPlayerFight = GetComponent<CombatPlayerFighting>();
        combatPlayerHealth = GetComponent<CombatPlayerHealth>();

        currentJumpCharge = chargeJumpTime;
    }


    void Update()
    {        
        if (GameHandler.Instance.pauseState != PauseState.None && GameHandler.Instance.pauseState != PauseState.Cutscene)
        {
            if(!forceSavePosition)
            {
                savedPosition = transform.position;
                forceSavePosition = true;
            }
            transform.position = savedPosition;
            rBody.velocity = Vector2.zero;
            return;
        }
        else
        {
            forceSavePosition = false;
        }

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

    public void SetAttackingInhibit(bool value)
    {
        attackingInhibit = value;
        if(attackingInhibit)
        {
            rBody.velocity = new Vector2(rBody.velocity.x / 3, 0f);
        }
    }

    private float GetLateralMovementDirection()
    {
        if (attackingInhibit)
            return 0f;

        if (Input.GetKey(moveLeftKey))
        {
            combatPlayerAnim.SetPlayerDirection(true, false);
            isDashingRight = false;
            return -1f;
        }
        else if (Input.GetKey(moveRightKey))
        {
            combatPlayerAnim.SetPlayerDirection(false, true);
            isDashingRight = true;
            return 1f;
        }

        return 0f;
    }

    public bool GetDashing()
    {
        return isDashing;
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }

    public void AttackLunge(float time, float force)
    {
        lungeInhibit = true;
        Invoke("EndLunge", time);
        rBody.AddForce(new Vector2(force * (isDashingRight ? 1 : -1), 0.05f), ForceMode2D.Impulse);        
    }

    private void EndLunge()
    {
        lungeInhibit = false;
        rBody.velocity = new Vector2(0f, rBody.velocity.y);
    }

    private void Jump()
    {
        jumpReleased = true;
        chargingJump = false;

        var chargeFactor = jumpChargeGraph.Evaluate((chargeJumpTime - currentJumpCharge) / chargeJumpTime);
        rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Sqrt(-2 * (Physics2D.gravity.y * rBody.gravityScale) * (jumpHeight * chargeFactor)));

        currentJumpCharge = chargeJumpTime;

        combatPlayerAnim.CallJump();
    }

    private void CalculateMovement()
    {
        if (combatPlayerHealth)
        {
            if (combatPlayerHealth.GetStunned())
            {
                combatPlayerFight.SetInhibitAttacks(true);
                return;
            }
        }
        combatPlayerFight.SetInhibitAttacks(false);

        if (isGrounded)
        {
            //playerSound.SetPlayerMovementSpeed(0f);
            //return;

            if (Input.GetKey(jumpKey) && !jumpReleased && !isDashing)
            {
                if(!chargingJump)
                {
                    combatPlayerAnim.CallChargeJump();
                }

                currentJumpCharge -= Time.deltaTime;
                chargingJump = true;             
                
                if(currentJumpCharge <= 0f)
                {
                    Jump();
                }
            }
        }

        var releasedJumpKey = Input.GetKeyUp(jumpKey);

        if (releasedJumpKey)
        {
            if (chargingJump && isGrounded)
            {
                Jump();
            }
            jumpReleased = false;
        }

        if (Input.GetKeyDown(dashKey) && !isDashing)
        {            
            currentPrewarm = dashPrewarm;
            isDashing = true;
            combatPlayerAnim.CallDash();
            combatPlayerFight.SetInhibitAttacks(true);
            Invoke("EndDash", dashTime);
        }
        
        if (isDashing)
        {
            var speed = dashDistance / dashTime;
            if(currentPrewarm > 0f)
            {
                speed = movementSpeed;
                currentPrewarm -= Time.deltaTime;
            }
            Vector3 targetVelocity = new Vector3(isDashingRight ? 1f * speed : -1f * speed, 0);
            rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref movementVelocity, movementSmoothing);
            rBody.velocity = new Vector2(rBody.velocity.x, 0);
        }
        else
        {
            if(!lungeInhibit)
            {
                Vector3 targetVelocity = new Vector3(GetLateralMovementDirection() * movementSpeed, rBody.velocity.y);
                rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref movementVelocity, movementSmoothing);
            }
        }
        combatPlayerAnim.SetPlayerSpeed(rBody.velocity.x);

        //playerAnimation.SetPlayerMovementSpeed(rBody.velocity.x);
        //playerSound.SetPlayerMovementSpeed(rBody.velocity.x);
    }

    private void EndDash()
    {        
        isDashing = false;
        combatPlayerFight.SetInhibitAttacks(false);
    }
}
