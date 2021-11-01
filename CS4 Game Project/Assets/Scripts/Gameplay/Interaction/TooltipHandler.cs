using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
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
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public GameObject tooltipObject;

    private Text titleText;
    private GameObject actionSection;
    private Text actionText;
    private Text actionKeyText;

    private KeyCode interactKey;

    private void Start()
    {
        titleText = tooltipObject.transform.Find("Panel").Find("title").gameObject.GetComponent<Text>();

        actionSection = tooltipObject.transform.Find("Panel").Find("InteractTip").gameObject;
        actionKeyText = actionSection.transform.Find("KeyInteract").GetChild(0).GetComponent<Text>();
        actionText = actionSection.transform.Find("ActionContext").GetComponent<Text>();

        ResetTooltip();
    }

    private void Update()
    {
        if (!tooltipObject.activeSelf)
            return;

        tooltipObject.transform.position = Input.mousePosition + new Vector3(10, -10, 0);
    }

    public void SetInteractKey(KeyCode _key)
    {
        interactKey = _key;
    }

    public void EnableTooltip(TooltipObject _tooltip)
    {
        tooltipObject.SetActive(true);
        actionSection.SetActive(false);

        titleText.text = _tooltip.title;        
    }

    public void EnableTooltip(InteractibleObject _interactibleObject)
    {
        tooltipObject.SetActive(true);
        actionSection.SetActive(true);

        titleText.text = _interactibleObject.title;

        actionKeyText.text = interactKey.ToString();
        actionText.text = _interactibleObject.actionText;
    }

    public void ResetTooltip()
    {
        tooltipObject.SetActive(false);
        actionSection.SetActive(false);
    }
}
