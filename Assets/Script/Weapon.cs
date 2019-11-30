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
        if (PlayerStatus.Instance.getPlayerAttacking() && collision.gameObject.tag == "Enemy")
        {
            durability--;
            Debug.Log(this.gameObject.name + " : " + durability);
        }
    }
}
