using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSelector : MonoBehaviour
{
    public AudioClip[] clips;
    public float maxTime;
    public float minTime;

    private AudioSource source;

    void OnEnable()
    {
        source = GetComponent<AudioSource>();

        Play();
    }

    void Play()
    {
        if (!gameObject.activeInHierarchy)
            return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
        Invoke("Play", Random.Range(minTime, maxTime));
    }
}
