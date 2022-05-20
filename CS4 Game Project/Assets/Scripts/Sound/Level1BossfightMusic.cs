using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1BossfightMusic : MonoBehaviour
{
    #region Instance Handling

    private static Level1BossfightMusic instance;

    public static Level1BossfightMusic Instance
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
            Destroy(gameObject);
        }
        else
        {
            instance = this;            
        }
    }

    #endregion

    public AudioClip preCombatMusic;
    public AudioClip combatLoopMusic;
    public AudioClip defeatMusic;
    public AudioClip victoryMusic;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartCombatMusic()
    {
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        audioSource.Stop();
        audioSource.clip = preCombatMusic;
        audioSource.Play();

        yield return new WaitForSeconds(preCombatMusic.length);

        audioSource.Stop();
        audioSource.clip = combatLoopMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Pause();
    }

    public void PlayDefeatMusic()
    {
        audioSource.clip = defeatMusic;
        audioSource.Play();
        audioSource.loop = false;
    }

    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;
        audioSource.Play();
        audioSource.loop = false;
    }
}
