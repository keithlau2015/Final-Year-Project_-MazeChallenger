using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Drawer"))
        {
            transform.SetParent(collision.transform);
        }
        if (collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Drawer"))
            {
                transform.SetParent(collision.transform);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Drawer"))
        {
            transform.SetParent(null);
        }
        if (collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Drawer"))
            {
                transform.SetParent(null);
            }
        }
    }
}
