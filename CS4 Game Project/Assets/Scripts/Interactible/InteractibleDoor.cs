using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : InteractibleObject
{
    public GameObject currentRoom;
    public GameObject nextRoom;
    public Transform teleportPosition;

    public override void Interact()
    {
        base.Interact();

        currentRoom.SetActive(false);
        nextRoom.SetActive(true);

        PlayerMain.Instance.transform.position = teleportPosition.position;
        Camera.main.transform.position = new Vector3(0, PlayerMain.Instance.playerHeight, 0) + teleportPosition.position;
    }
}
