using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteEffects : MonoBehaviour
{
    private Animator animator;
    private BossBasicCombat bbc;

    private void Start()
    {
        animator = transform.Find("Visuals").Find("Effects").GetComponent<Animator>();
        animator.gameObject.SetActive(false);

        bbc = GetComponent<BossBasicCombat>();
    }

    private void OnEnable()
    {
        bbc = GetComponent<BossBasicCombat>();
        bbc.OnBossStartedAttack += StartAnimation;
        bbc.OnBossEndedAttack += EndAnimation;
    }

    public void StartAnimation(int _idx)
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger("boss_atk" + _idx.ToString());
    }

    public void EndAnimation(int _idx)
    {
        animator.gameObject.SetActive(false);
    }
}
