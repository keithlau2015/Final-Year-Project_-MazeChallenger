using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health, damage;
    public float spwanRate, attackSpeed, speed;
    public string enemyType;

    public Animator enemyAnimator
    {
        get
        {
            return this.GetComponent<Animator>();
        }
    }
}
