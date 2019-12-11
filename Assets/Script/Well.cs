using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    [HideInInspector]
    public int useableTime;
    // Start is called before the first frame update
    private void Start()
    {
        useableTime = Random.Range(0, 3);
    }
}
