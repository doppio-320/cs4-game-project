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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseState == PauseState.None)
            {
                pauseState = PauseState.PauseMenu;
                PauseMenuHandler.Instance.SetMenuActive(true);
            }
            else if(pauseState == PauseState.PauseMenu)
            {
                pauseState = PauseState.None;
                PauseMenuHandler.Instance.SetMenuActive(false);
            }
        }
    }

    public void UnpauseMenu()
    {
        if (pauseState == PauseState.PauseMenu)
        {
            pauseState = PauseState.None;
            PauseMenuHandler.Instance.SetMenuActive(false);
        }
    }

    public void SetCutsceneState(bool _state)
    {
        if (_state)
        {
            pauseState = PauseState.Cutscene;
        }
        else
        {
            pauseState = PauseState.None;
        }
    }

    public void SetDialogueState(bool _state)
    {
        if (_state)
        {
            pauseState = PauseState.Dialogue;
        }
        else
        {
            pauseState = PauseState.None;
        }
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
    Cutscene,
    PauseMenu
}
