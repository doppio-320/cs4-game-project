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
    public bool isActive = true;
    public Transform speechBubbleLocation;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
