using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VRGameManager : MonoBehaviour
{
    private string path_Char = "/Subject_Adam.Char";

    [SerializeField]
    private GameObject errorVideo;

    private Vector3 gravity = new Vector3(0f, -9.89f, 0f);
    private void Awake()
    {
        Time.timeScale = 1;
        Physics.gravity = gravity;
        if (File.Exists(path_Char))
        {
            errorVideo.SetActive(false);
        }
        else
        {
            errorVideo.SetActive(true);
        }
    }
}