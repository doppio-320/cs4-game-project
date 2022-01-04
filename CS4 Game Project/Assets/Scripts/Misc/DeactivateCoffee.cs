using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCoffee : MonoBehaviour
{
    private DestroyOnDialogueEvent destroyOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        destroyOn = GetComponent<DestroyOnDialogueEvent>();

        destroyOn.OnReEnabled += ExecuteDeactivateCoffee;
    }

    // Update is called once per frame
    void OnDisable()
    {
        destroyOn.OnReEnabled -= ExecuteDeactivateCoffee;
    }

    void ExecuteDeactivateCoffee()
    {
        Destroy(GetComponent<InteractibleObject>());
        var tto = gameObject.AddComponent<TooltipObject>();
        tto.title = "Coffee Maker";
        Level1Cutscene.Instance.SpawnMom();
    }
}
