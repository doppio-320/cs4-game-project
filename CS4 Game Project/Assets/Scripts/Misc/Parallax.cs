using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float distanceIndex = 0.2f;
    private float initialXPos;

    private void Start()
    {
        initialXPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        float dist = Camera.main.transform.position.x * distanceIndex;

        transform.position = new Vector3(initialXPos + dist, transform.position.y, transform.position.z);
    }
}
