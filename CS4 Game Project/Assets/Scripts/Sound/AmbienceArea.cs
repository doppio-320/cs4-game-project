using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceArea : MonoBehaviour
{
    public bool isActive = false;

    [Header("Audio Info")]    
    public AudioClip clip;
    public float strength;
    public float fadeTime;

    [Header("Ambient Area")]
    public Vector2 lowerLeftCorner;
    public Vector2 upperRightCorner;

    private Transform player;
    private AudioSource source;
    private float interpTime;
    private float fadoutVolume;

    private void Start()
    {
        player = PlayerMain.Instance.transform;

        source = GetComponent<AudioSource>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(lowerLeftCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(lowerLeftCorner.x + transform.position.x, upperRightCorner.y + transform.position.y));
        Gizmos.DrawLine(lowerLeftCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(upperRightCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y));
        Gizmos.DrawLine(upperRightCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(lowerLeftCorner.x + transform.position.x, upperRightCorner.y + transform.position.y));
        Gizmos.DrawLine(upperRightCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(upperRightCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y));
    }

    void Update()
    {        
        if (Rect.MinMaxRect(lowerLeftCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y, upperRightCorner.x + transform.position.x, upperRightCorner.y + transform.position.y).Contains(new Vector2(player.position.x, player.position.y)) && !GameHandler.Instance.inhibitAmbientSounds)
        {            
            if (!isActive)
            {
                isActive = true;
                interpTime = 0;
                source.clip = clip;
                source.Play();
            }
            interpTime += Time.deltaTime;
            source.volume = Mathf.Lerp(0, strength, interpTime);            
            return;
        }

        if(isActive)
        {
            isActive = false;
            interpTime = 0;
            fadoutVolume = source.volume;
        }
        interpTime += Time.deltaTime;
        source.volume = Mathf.Lerp(fadoutVolume, 0, interpTime);
    }
}
