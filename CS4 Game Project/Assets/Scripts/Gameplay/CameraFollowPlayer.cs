using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public static Camera currentCam;
    public float lerpSpeed;
    private Transform target;

    private void OnEnable()
    {
        target = GameObject.Find("NormalPlayer").transform;
        currentCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0.75f, -10), lerpSpeed * Time.fixedDeltaTime);
    }
}
