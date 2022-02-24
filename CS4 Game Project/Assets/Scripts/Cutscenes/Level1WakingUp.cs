using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1WakingUp : MonoBehaviour
{
    public Sprite normalBedSprite;
    public GameObject bedObject;
    public Transform cameraTarget;
    public float initialWaitTime = 0f;
    public AudioClip glassBreakingSound;
    public GameObject wakeupGuide;
    public GameObject moveGuide;
    public GameObject destroyAlarmGuide;
    public DialoguePreset alarmClockDiag;
    public Transform alarmClockSpeechBubble;
    public AudioSource alarmClockSource;
    public float minimumMoveTimeForTutorial = 0.35f;
    public InteractibleDoor unlockDoor;

    public GameObject animatedBed;
    public GameObject staticBed;

    private PlayerController playerController;

    private bool canWakeUp = false;
    private bool wokenUp = false;

    private void DiagStarted(DialoguePreset _dialogue, int _progress)
    {
        if(_dialogue.dialogueID == alarmClockDiag.dialogueID)
        {
            unlockDoor.doorIsLocked = false;
            destroyAlarmGuide.SetActive(false);
            moveGuide.SetActive(false);
            SpeechBubbleHandler.Instance.DeleteSpeechBubble(alarmClockSpeechBubble);
            GameHandler.Instance.SetCutsceneState(false);
            Destroy(gameObject);            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogueHandler.Instance.OnDialogueStarted += DiagStarted;        

        playerController = PlayerMain.Instance.GetComponent<PlayerController>();

        StartCutscene();
        GameHandler.Instance.SetCutsceneState(true);
        GameHandler.Instance.inhibitAmbientSounds = true;
    }

    void StartCutscene()
    {
        Camera.main.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, Camera.main.transform.position.z);
        StartCoroutine(MoveCamFOV(0.4f, 1.85f, initialWaitTime));
        Invoke("BreakMusic", initialWaitTime);
    }

    void BreakMusic()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = glassBreakingSound;
        GetComponent<AudioSource>().Play();

        wakeupGuide.SetActive(true);

        alarmClockSource.Play();
        SpeechBubbleHandler.Instance.AddSpeechBubble(alarmClockSpeechBubble, "Wakey, wakey! It's time for school!");
        GameHandler.Instance.inhibitAmbientSounds = false;

        canWakeUp = true;
    }

    private void Update()
    {
        if (canWakeUp)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {                
                PlayerMain.Instance.isActive = true;
                wakeupGuide.SetActive(false);
                moveGuide.SetActive(true);                
                canWakeUp = false;
                wokenUp = true;
                Destroy(GetComponent<AudioSource>());

                animatedBed.SetActive(false);
                staticBed.SetActive(true);
            }            
        }

        if (wokenUp)
        {
            if(Input.GetKey(playerController.moveLeftKey) || Input.GetKey(playerController.moveRightKey) && minimumMoveTimeForTutorial > 0f)
            {
                minimumMoveTimeForTutorial -= Time.deltaTime;
            }
            if(minimumMoveTimeForTutorial <= 0f)
            {
                if (moveGuide.activeSelf)
                {
                    destroyAlarmGuide.SetActive(true);
                }                    
                moveGuide.SetActive(false);                
            }
        }
    }

    IEnumerator MoveCamFOV(float from, float to, float duration)
    {
        var timePassed = 0f;
        while (timePassed < duration)
        {            
            var factor = timePassed / duration;            
            
            Camera.main.orthographicSize = Mathf.Lerp(from, to, factor);

            timePassed += Mathf.Min(Time.deltaTime, duration - timePassed);

            yield return null;
        }
        
        Camera.main.orthographicSize = to;
    }
}
