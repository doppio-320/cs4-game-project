using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDiagEnded : MonoBehaviour
{
    private void Start()
    {        
        DialogueManager.Instance.DialogueEnded += (obj, args) =>
        {
            Debug.Log("Test ended diag " + args.ID);
        };
    }
}
