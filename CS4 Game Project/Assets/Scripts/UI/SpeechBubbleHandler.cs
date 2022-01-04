using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleHandler : MonoBehaviour
{
    #region Instance Handling

    private static SpeechBubbleHandler instance;

    public static SpeechBubbleHandler Instance
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
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    public GameObject speechBubbleSample;
    private Dictionary<Transform, GameObject> speechBubbles;
    private Transform speechBubbleParent;
    
    void OnEnable()
    {
        speechBubbleParent = transform.Find("SpeechBubbles");
        speechBubbles = new Dictionary<Transform, GameObject>();
    }
    
    void Update()
    {
        foreach (var sb in speechBubbles)
        {
            sb.Value.transform.position = Camera.main.WorldToScreenPoint(sb.Key.position);
        }
    }

    public void AddSpeechBubble(Transform _tr, string _text)
    {
        if (!speechBubbles.ContainsKey(_tr))
        {
            GameObject obj = Instantiate(speechBubbleSample, speechBubbleParent);
            obj.transform.Find("Text").GetComponent<Text>().text = _text;
            speechBubbles.Add(_tr, obj);
            LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
        }
        else
        {
            speechBubbles[_tr].transform.Find("Text").GetComponent<Text>().text = _text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(speechBubbles[_tr].GetComponent<RectTransform>());
        }
        
    }

    public void DeleteSpeechBubble(Transform _tr)
    {
        if (speechBubbles.ContainsKey(_tr))
        {
            Destroy(speechBubbles[_tr]);
            speechBubbles.Remove(_tr);            
        }
    }
}
