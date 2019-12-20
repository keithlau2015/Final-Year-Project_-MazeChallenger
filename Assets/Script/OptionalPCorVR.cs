using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class OptionalPCorVR : MonoBehaviour
{
    public void OnClickPCButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickVRButton()
    {
        if (XRDevice.isPresent)
        {
            XRSettings.LoadDeviceByName("OpenVR");
            XRSettings.enabled = true;
            SceneManager.LoadScene(5);
        }
        else
        {
            Application.Quit();
        }
    }
}
