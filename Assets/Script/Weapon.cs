using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damge;
    public int durability;
    public float spawningRate;
    public string counterType;

    private bool isDurabilityMinus;

    private void Awake()
    {
        isDurabilityMinus = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerStatus.Instance.getPlayerAttacking() && collision.gameObject.tag == "Enemy" && !isDurabilityMinus)
        {
            durability--;
            isDurabilityMinus = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            isDurabilityMinus = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            isDurabilityMinus = false;
        }
    }
}
