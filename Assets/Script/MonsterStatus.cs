using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public int hitPoints { get; set; }
    public float attackSpeed { get; set; }
    public float speed { get; set; }
    public int damage { get; set; }
    public float attackRange { get; set; }
    public float spawnningRate { get; set; }

    public MonsterStat(int hitPoints, float attackSpeed, float speed, int damage)
    {

    }
}
