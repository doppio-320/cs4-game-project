using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Cutscene : MonoBehaviour
{
    #region Instance Handling
    private static Level1Cutscene instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public static Level1Cutscene Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public Transform cameraTarget;
    public bool isActive = false;
    public bool hasDrankCoffee = false;
    private bool reminderIsShown;

    [Header("Visual Assets")]
    public GameObject shockFX;
    public GameObject npcMom;
    public Transform momSpawnLocation;

    [Header("Audio Assets")]
    public AudioClip vineBoomShock;
    public AudioClip speechBubbleSound;
    public AudioClip intenseMusic;

    [Header("Trigger Area")]
    public Vector2 lowerLeftCorner;
    public Vector2 upperRightCorner;

    [Header("Speeches")]
    public string npcMomSpeech1 = "Do you know how late you are to class?";
    public string npcMomSpeech2 = "Give me your hand!";

    private bool waitForDistanceToClose = false;
    private bool isDoingTransition = false;

    private PlayerMain playerMain;
    private PlayerController playerController;
    private PlayerSound playerSound;
    private CameraFollowPlayer playerCam;    

    private AudioSource oneShot;

    public GameObject momNPC;
    private NPCMovement momNpcMovement;
    private Transform momSpeech;

    // Start is called before the first frame update
    void Start()
    {
        playerMain = PlayerMain.Instance;
        playerController = playerMain.GetComponent<PlayerController>();
        playerCam = playerMain.GetComponent<CameraFollowPlayer>();
        playerSound = playerMain.GetComponent<PlayerSound>();

        oneShot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Rect.MinMaxRect(lowerLeftCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y, upperRightCorner.x + transform.position.x, upperRightCorner.y + transform.position.y).Contains(new Vector2(playerMain.transform.position.x, playerMain.transform.position.y)))
        {
            if (!isActive && hasDrankCoffee)
            {
                isActive = true;
                Step1Cutscene();
            }
            if (!hasDrankCoffee)
            {
                if (!reminderIsShown)
                {
                    reminderIsShown = true;
                    SpeechBubbleHandler.Instance.AddSpeechBubble(playerMain.speechBubbleLocation, "I should get some coffee in the kitchen before going to school.");
                }
            }
        }
        else
        {
            if (!hasDrankCoffee)
            {
                if (reminderIsShown)
                {
                    reminderIsShown = false;
                    SpeechBubbleHandler.Instance.DeleteSpeechBubble(playerMain.speechBubbleLocation);
                }
            }
        }

        if (waitForDistanceToClose)
        {
            if (momNpcMovement.TargetIsWithinStoppingDistance() && !isDoingTransition)
            {
                LoadingScreen.Instance.StartLoadSceneAsync("Level1Bossfight");
            }
        }
    }

    void Step1Cutscene()
    {
        GameHandler.Instance.SetCutsceneState(true);
        playerSound.PlayMiscSound(intenseMusic, 0.1f);
        playerController.SetInhibitMoving(true);
        var shockObject = Instantiate(shockFX, playerMain.transform.Find("Visuals"));
        Destroy(shockObject, 0.45f);

        Invoke("Step2Cutscene", 1f);
    }

    void Step2Cutscene()
    {
        playerCam.SetTarget(cameraTarget);
        Invoke("PlayShockSfx", 0.65f);
        Invoke("FlipMom", 2.5f);
    }

    void PlayShockSfx()
    {
        oneShot.PlayOneShot(vineBoomShock);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(lowerLeftCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(lowerLeftCorner.x + transform.position.x, upperRightCorner.y + transform.position.y));
        Gizmos.DrawLine(lowerLeftCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(upperRightCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y));
        Gizmos.DrawLine(upperRightCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(lowerLeftCorner.x + transform.position.x, upperRightCorner.y + transform.position.y));
        Gizmos.DrawLine(upperRightCorner + new Vector2(transform.position.x, transform.position.y), new Vector3(upperRightCorner.x + transform.position.x, lowerLeftCorner.y + transform.position.y));
    }

    public void SpawnMom()
    {
        hasDrankCoffee = true;
        momNPC = Instantiate(npcMom, momSpawnLocation.position, Quaternion.identity, GameHandler.Instance.npcContainer);
        momSpeech = momNPC.GetComponent<NPCMain>().speechBubbleLocation;
        momNpcMovement = momNPC.GetComponent<NPCMovement>();
    }

    public void FlipMom()
    {
        momNPC.GetComponent<NPCAnimation>().SetNPCDirection(true, false);
        SpeechBubbleHandler.Instance.AddSpeechBubble(momSpeech, npcMomSpeech1);
        oneShot.PlayOneShot(speechBubbleSound);
        Invoke("StartMovingMomToPlayer", 3f);
    }

    public void StartMovingMomToPlayer()
    {
        momNPC.GetComponent<NPCMovement>().SetTarget(PlayerMain.Instance.transform);
        waitForDistanceToClose = true;
    }
}
