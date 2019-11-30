﻿using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    static readonly PlayerStatus instance = new PlayerStatus();
    //Default
    private const int HEALTH = 3, HUNGER = 100, COINS = 0;
    private const float SPEED = 50;

    //Current
    private int current_Health, current_Hunger, current_Coins;
    private float current_Speed;

    //Total
    private int total_Health, total_Hunger;
    private float total_Speed;

    //Check player status
    private bool playerGetIntoNextLevel, playerAtTheMenu, attacking;

    //UI
    private bool playerCanInteractWithOtherObject, playerCanInteractWithVendingMachine;
    private string priceUIText, killedBy;

    //Stage
    private int reachedLevels;

    //Skill
    private bool healingSkill, dualBladeSkill, bladeThrowingSkill;
    private float timer;
    private int healingTime;

    public static PlayerStatus Instance
    {
        get
        {
            return instance;
        }
    }

    private PlayerStatus()
    {
        total_Health = HEALTH;
        total_Hunger = HUNGER;
        total_Speed = SPEED;
        reachedLevels = 0;
        timer = 0;
        healingTime = 10;
        current_Health = total_Health;
        current_Hunger = total_Hunger;
        current_Speed = total_Speed;
        healingSkill = playerGetIntoNextLevel = playerCanInteractWithOtherObject = playerCanInteractWithVendingMachine = attacking = false;
        priceUIText = killedBy = "";
    }

    public string getPriceUIText()
    {
        return priceUIText;
    }

    public bool getPlayerCanInteractWithOtherObject()
    {
        return instance.playerCanInteractWithOtherObject;
    }

    public bool getPlayerCanInteractWithVendingMachine()
    {
        return instance.playerCanInteractWithVendingMachine;
    }

    public int getCoins()
    {
        return instance.current_Coins;
    }
    
    public float getSpeed()
    {
        return instance.current_Speed;
    }

    public int getHealth()
    {
        return instance.current_Health;
    }

    public int getHunger()
    {
        return instance.current_Hunger;
    }

    public void setPlayerCanInteractWithOtherObject(bool interacted)
    {
        playerCanInteractWithOtherObject = interacted;
    }

    public void setPlayerCanInteractWithVendingMachine(bool interacted)
    {
        playerCanInteractWithVendingMachine = interacted;
    }

    public void setCoins(int coins)
    {
        this.current_Coins = this.current_Coins + coins;
    }

    public void setSpeed(float speed, string extraBuff)
    {
        if(extraBuff == "")
        {
            return;
        }
        else if(extraBuff == "upgradeSpeed" && total_Speed < 150f)
        {
            total_Speed += speed;
            current_Speed = total_Speed;
        }
    }

    public void setHealth(int health, string extraBuff)
    {
        if(extraBuff == "")
        {
            if(current_Health + health >= total_Health)
            {
                current_Health = total_Health;
            }
            else if(current_Health + health <= 0)
            {
                current_Health = 0;
            }
            else
            {
                current_Health += health;
            }
        }
        else if(extraBuff == "upgradeHealth")
        {
            total_Health += health;
        }
    }

    public void setPriceUIText(string price)
    {
        priceUIText = price;
    }

    public void setHunger(int hunger, string extraBuff)
    {
        if(extraBuff == "")
        {
            if (this.current_Hunger + hunger >= total_Hunger)
            {
                this.current_Hunger = total_Hunger;
            }
            else
            {
                this.current_Hunger += hunger;
            }
        }
    }    

    public void setPlayerGetIntoNextLevel(bool playerGetIntoNextLevel)
    {
        this.playerGetIntoNextLevel = playerGetIntoNextLevel;
    }

    public bool getPlayerGetIntoNextLevel()
    {
        return this.playerGetIntoNextLevel;
    }

    public void setPlayerAtTheMenu(bool playerAtTheMenu)
    {
        this.playerAtTheMenu = playerAtTheMenu;
    }

    public bool getPlayerAtTheMenu()
    {
        return this.playerAtTheMenu;
    }

    public void setPlayerAttacking(bool set)
    {
        this.attacking = set;
    }

    public bool getPlayerAttacking()
    {
        return attacking;
    }

    public void setPlayerReachLevels(int level)
    {
        this.reachedLevels += level;
    }

    public int getPlayerReachLevels()
    {
        return this.reachedLevels;
    }

    public void setPlayerKilledBy(string killedBy)
    {
        this.killedBy = killedBy;
    }

    public string getPlayerKilledBy()
    {
        return this.killedBy;
    }

    //Skill
    public void setActiveHealingSkill(bool active)
    {
        healingSkill = active;
    }

    public bool getActiveHealingSkill()
    {
        return healingSkill;
    }

    public void HealingSkill()
    {
        timer += Time.deltaTime;
        if (timer > healingTime && current_Health > 0 && Time.timeScale > 0 && !playerAtTheMenu && !playerGetIntoNextLevel)
        {
            setHealth(1, "");
            timer = 0f;
        }
    }

    public void setHealingTime(int set)
    {
        if(this.healingTime > 2)this.healingTime -= set;
    }

    //reset
    public void resetData()
    {
        this.priceUIText = this.killedBy = "";
        this.current_Health = HEALTH;
        this.current_Hunger = HUNGER;
        this.current_Speed = SPEED;
        this.total_Health = HEALTH;
        this.total_Hunger = HUNGER;
        this.total_Speed = SPEED;
        this.current_Coins = COINS;
        this.reachedLevels = 0;
        healingSkill = playerGetIntoNextLevel = playerCanInteractWithOtherObject = playerCanInteractWithVendingMachine = attacking = false;
    }
}
