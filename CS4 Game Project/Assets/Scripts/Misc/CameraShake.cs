using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Instance Handling

    private static CameraShake instance;

    public static CameraShake Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;            
        }
    }

    #endregion

    public void CallShake(float _dur, float _mag)
    {
        StartCoroutine(ProcessShake(_dur, _mag));
    }

    private IEnumerator ProcessShake(float _duration, float _magnitude)
    {
        Vector3 initPosition = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < _duration)
        {
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;

            transform.localPosition = new Vector3(x, y, initPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = initPosition;
    }
}
