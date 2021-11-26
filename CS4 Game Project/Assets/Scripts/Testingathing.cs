using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testingathing : MonoBehaviour
{
    private InteractibleObject interactible;

    private void OnEnable()
    {
        interactible = GetComponent<InteractibleObject>();

        interactible.OnInteracted += Eut;
        Destroy(this);
    }

    private void OnDisable()
    {
        interactible.OnInteracted -= Eut;
    }

    private void Eut(InteractibleObject _obj)
    {
        Debug.Log("Bayot si " + _obj.title);
    }
}
