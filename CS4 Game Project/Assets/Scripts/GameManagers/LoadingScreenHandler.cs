using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenHandler : MonoBehaviour
{
    public static LoadingScreenHandler instance;

    public GameObject loadingScreenCanvasObject;    

    public static LoadingScreenHandler Instance
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

    public void SetLoadingScreen(float percentage)
    {

    }
}
