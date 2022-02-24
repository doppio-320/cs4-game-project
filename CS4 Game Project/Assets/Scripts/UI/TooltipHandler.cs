using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    #region Instance Handling

    private static TooltipHandler instance;

    public static TooltipHandler Instance
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
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public Vector2 tooltipOffset;

    private GameObject tooltipHandler;
    private Text nameText;
    private GameObject interactTip;
    private Text actionKeyText;
    private Text actionContextText;

    void Start()
    {
        tooltipHandler = transform.Find("Tooltips").Find("TooltipParent").gameObject;
        nameText = tooltipHandler.transform.Find("NamePanel").Find("NameText").GetComponent<Text>();
        interactTip = tooltipHandler.transform.Find("NamePanel").Find("InteractTip").gameObject;
        actionKeyText = interactTip.transform.Find("ButtonImage").Find("InteractKey").GetComponent<Text>();
        actionContextText = interactTip.transform.Find("ActionContext").GetComponent<Text>();

        EndTooltip();

        DialogueHandler.Instance.OnDialogueStarted += (dg, prog) =>
        {
            EndTooltip();
        };

        SceneManager.sceneLoaded += ClearTooltip;
    }

    private void ClearTooltip(Scene arg0, LoadSceneMode arg1)
    {
        EndTooltip();
    }

    private void OnEnable()
    {
        
    }

    void Update()
    {
        if (!tooltipHandler.activeSelf)
            return;

        tooltipHandler.transform.position = Input.mousePosition + new Vector3(tooltipOffset.x, tooltipOffset.y, 0f);
    }

    public void SetTooltip(TooltipObject _obj)
    {
        tooltipHandler.SetActive(true);
        interactTip.SetActive(false);

        nameText.text = _obj.title;

        ForceUpdateLayout();
    }

    public void SetTooltip(InteractibleObject _obj, KeyCode _key)
    {
        tooltipHandler.SetActive(true);
        interactTip.SetActive(true);

        nameText.text = _obj.title;
        actionContextText.text = _obj.action;
        actionKeyText.text = _key.ToString();

        ForceUpdateLayout();
    }

    void ForceUpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(actionKeyText.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(actionContextText.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(nameText.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipHandler.transform.GetComponent<RectTransform>());
    }

    public void EndTooltip()
    {
        tooltipHandler.SetActive(false);
        interactTip.SetActive(false);
    }
}
