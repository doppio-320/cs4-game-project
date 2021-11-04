using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public float followSpeed = 2.5f;
    public float verticalOffset = 1.75f;    
    private float initialDepthOffset = 0f;

    private void Start()
    {
        initialDepthOffset = Camera.main.transform.position.z;
    }

    private void FixedUpdate()
    {
        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            transform.position + new Vector3(0f, verticalOffset, initialDepthOffset), followSpeed * Time.fixedDeltaTime);
    }
}
