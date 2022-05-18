using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatToolsDisplay : MonoBehaviour
{
    #region Instance Handling
    private static PlayerCombatToolsDisplay instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public static PlayerCombatToolsDisplay Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    [Header("Dash Visuals")]
    public GameObject darkenCover;
    public Text visualText;

    public void SetDashAvailable(bool _val)
    {
        darkenCover.SetActive(!_val);
    }

    public void SetDashRemainingDashes(int _amt)
    {
        visualText.text = _amt.ToString() + "x";
    }

    public void SetDashCooldown(float _time)
    {
        visualText.text = _time.ToString("F1");
    }
}
