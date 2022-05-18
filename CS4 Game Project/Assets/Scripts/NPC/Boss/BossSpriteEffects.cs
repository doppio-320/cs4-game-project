using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpriteEffects : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = transform.Find("Visuals").Find("Effects").GetComponent<Animator>();
        animator.gameObject.SetActive(false);
    }

    public void StartAnimation(int _idx)
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger("boss_atk" + _idx.ToString());
    }

    public void EndAnimation()
    {
        animator.gameObject.SetActive(false);
    }
}
