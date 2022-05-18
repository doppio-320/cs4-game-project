using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour
{
    public List<Transform> targets;    
    public float depthOffset;
    public float heightOffset;
    public float smoothTime = 0.5f;

    public float zoomLimiter = 5f;

    public float minZoom = 3f;
    public float maxZoom = 8f;

    private Vector3 velocity;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.Find("Camera").GetComponent<Camera>();

        depthOffset = transform.position.z;

        heightOffset = transform.position.y - targets[0].position.y;

        CalculateCamera(true);
    }

    private void LateUpdate()
    {
        if (GameHandler.Instance.pauseState != PauseState.None && GameHandler.Instance.pauseState != PauseState.Cutscene)
            return;

        CalculateCamera(false);
    }

    private void CalculateCamera(bool _instant)
    {
        if (targets.Count == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPos = centerPoint + new Vector3(0f, 0f, depthOffset);        
        
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetFurthestDistance() / zoomLimiter);
        if(_instant)
        {
            cam.orthographicSize = newZoom;
            transform.position = newPos;
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
        }
    }

    private float GetFurthestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position + new Vector3(0f, heightOffset, 0f), Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position + new Vector3(0f, heightOffset, 0f));
        }

        return bounds.center;
    }
}
