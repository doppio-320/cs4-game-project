using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public float followSpeed = 2.5f;
    public float verticalOffset = 1.75f;
    public float zoomRate = 0.5f;
    private float initialDepthOffset = 0f;
    private float initialCamSize = 1.85f;

    [Header("Custom Control")]
    public bool scriptHasControl = true;
    public Transform target;
    public float targetZoom;    

    private void Start()
    {
        //initialCamSize = Camera.main.orthographicSize;
        initialDepthOffset = Camera.main.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (scriptHasControl && PlayerMain.Instance.isActive)
        {
            Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            transform.position + new Vector3(0f, verticalOffset, initialDepthOffset), followSpeed * Time.fixedDeltaTime);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, initialCamSize, zoomRate);
        }
        else
        {
            if (target)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(target.position.x, target.position.y, initialDepthOffset), followSpeed * Time.fixedDeltaTime);
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomRate);
            }
        }
    }

    public void SetTarget(Transform _tr, float _tz = 1.85f)
    {
        scriptHasControl = false;
        target = _tr;
        targetZoom = _tz;
    }

    public void DeleteTarget()
    {
        scriptHasControl = true;
        target = null;        
    }
}
