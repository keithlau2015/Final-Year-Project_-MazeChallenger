using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Golem, FloatingEnemy }
    public EnemyType enemyType;

    //The Total value
    private  int hitPoint;
    private float speed;
    private int health;
    private float spawningRate;

    //Animation
    private int attack_pattern;

    public Enemy()
    {
        hitPoint = 1;
        speed = 10;
        health = 3;
        spawningRate = 0.8f;

        attack_pattern = 0;
    }    

    //Health get & set
    public void setEnemyHealth(int health, string extraBuff)
    {
        if (extraBuff == "") this.health += health;
        else if (extraBuff == "upgradeEnemy") this.health += health;
    }

    public int getEnemyHealth()
    {
        return health;
    }

    //Speed get & set
    public void setEnemySpeed(float speed, string extraBuff)
    {
        if (extraBuff == "") this.speed += speed;
        else if (extraBuff == "upgradeEnemy") this.speed += speed;
    }

    public float getEnemySpeed()
    {
        return speed;
    }

    //Spawning rate get & set
    public void setEnemySpawningRate(float spawningRate, string extraBuff)
    {
        if (extraBuff == "") this.spawningRate += spawningRate;
        else if (extraBuff == "upgradeEnemy") this.spawningRate += spawningRate;
    }

    public float getEnemySpwaningRate()
    {
        return spawningRate;
    }

    //Hit point get & set
    public void setEnemyHitPoint(int hitPoint, string extraBuff)
    {
        if (extraBuff == "") this.hitPoint += hitPoint;
        else if (extraBuff == "upgradeEnemy") this.hitPoint += hitPoint;
    }

    public int getEnemyHitPoint()
    {
        return hitPoint;
    }

    //Reset
    public void resetEnemyValue()
    {
        hitPoint = 1;
        speed = 10;
        health = 3;
        spawningRate = 0.8f;

        attack_pattern = 0;
    }
}
