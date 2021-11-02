using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChangeSubLevel : InteractibleObject
{
    public GameObject currentLevel;    
    public GameObject nextLevel;
    public Transform teleportPoint;

    public override void Interact()
    {
        base.Interact();
        CursorInteraction.playerInteractSystem.RemoveTooltip();
        NormalPlayer.Instance.transform.position = teleportPoint.position;
        CameraFollowPlayer.currentCam.transform.position = teleportPoint.position;

        currentLevel.SetActive(false);
        nextLevel.SetActive(true);
    }
}
