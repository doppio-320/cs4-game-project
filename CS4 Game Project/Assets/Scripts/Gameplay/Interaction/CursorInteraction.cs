using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteraction : MonoBehaviour
{
    public static PlayerInteractSystem playerInteractSystem;

    private void Start()
    {
        playerInteractSystem = GetComponentInParent<PlayerInteractSystem>();
    }

    void Update()
    {
        transform.position = CameraFollowPlayer.currentCam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<TooltipObject>() != null)
        {
            playerInteractSystem.AddTooltip(collision.gameObject);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<TooltipObject>() != null)
        {
            playerInteractSystem.RemoveTooltip();
        }
    }
}
