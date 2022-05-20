using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1BossfightOutcomeHandler : MonoBehaviour
{
    public float panTime;
    public float panAmount;
    public GameObject cutsceneCamera;
    private Camera cutsceneCameraReference;
    public GameObject youLoseScreen;
    public GameObject victoryScreen;
    public GameObject combatPlayer;    
    public GameObject momBoss;
    public DialoguePreset victoryDiag;

    private void OnEnable()
    {
        combatPlayer.GetComponent<CombatPlayerHealth>().OnCombatPlayerDie += StartLoseSequence;
        momBoss.GetComponent<BossBasicCombat>().OnBossDied += StartVictorySequence;        
        cutsceneCameraReference = cutsceneCamera.GetComponent<Camera>();
    }

    private void Start()
    {
        DialogueHandler.Instance.OnDialogueEnded += StartVictoryFanfare;
    }

    private void StartLoseSequence()
    {
        StartCoroutine(LostSequence1());

        GameHandler.Instance.SetCutsceneState(true);
    }

    IEnumerator LostSequence1()
    {
        yield return new WaitForSeconds(1.5f);
        PauseMenuHandler.Instance.SetMenuActive(false);
        Camera.current.transform.parent.gameObject.SetActive(false);
        youLoseScreen.SetActive(true);
        cutsceneCamera.SetActive(true);
        Level1BossfightMusic.Instance.StopMusic();

        StartCoroutine(LostSequence2());        
    }

    IEnumerator LostSequence2()
    {        
        var elapsed = panTime;
        var zOffset = cutsceneCamera.transform.position.z;
        Level1BossfightMusic.Instance.PlayDefeatMusic();
        while (elapsed > 0f)
        {
            elapsed -= Time.deltaTime;
            cutsceneCameraReference.orthographicSize += panAmount;
            cutsceneCamera.transform.position = combatPlayer.transform.position + new Vector3(0f, 0f, zOffset);

            yield return null;            
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LoadingScreen.Instance.StartLoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void StartVictorySequence()
    {
        StartCoroutine(VictorySequence1());
    }

    private void StartVictoryFanfare(DialoguePreset _pres, int _step)
    {
        if(_pres == victoryDiag)
        {
            StartCoroutine(VictorySequence2());
        }
    }

    IEnumerator VictorySequence1()
    {
        yield return new WaitForSeconds(2f);
        PauseMenuHandler.Instance.SetMenuActive(false);
        Level1BossfightMusic.Instance.StopMusic();
        DialogueHandler.Instance.StartDialogue(victoryDiag);
    }

    IEnumerator VictorySequence2()
    {
        victoryScreen.SetActive(true);

        var elapsed = 7.5f;
        var zOffset = cutsceneCamera.transform.position.z;
        Level1BossfightMusic.Instance.PlayVictoryMusic();
        while (elapsed > 0f)
        {
            elapsed -= Time.deltaTime;
            cutsceneCameraReference.orthographicSize -= panAmount;
            cutsceneCamera.transform.position = combatPlayer.transform.position + new Vector3(0f, 0f, zOffset);

            yield return null;
        }
        
        LoadingScreen.Instance.StartLoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
