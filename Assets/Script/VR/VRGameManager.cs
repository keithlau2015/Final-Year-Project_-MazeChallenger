using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VRGameManager : MonoBehaviour
{
    private string path_Char = "/Subject_Adam.Char";

    [SerializeField]
    private GameObject smoke, errorVideo;

    private Vector3 gravity = new Vector3(0f, -9.89f, 0f);
    private void Awake()
    {
        Physics.gravity = gravity;
        if (File.Exists(path_Char))
        {
            smoke.SetActive(false);
            errorVideo.SetActive(false);
        }
        else
        {
            smoke.SetActive(true);
            errorVideo.SetActive(true);
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
