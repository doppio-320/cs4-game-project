using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAmbience : MonoBehaviour
{
    [Header("Volume")]
    public float maxVolume = 1f;
    public AnimationCurve volumePerDistance;

    [Header("Volume")]
    public float distanceFullDeflection = 1f;
    public AnimationCurve panningByDistance;

    private AudioSource source;
    private Transform playerTransform;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerTransform = PlayerMain.Instance.transform;
    }
    
    private void Update()
    {        
        source.volume = volumePerDistance.Evaluate(Mathf.Abs(transform.position.x - playerTransform.position.x)) * maxVolume;

        if(playerTransform.position.x < transform.position.x)
        {
            source.panStereo = panningByDistance.Evaluate(Mathf.Abs(transform.position.x - playerTransform.position.x)) * distanceFullDeflection;
        }
        else if(playerTransform.position.x > transform.position.x)
        {
            source.panStereo = -panningByDistance.Evaluate(Mathf.Abs(transform.position.x - playerTransform.position.x)) * distanceFullDeflection;
        }
    }
}
