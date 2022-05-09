using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    #region Instance Handling
    private static LoadingScreen instance;

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

    public static LoadingScreen Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    private AsyncOperation currentLoadSceneAo;

    private GameObject loadScreenParent;
    private RectTransform loadFill;
    private float fullLoadWidth;

    private void Start()
    {
        loadScreenParent = transform.Find("LoadingScreen").gameObject;
        loadFill = loadScreenParent.transform.Find("LoadingBorder").Find("LoadingBg").Find("LoadFill").GetComponent<RectTransform>();
        fullLoadWidth = loadFill.rect.width;

        //Debug.Log(fullLoadWidth);        
    }

    private void Update()
    {
        if (!loadScreenParent.activeInHierarchy)
            return;

        loadFill.sizeDelta = new Vector2(currentLoadSceneAo.progress * fullLoadWidth, loadFill.sizeDelta.y);
    }    

    public void StartLoadSceneAsync(string _sceneName)
    {
        loadScreenParent.SetActive(true);        

        currentLoadSceneAo = SceneManager.LoadSceneAsync(_sceneName);
        currentLoadSceneAo.completed += OnSceneFullyLoaded;
    }

    private void OnSceneFullyLoaded(AsyncOperation obj)
    {
        loadScreenParent.SetActive(false);
    }
}
