using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int health;
    public float probability;
    public string enemyType;

    public Animator enemyAnimator
    {
        get
        {
            return this.GetComponent<Animator>();
        }
    }
}
