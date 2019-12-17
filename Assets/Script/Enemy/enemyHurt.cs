using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHurt : MonoBehaviour
{
    [SerializeField]
    private Material hurt, normalMaterial;

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.GetComponent<EnemyBehaviour>().getBeingAttack() && PlayerStatus.Instance.getPlayerAttacking())
        {
            this.GetComponent<Renderer>().material = hurt;
        }
        else
        {
            this.GetComponent<Renderer>().material = normalMaterial;
        }
    }
}
