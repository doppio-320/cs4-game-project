using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
