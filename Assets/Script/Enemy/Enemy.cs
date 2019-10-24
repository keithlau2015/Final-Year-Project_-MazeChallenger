using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { IDLE, CHASING, DEAD }
    public enum EnemyType { Golem }

    public EnemyType enemyType;
    public State state;

    //The Base value
    private const int base_HitPoint = 0;
    private const float base_Speed = 30;
    private const int base_Health = 50;
    private const float base_SpawningRate = 0;

    //The Total value
    public int total_HitPoint;
    public float total_Speed;
    public int total_Health;
    public float total_spwaningRate;

    //The Buffed value
    public int buff_HitPoint;
    public float buff_Speed;
    public int buff_Health;
    public float buff_SpwaningRate;
    public int buffSpeedCounter, buffHealthCounter, buffHitPointCounter, buffSpawningRateCounter;

    public Transform attackAreaPosition;
    public Transform[] idleWaypoint;
    public Collider detectPlayer;

    public Enemy()
    {
        total_Health = base_Health;
        total_HitPoint = base_HitPoint;
        total_Speed = base_Speed;
        total_spwaningRate = base_SpawningRate;

        buffHealthCounter = buffHitPointCounter = buffSpawningRateCounter = buffSpeedCounter = 0;
        buff_Health = buff_HitPoint = 0;
        buff_Speed = buff_SpwaningRate = 0;

        state = State.IDLE;
    }

    //Health get & set
    public void setEnemyHealth(int health)
    {
        buff_Health += health;
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
        buff_Speed += speed;
        total_Speed += buff_Speed;
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
