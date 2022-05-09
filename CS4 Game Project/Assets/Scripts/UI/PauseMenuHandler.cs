using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    #region Instance Handling

    private static PauseMenuHandler instance;

    public static PauseMenuHandler Instance
    {
        get
        {
            return instance;
        }
    }

    public Button.ButtonClickedEvent OnResumeButtonClicked { get; private set; }

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

    #endregion

    private GameObject pauseMenuObject;
    private Button resumeButton;

    private void Start()
    {
        pauseMenuObject = transform.Find("PauseMenu").gameObject;

        resumeButton = pauseMenuObject.transform.Find("ResumeButton").GetComponent<Button>();
        resumeButton.onClick.AddListener(ResumeGame);
    }

    public void SetMenuActive(bool _value)
    {
        pauseMenuObject.SetActive(_value);
    }

    public void ResumeGame()
    {
        GameHandler.Instance.UnpauseMenu();
    }
}
