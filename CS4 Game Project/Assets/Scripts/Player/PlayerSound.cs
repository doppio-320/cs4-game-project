using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Footsteps")]
    public float fadeSpeed = 5f;
    public AudioSource footstepsSource;
    public AudioClip floorFootstepsSound;
    public AudioClip concreteFootstepsSound;
    private float walkingSpeed;
    private AudioClip selectedFootstepSound;

    [Header("Misc Sounds")]
    public AudioSource miscSource;
    private AudioClip activeMiscSound;
    

    void Start()
    {
        footstepsSource.clip = floorFootstepsSound;
        footstepsSource.loop = true;
        footstepsSource.Play();
    }

    
    void Update()
    {
        if(footstepsSource != null)
        {
            if (Mathf.Abs(walkingSpeed) > 0.075f && GameHandler.Instance.pauseState == PauseState.None && PlayerMain.Instance.isActive)
            {
                footstepsSource.volume = Mathf.Lerp(footstepsSource.volume, 1f, fadeSpeed * Time.deltaTime);
            }
            else
            {
                footstepsSource.volume = Mathf.Lerp(footstepsSource.volume, 0f, fadeSpeed * Time.deltaTime);
            }
        }
    }

    public void PlayMiscSound(AudioClip _clip)
    {
        activeMiscSound = _clip;
        miscSource.clip = activeMiscSound;
        miscSource.Play();
    }    

    public void SetPlayerMovementSpeed(float _speed)
    {
        walkingSpeed = _speed; 
    }

    public void UpdateFootstepSurface(string _name)
    {
        switch (_name)
        {
            default:
                selectedFootstepSound = floorFootstepsSound;
                break;
        }
    }
}
