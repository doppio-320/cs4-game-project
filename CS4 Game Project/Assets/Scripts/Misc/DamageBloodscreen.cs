using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DamageBloodscreen : MonoBehaviour
{
    #region Instance Handling

    private static DamageBloodscreen instance;

    public static DamageBloodscreen Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;            
        }
    }

    #endregion

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowBloodScreenLinear()
    {
        animator.SetTrigger("damage");
    }
}
