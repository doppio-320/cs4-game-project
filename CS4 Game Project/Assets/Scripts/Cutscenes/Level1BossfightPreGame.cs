using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1BossfightPreGame : MonoBehaviour
{
    public int helpIndex;
    public GameObject[] helpObjects;
    public GameObject countdownObject;
    public Text countdownText;
    public GameObject playerGameObject;
    public GameObject bossGameObject;
    public GameObject mainCamObj;
    public int showLeftUIIndex;
    private GameObject child;
    private bool finalCountdown;
    private bool cutsceneActive = true;
    private float timeLeftCountdown;

    private void Start()
    {        
        GameHandler.Instance.SetCutsceneState(true);
        child = transform.Find("Panel").gameObject;
        finalCountdown = false;
        PlayerCombatToolsDisplay.Instance.transform.Find("PrimaryHUD").gameObject.SetActive(false);
        PlayerCombatToolsDisplay.Instance.transform.Find("EnemyInfo").gameObject.SetActive(false);

        StartCutscene();

        //ReadyFight();
    }

    private void Update()
    {
        if (!cutsceneActive)
            return;

        if (Input.anyKeyDown && !finalCountdown)
        {
            if (helpIndex < helpObjects.Length - 1)
            {
                helpIndex++;
                foreach (var obj in helpObjects)
                {
                    obj.SetActive(false);
                }
                UpdateTip();

                if (showLeftUIIndex == helpIndex)
                {
                    PlayerCombatToolsDisplay.Instance.transform.Find("PrimaryHUD").gameObject.SetActive(true);
                    PlayerCombatToolsDisplay.Instance.transform.Find("EnemyInfo").gameObject.SetActive(true);
                }
            }
            else if(helpIndex == helpObjects.Length - 1)
            {                
                ReadyFight();
            }
        }

        if(finalCountdown)
        {
            timeLeftCountdown -= Time.deltaTime;
            countdownText.text = ((int)Mathf.Floor(timeLeftCountdown)).ToString();

            if(timeLeftCountdown <= 0f)
            {
                finalCountdown = false;
                cutsceneActive = false;
                countdownObject.SetActive(false);

                StartFight();
            }
        }
    }

    private void ReadyFight()
    {
        timeLeftCountdown = 3.9f;
        playerGameObject.transform.Find("Visuals").GetComponent<Animator>().SetBool("stillPeaceful", false);
        bossGameObject.transform.Find("Visuals").GetComponent<Animator>().SetBool("stillPeaceful", false);

        PlayerCombatToolsDisplay.Instance.transform.Find("PrimaryHUD").gameObject.SetActive(true);
        PlayerCombatToolsDisplay.Instance.transform.Find("EnemyInfo").gameObject.SetActive(true);

        helpObjects[helpIndex].SetActive(false);
        countdownObject.SetActive(true);
        finalCountdown = true;

        Level1BossfightMusic.Instance.StartCombatMusic();
    }

    private void StartFight()
    {
        playerGameObject.GetComponent<CombatPlayerController>().enabled = true;
        playerGameObject.GetComponent<CombatPlayerAnimation>().enabled = true;
        playerGameObject.GetComponent<CombatPlayerFighting>().enabled = true;
        playerGameObject.GetComponent<CombatPlayerHealth>().enabled = true;
        playerGameObject.GetComponent<CombatPlayerTools>().enabled = true;

        bossGameObject.GetComponent<NPCAnimation>().enabled = true;
        bossGameObject.GetComponent<NPCMovement>().enabled = true;
        bossGameObject.GetComponent<BossBasicCombat>().enabled = true;
        bossGameObject.GetComponent<BossSpriteEffects>().enabled = true;        

        Camera.current.gameObject.SetActive(false);
        mainCamObj.SetActive(true);

        GameHandler.Instance.SetCutsceneState(false);
        Destroy(this);
    }

    private void StartCutscene()
    {
        helpIndex = 0;
        UpdateTip();
    }    

    private void UpdateTip()
    {
        helpObjects[helpIndex].SetActive(true);
        helpObjects[helpIndex].GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().enabled = false;
        helpObjects[helpIndex].GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().enabled = true;
    }
}
