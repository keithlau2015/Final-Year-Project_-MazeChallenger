using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Golem }

    public EnemyType enemyType;

    //The Base value
    private const int base_HitPoint = 0;
    private const float base_Speed = 10;
    private const int base_Health = 50;
    private const float base_SpawningRate = 0;

    //The Total value
    private  int total_HitPoint;
    private float total_Speed;
    private int total_Health;
    private float total_spwaningRate;

    //The Buffed value
    private int buff_HitPoint;
    private float buff_Speed;
    private int buff_Health;
    private float buff_SpwaningRate;
    private int buffSpeedCounter, buffHealthCounter, buffHitPointCounter, buffSpawningRateCounter;


    public Enemy()
    {
        total_Health = base_Health;
        total_HitPoint = base_HitPoint;
        total_Speed = base_Speed;
        total_spwaningRate = base_SpawningRate;

        buffHealthCounter = buffHitPointCounter = buffSpawningRateCounter = buffSpeedCounter = 0;
        buff_Health = buff_HitPoint = 0;
        buff_Speed = buff_SpwaningRate = 0;
    }

    //Health get & set
    public void setEnemyHealth(int health)
    {
        int temp = buff_Health + health;
        total_Health += buff_Health;
        buffHealthCounter++;
    }

    public int getEnemyHealth()
    {
        return total_Health;
    }

    public int getEnemyHealthBuffCounter()
    {
        return buffHealthCounter;
    }

    //Speed get & set
    public void setEnemySpeed(float speed)
    {
        buff_Speed = buff_Speed + speed;
        total_Speed = buff_Speed + base_Speed;
        buffSpeedCounter++;
    }

    public float getEnemySpeed()
    {
        return total_Speed;
    }

    public int getEnemyBuffSpeedCounter()
    {
        return buffSpeedCounter;
    }

    //Spawning rate get & set
    public void setEnemySpawningRate(float spawningRate)
    {
        buff_SpwaningRate += spawningRate;
        total_spwaningRate += buff_SpwaningRate;

    }

    public float getEnemySpwaningRate()
    {
        return total_spwaningRate;
    }

    public int getEnemyBuffSpawningRateCounter()
    {
        return buffSpawningRateCounter;
    }

    //Hit point get & set
    public void setEnemyHitPoint(int hitPoint)
    {
        buff_HitPoint += hitPoint;
        total_HitPoint += buff_HitPoint;
        buffHitPointCounter++;
    }

    public int getEnemyHitPoint()
    {
        return total_HitPoint;
    }

    public int getEnemyBuffHitPointCounter()
    {
        return buffHitPointCounter;
    }

    //Reset
    public void resetEnemyValue()
    {
        total_Health = base_Health;
        total_HitPoint = base_HitPoint;
        total_Speed = base_Speed;
        total_spwaningRate = base_SpawningRate;

        buffHealthCounter = buffHitPointCounter = buffSpawningRateCounter = buffSpeedCounter = 0;
        buff_Health = buff_HitPoint = 0;
        buff_Speed = buff_SpwaningRate = 0;
    }
}
