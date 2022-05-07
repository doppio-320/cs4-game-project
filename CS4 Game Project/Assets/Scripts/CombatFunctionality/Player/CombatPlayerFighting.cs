using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerFighting : MonoBehaviour
{
    [System.Serializable]
    public class PlayerAttack
    {
        public float attackTime;
        public float startHitscanTime;
        public float missHitscanTime;
        public float minimumNextAttack;
        public bool doesLunge;
        public int redirectIndex = -1;
    }

    [Header("Primary Attack")]
    public float preAttackTime;    
    public PlayerAttack[] attackSequence;
    public float postAttackTime;

    [Header("Complimentary Lunge")]
    public float tempDisableMovementTime = 0.1f;
    public float lungeForce = 20f;

    [Header("Attack vars")]
    public FightingState fightingState;
    public int attackIndex;
    public bool hitAvailable;
    public bool didLunge;
    public float preAttackRemaining;
    public float attackRemaining;
    public float postAttackRemaining;

    [Header("Visuals")]
    private GameObject visualsObject;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CombatPlayerController playerController;

    void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;
        spriteRenderer = visualsObject.GetComponent<SpriteRenderer>();
        animator = visualsObject.GetComponent<Animator>();
        playerController = GetComponent<CombatPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {            
            switch(fightingState)
            {
                case FightingState.None:
                    preAttackRemaining = preAttackTime;
                    fightingState = FightingState.PreAttack;
                    animator.SetBool("anyAttack", true);
                    animator.SetTrigger("preAttack");
                    attackIndex = 0;
                    break;

                case FightingState.Attacking:
                    if(attackRemaining < (attackSequence[attackIndex].attackTime - attackSequence[attackIndex].minimumNextAttack))
                    {                        
                        if (attackSequence[attackIndex].redirectIndex > -1)
                        {
                            attackIndex = attackSequence[attackIndex].redirectIndex;
                            UpdateAttack();
                        }
                        else if (attackIndex < attackSequence.Length - 1)
                        {
                            attackIndex++;
                            UpdateAttack();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        if(fightingState == FightingState.PreAttack)
        {
            playerController.SetAttackingInhibit(true);
            if(preAttackRemaining < 0f)
            {
                fightingState = FightingState.Attacking;
                UpdateAttack();
            }
            preAttackRemaining -= Time.deltaTime;
        }
        else if(fightingState == FightingState.Attacking)
        {            
            //Debug.Log("Atk rem: " + attackRemaining.ToString() + " vs. " + (attackSequence[attackIndex].attackTime - attackSequence[attackIndex].startHitscanTime).ToString());
            if(attackRemaining < (attackSequence[attackIndex].attackTime - attackSequence[attackIndex].startHitscanTime) &&
                attackRemaining > (attackSequence[attackIndex].attackTime - attackSequence[attackIndex].missHitscanTime))
            {
                if(hitAvailable)
                {
                    //spriteRenderer.color = Color.green;
                    //Check hitscan
                }

                if(!didLunge && attackSequence[attackIndex].doesLunge)
                {
                    didLunge = true;
                    playerController.AttackLunge(tempDisableMovementTime, lungeForce);
                }
            }
            else
            {                
                //spriteRenderer.color = Color.white;
            }

            if(attackRemaining < 0f)
            {
                fightingState = FightingState.PostAttack;
                postAttackRemaining = postAttackTime;
                hitAvailable = false;
                animator.SetTrigger("postAttack");
                playerController.SetAttackingInhibit(false);
            }
            attackRemaining -= Time.deltaTime;
        }
        else if(fightingState == FightingState.PostAttack)
        {            
            postAttackRemaining -= Time.deltaTime;
            if (postAttackRemaining < 0f)
            {
                animator.SetBool("anyAttack", false);
                fightingState = FightingState.None;                
            }
        }
    }

    private void UpdateAttack()
    {
        attackRemaining = attackSequence[attackIndex].attackTime;
        animator.SetTrigger("cb_atk" + attackIndex.ToString());
        hitAvailable = true;
        didLunge = false;
    }
}

public enum FightingState
{
    None,
    PreAttack,
    Attacking,
    PostAttack
}
