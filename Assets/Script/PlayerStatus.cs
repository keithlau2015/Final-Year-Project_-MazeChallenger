using System.Collections;
using System.Collections.Generic;

public class PlayerStatus
{
    static readonly PlayerStatus instance = new PlayerStatus();

    private const int base_Health = 10, base_Hunger = 100;
    private int upgradeSlot_Health, upgradeSlot_Hunger;
    private int total_Health, total_Hunger;

    private bool playerGetIntoNextLevel;

    public static PlayerStatus Instance
    {
        get
        {
            return instance;
        }
    }

    private PlayerStatus()
    {
        total_Health = base_Health;
        total_Hunger = base_Hunger;
        playerGetIntoNextLevel = false;
    }
    
    public int getHealth()
    {
        return instance.total_Health;
    }

    public int getHunger()
    {
        return instance.total_Hunger;
    }

    public void setHealth(int health, string extraBuff)
    {
        int temp = this.total_Health + health;
        if (temp >= 10 && extraBuff == "")
        {
            this.total_Health = base_Health;
        }
        else if(temp >= 10 && extraBuff.Equals("upgradeHP"))
        {
            //the number is over the base health plus the upgrade health
            if(temp > base_Health + this.upgradeSlot_Health)
            {
                //full health
               this.total_Health = base_Health + this.upgradeSlot_Health;
            }
            else
            {
                this.total_Health = temp;
            }
        }
        else
        {
            this.total_Health += health;
        }
    }

    public void setHunger(int hunger)
    {
        int temp = this.total_Hunger += hunger;
        if (temp >= 100)
        {
            this.total_Hunger = base_Hunger;
        }
        else
        {
            this.total_Hunger += hunger;
        }
    }
    
    public void setTotalHealth()
    {
        this.total_Health = total_Health + 1;
    }

    public void setPlayerGetIntoNextLevel(bool playerGetIntoNextLevel)
    {
        this.playerGetIntoNextLevel = playerGetIntoNextLevel;
    }

    public bool getPlayerGetIntoNextLevel()
    {
        return this.playerGetIntoNextLevel;
    }
}
