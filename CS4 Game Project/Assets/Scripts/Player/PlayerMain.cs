using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private static PlayerMain instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public static PlayerMain Instance
    {
        get
        {
            return instance;
        }
    }

    public float playerHeight;    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
