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
    private const float base_SpawningRate = 0.8f;

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

    //Behaviour
    private bool collisionWithObject = false, triggerWithPlayer = false, onGround = false, insideAttackArea = false, isSpawnAttackArea = false;

    //Animation
    private int attack_pattern = 0;

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

    //Behaviour get & set
    public void setEnemyCollisionWithObject(bool set)
    {
        this.collisionWithObject = set;
    }

    public bool getEnemyCollisionWithObject()
    {
        return this.collisionWithObject;
    }

    public void setEnemyTriggerWithPlayer(bool set)
    {
        this.triggerWithPlayer = set;
    }

    public bool getEnemyTriggerWithPlayer()
    {
        return this.triggerWithPlayer;
    }

    public void setOnGround(bool set)
    {
        this.onGround = set;
    }

    public bool getOnGround()
    {
        return this.onGround;
    }

    public void setInsideAttackArea(bool set)
    {
        this.insideAttackArea = set;
    }

    public bool getInsideAttackArea()
    {
        return this.insideAttackArea;
    }

    public void setIsSpawnAttackArea(bool set)
    {
        this.isSpawnAttackArea = set;
    }

    public bool getIsSpawnAttackArea()
    {
        return this.isSpawnAttackArea;
    }

    //Animator get & set
    public void setAttack_pattern(int attack_pattern)
    {
        this.attack_pattern = attack_pattern;
    }

    public int getAttack_pattern()
    {
        return this.attack_pattern;
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
