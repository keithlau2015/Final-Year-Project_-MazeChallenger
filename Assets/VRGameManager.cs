using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGameManager : MonoBehaviour
{
    private Vector3 gravity = new Vector3(0f, -9.89f, 0f);
    private void Awake()
    {
        Physics.gravity = gravity;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
