using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatHUD : MonoBehaviour
{
    #region Instance Handling
    private static PlayerCombatHUD instance;

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

    public static PlayerCombatHUD Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private Transform primaryHudParent;

    [Header("Player Health Display")]
    [SerializeField] private Transform playerHealthParent;
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private float initialWidth;
    [SerializeField] private Text healthDisplay;

    void OnEnable()
    {
        primaryHudParent = transform.Find("PrimaryHUD");

        playerHealthParent = primaryHudParent.Find("PlayerHealth");
        healthFill = playerHealthParent.Find("HP_Fill").GetComponent<RectTransform>();
        initialWidth = healthFill.rect.width;
        healthDisplay = playerHealthParent.Find("HP_Text").GetComponent<Text>();
    }

    public void SetEnabled(bool _value)
    {
        primaryHudParent.gameObject.SetActive(_value);
    }

    public void SetPlayerHealth(float _hp, float _max)
    {
        healthDisplay.text = ((int)_hp).ToString();        

        healthFill.sizeDelta = new Vector2(initialWidth * (_hp / _max), healthFill.sizeDelta.y);
    }
}
