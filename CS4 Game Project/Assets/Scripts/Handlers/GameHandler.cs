using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    #region Instance Handling

    private static GameHandler instance;

    public static GameHandler Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public PauseState pauseState;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}

public enum PauseState
{
    None,
    Dialogue,
    PauseMenu
}
