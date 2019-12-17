using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int durability;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "bullet_0(Clone)")
        {
            Destroy(other.gameObject);
            durability--;
        }
        if (other.CompareTag("attack_point"))
        {
            durability--;
        }
    }
}
