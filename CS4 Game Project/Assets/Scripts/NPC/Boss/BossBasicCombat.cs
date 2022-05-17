using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBasicCombat : MonoBehaviour
{
    [Header("Target")]
    public GameObject targetPlayer;
    public LayerMask playerLayer;

    [System.Serializable]
    public class BossAttack
    {
        [Header("Timings")]
        public float attackDuration;
        public float attackDelay;
        public float attackRange;

        [Header("Parry")]
        public float delayUntilParryAvail;

        [Header("Damage")]
        public Vector2 attackKnockback;
        public float attackDamage;
    }

    private float attackRemaining = 0f;
    private float attackCooldown;
    private bool isInAttackPhase = false;
    private bool attackingRight;

    [Header("Attack Vars")]
    public int attackIndex;
    public BossAttack[] attackSequence;
    public float lastAttackCooldown;
    private float attackHeightOffset;

    [Header("Health")]
    public float defaultHeath;
    [SerializeField] private float currentHealth;

    [Header("Stun")]
    public bool isStunned;
    public float sustainStunDistance;
    public float sustainStunDuration;
    private float stunDisLeft;
    private float stunTimeLeft;
    private Vector2 previousPos;

    [Header("Parry")]
    public Vector2 knockBackOnParry;
    public bool canTakeParry;

    private NPCAnimation npcAnimation;
    private NPCMovement npcMovement;
    private Animator animator;

    private CombatPlayerHealth targetCPH;

    public bool ParryIsAvailable()
    {
        return canTakeParry;
    }

    public bool IsAttacking()
    {
        return isInAttackPhase;
    }

    private void Start()
    {
        npcAnimation = GetComponent<NPCAnimation>();
        npcMovement = GetComponent<NPCMovement>();
        animator = transform.Find("Visuals").GetComponent<Animator>();        
        attackHeightOffset = GetComponent<BoxCollider2D>().bounds.center.y;        

        currentHealth = defaultHeath;
        
        UpdateTarget(targetPlayer);

        EnemyHealthDisplay.Instance.SetHealth(GetComponent<NPCMain>().npcName, currentHealth, defaultHeath);
    }

    public void UpdateTarget(GameObject _target)
    {
        if (_target.GetComponent<CombatPlayerHealth>())
        {
            targetPlayer = _target;
            targetCPH = targetPlayer.GetComponent<CombatPlayerHealth>();
        }
    }

    private void Update()
    {
        if (GameHandler.Instance.pauseState != PauseState.None && GameHandler.Instance.pauseState != PauseState.Cutscene)
            return;

        if (IsDead())
            return;

        if (isStunned)
        {
            if (npcMovement.IsGrounded() && (stunDisLeft <= 0f || stunTimeLeft <= 0f))
            {
                UnStun();
            }

            stunDisLeft -= Vector2.Distance(new Vector2(transform.position.x, transform.position.y), previousPos);
            stunTimeLeft -= Time.deltaTime;

        }
        previousPos = transform.position;

        if (!isInAttackPhase)
        {
            if(attackCooldown <= 0f)
            {
                if (GetDistanceFromTarget() < attackSequence[0].attackRange && !targetCPH.IsDead())
                {
                    StartAttacking(attackIndex);
                }
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }          
        }
        else
        {
            if(attackRemaining <= 0)
            {
                if(attackIndex < attackSequence.Length - 1)
                {                    
                    attackIndex++;
                    if (GetDistanceFromTarget() < attackSequence[attackIndex].attackRange && !targetCPH.IsDead())
                    {
                        StartAttacking(attackIndex);
                    }
                    else
                    {
                        EndAttacking();                        
                    }
                }                
                else
                {
                    EndAttacking();                    
                }
            }
            else
            {
                attackRemaining -= Time.deltaTime;
            }
        }
    }

    public float GetDistanceFromTarget()
    {
        return Vector2.Distance(transform.position + new Vector3(0f, attackHeightOffset, 0f), targetPlayer.transform.position + new Vector3(0f, attackHeightOffset, 0f));        
    }

    private void StartAttacking(int _idx)
    {        
        if(targetPlayer.transform.position.x > transform.position.x)
        {
            attackingRight = true;
            npcAnimation.SetNPCDirection(false, true);
        }
        else
        {
            attackingRight = false;
            npcAnimation.SetNPCDirection(true, false);
        }

        isInAttackPhase = true;
        canTakeParry = false;
        SetNPCScriptsStatus(false);
        attackRemaining = attackSequence[_idx].attackDuration;
        animator.SetBool("AttackActive", true);
        animator.SetTrigger("boss_atk" + _idx);

        Invoke("StartHitscan", attackSequence[_idx].attackDelay);
        Invoke("StartAvailParry", attackSequence[_idx].delayUntilParryAvail);
    }

    public void Parry()
    {
        EndAttacking();
        SetStunned();
        ApplyParryKnockback();
    }

    private void ApplyParryKnockback()
    {
        var finalKB = knockBackOnParry;
        if(animator.transform.localScale.x > 0)
        {
            finalKB.x = -finalKB.x;
        }
        GetComponent<Rigidbody2D>().velocity = finalKB;
    }

    private void SetStunned()
    {
        isStunned = true;        
        npcMovement.enabled = false;

        stunDisLeft = sustainStunDistance;
        stunTimeLeft = sustainStunDuration;
    }

    private void UnStun()
    {
        isStunned = false;        
        npcMovement.enabled = true;
    }

    private void StartHitscan()
    {
        if (IsDead() || !isInAttackPhase)
            return;

        var ray = Physics2D.Raycast(GetComponent<BoxCollider2D>().bounds.center, attackingRight ? Vector2.right : Vector2.left, attackSequence[attackIndex].attackRange, playerLayer);
        if (ray)
        {
            var cpC = ray.transform.GetComponent<CombatPlayerController>();
            if (cpC)
            {
                if (!cpC.GetDashing())
                {
                    var cpH = ray.transform.GetComponent<CombatPlayerHealth>();
                    if (cpH)
                    {
                        ApplyAttackOnPlayer(cpH);
                        canTakeParry = false;
                    }
                }                
            }            
        }
    }

    private void StartAvailParry()
    {
        if (IsDead() || !isInAttackPhase)
            return;

        canTakeParry = true;
    }

    private void ApplyAttackOnPlayer(CombatPlayerHealth _playerHealth) 
    {
        _playerHealth.Damage(attackSequence[attackIndex].attackDamage);
        if (attackSequence[attackIndex].attackKnockback.magnitude > 0)
        {
            var knockBack = attackSequence[attackIndex].attackKnockback;
            if (!attackingRight)
            {
                knockBack.x = -knockBack.x;
            }
            _playerHealth.Knockback(knockBack);
        }
    }

    private void EndAttacking()
    {        
        SetNPCScriptsStatus(true);
        animator.SetBool("AttackActive", false);
        isInAttackPhase = false;
        attackIndex = 0;
        canTakeParry = false;

        attackCooldown = lastAttackCooldown;
    }

    private void SetNPCScriptsStatus(bool _value)
    {
        npcAnimation.enabled = _value;
        npcMovement.enabled = _value;
    }

    public void SetAtkDirection(bool _isRight)
    {
        attackingRight = _isRight;
    }

    public void Damage(float _dmg)
    {
        if (IsDead())
            return;

        currentHealth -= _dmg;

        if (IsDead())
        {
            Die();
        }

        EnemyHealthDisplay.Instance.SetHealth(GetComponent<NPCMain>().npcName, currentHealth, defaultHeath);
    }

    private void Die()
    {
        EndAttacking();
        attackCooldown = 0f;
        attackRemaining = 0f;

        npcAnimation.enabled = false;
        npcMovement.enabled = false;
        animator.SetBool("isDead", true);        

        currentHealth = 0f;
    }

    public void Revive()
    {
        npcAnimation.enabled = true;
        npcMovement.enabled = true;
        animator.SetBool("isDead", false);
    }

    private bool IsDead()
    {
        if(currentHealth <= 0f)
        {
            return true;
        }
        else { return false; }
    }
}
