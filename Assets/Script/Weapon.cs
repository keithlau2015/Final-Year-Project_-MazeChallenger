using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damge;
    public int durability;
    public float spawningRate;
    public string counterType;

    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerStatus.Instance.getPlayerAttacking() && collision.gameObject.CompareTag("Enemy"))
        {
            durability--;
        }
        if (PlayerStatus.Instance.getPlayerAttacking() && collision.gameObject.CompareTag("Breakable"))
        {
            durability--;
            if (collision.gameObject.name == "HeadSkull(Clone)")
            {
                PlayerStatus.Instance.setSanity(+2);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().popDescriptionText("+ Sanity");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerStatus.Instance.getPlayerAttacking() && other.gameObject.CompareTag("Breakable"))
        {
            durability--;
        }
        if(PlayerStatus.Instance.getPlayerAttacking() && other.name == "Wall_0_Breakable")
        {
            durability--;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            PlayerStatus.Instance.setPlayerAttacking(false);
        }
        if (collision.gameObject.CompareTag("Breakable"))
        {
            PlayerStatus.Instance.setPlayerAttacking(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
            if (other.gameObject.CompareTag("Enemy"))
            {
                PlayerStatus.Instance.setPlayerAttacking(false);
            }
            if (other.gameObject.CompareTag("Breakable"))
            {
                PlayerStatus.Instance.setPlayerAttacking(false);
            }
        }
}
