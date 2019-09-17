using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private int hp;

    private int stamina;
    private float hunger;
    private float speed;
       
    public PlayerData(int hp, float hunger, float speed, int stamina) {
        this.hp = hp;
        this.stamina = stamina;
        this.hunger = hunger;
        this.speed = speed;
    }

    public void setHp(int hp)
    {
        this.hp = hp;
    }

    public void setStamina(int stamina)
    {
        this.stamina = stamina;
    }

    public void setHunger(int hunger)
    {
        this.hunger = hunger;
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }

    public int getHp(int hp)
    {
        return hp;
    }

    public int getStamina(int stamina)
    {
        return stamina;
    }

    public int getHunger(int hunger)
    {
        return hunger;
    }

    public int getSpeed(int speed)
    {
        return speed;
    }
}
