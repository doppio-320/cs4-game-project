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
    public bool inhibitAmbientSounds;
    public Transform npcContainer;

    void Start()
    {
        
    }   

    void Update()
    {
        
    }

    public void SetInhibitAmbientSounds(bool _value)
    {
        inhibitAmbientSounds = _value;
    }
}

public enum PauseState
{
    None,
    Dialogue,
    PauseMenu
}
