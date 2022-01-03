using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private GameObject visualsObject;

    // Start is called before the first frame update
    void Start()
    {
        visualsObject = transform.Find("Visuals").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNPCDirection(bool _left, bool _right)
    {
        if (_left)
        {
            visualsObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(_right)
        {
            visualsObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
 