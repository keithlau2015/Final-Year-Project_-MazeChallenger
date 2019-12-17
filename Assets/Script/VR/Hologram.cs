using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{
    public GameObject _object;

    private void OnTriggerEnter(Collider other)
    {
        _object.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        _object.SetActive(true);
    }
}
