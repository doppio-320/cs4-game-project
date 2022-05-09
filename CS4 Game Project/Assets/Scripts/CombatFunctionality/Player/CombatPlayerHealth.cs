using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayerHealth : MonoBehaviour
{
    [Header("General")]
    public float startingHealth = 1000f;
    public float sustainStunDistance;
    public float sustainStunTime;

    [Header("Runtime")]
    public float currentHealth;

    [Header("Stun")]
    public bool isStunned;
    public float stunDisLeft;
    public float stunTimeLeft;

    private CombatPlayerController playerController;
    private Rigidbody2D rBody;
    private Vector2 previousPos;

    public bool GetStunned()
    {
        return isStunned;
    }

    private void Start()
    {
        playerController = GetComponent<CombatPlayerController>();
        rBody = GetComponent<Rigidbody2D>();

        currentHealth = startingHealth;

        PlayerCombatHUD.Instance.SetPlayerHealth(currentHealth, startingHealth);
    }

    private void Update()
    {
        if(isStunned)
        {
            if (playerController.GetGrounded() && (stunDisLeft <= 0f || stunTimeLeft <= 0f))
            {
                isStunned = false;
            }

            stunDisLeft -= Vector2.Distance(new Vector2(transform.position.x, transform.position.y), previousPos);
            stunTimeLeft -= Time.deltaTime;

        }
        previousPos = transform.position;

        if(Input.GetKeyDown(KeyCode.R))
        {
            Revive();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
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

        PlayerCombatHUD.Instance.SetPlayerHealth(currentHealth, startingHealth);
    }

    public void Die()
    {
        GetComponent<CombatPlayerAnimation>().enabled = false;
        GetComponent<CombatPlayerController>().enabled = false;
        GetComponent<CombatPlayerFighting>().enabled = false;
        transform.Find("Visuals").GetComponent<Animator>().SetBool("isDead", true);

        currentHealth = 0;
    }

    public void Revive()
    {
        currentHealth = startingHealth;

        GetComponent<CombatPlayerAnimation>().enabled = true;
        GetComponent<CombatPlayerController>().enabled = true;
        GetComponent<CombatPlayerFighting>().enabled = true;        
        transform.Find("Visuals").GetComponent<Animator>().SetBool("isDead", false);

        transform.position = Vector3.zero;        
    }

    public void Knockback(Vector2 _dir)
    {
        SetStunned();
        rBody.velocity = _dir;
    }

    public void SetStunned()
    {
        isStunned = true;
        stunDisLeft = sustainStunDistance;
        stunTimeLeft = sustainStunTime;
    }
}
