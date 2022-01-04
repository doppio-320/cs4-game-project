using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : InteractibleObject
{
    public AudioClip doorSound;
    public AudioClip doorDenySound;
    public bool doorIsLocked = false;
    public GameObject currentRoom;
    public GameObject nextRoom;
    public Transform teleportPosition;

    public override void Interact()
    {
        base.Interact();

        if (doorIsLocked)
        {
            if(doorDenySound != null)
            {
                PlayerMain.Instance.GetComponent<PlayerSound>().PlayMiscSound(doorDenySound);
            }            
            return;
        }

        currentRoom.SetActive(false);
        nextRoom.SetActive(true);

        PlayerMain.Instance.transform.position = teleportPosition.position;
        Camera.main.transform.position = new Vector3(0, PlayerMain.Instance.playerHeight, 0) + teleportPosition.position;

        if(doorSound != null)
        {
            PlayerMain.Instance.GetComponent<PlayerSound>().PlayMiscSound(doorSound);
        }
    }
}
