using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    static readonly PlayerStatus instance = new PlayerStatus();

    //Base
    private const int base_Health = 10, base_Hunger = 100;
    private const float base_Speed = 25;

    //Upgrade
    private int upgradeSlot_Health, upgradeSlot_Hunger;
    private float upgradeSlot_Speed;

    //Total
    private int total_Health, total_Hunger;
    private float total_Speed;

    private bool playerGetIntoNextLevel, playerAtTheMenu;

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
        total_Speed = base_Speed;
        playerGetIntoNextLevel = false;
    }
    
    public float getSpeed()
    {
        return instance.total_Speed;
    }

    public int getHealth()
    {
        return instance.total_Health;
    }

    public int getHunger()
    {
        return instance.total_Hunger;
    }

    public void setSpeed(float speed, string extraBuff)
    {
        float temp = this.total_Speed + speed;
        if(temp >= base_Speed && extraBuff == "")
        {
            this.total_Speed = base_Speed;
        }
        else if (temp >= base_Speed && extraBuff.Equals("upgradeSpeed"))
        {
            if(temp > base_Speed + this.upgradeSlot_Speed)
            {
                this.upgradeSlot_Speed = base_Speed + this.upgradeSlot_Speed;
            }
            else
            {
                this.total_Speed = temp;
            }
        }
        else
        {
            this.total_Speed = temp;
        }
    }

    public void setHealth(int health, string extraBuff)
    {
        int temp = this.total_Health + health;
        if (temp >= base_Health && extraBuff == "")
        {
            this.total_Health = base_Health;
        }
        else if(temp >= base_Health && extraBuff.Equals("upgradeHP"))
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

    public void resetData()
    {
        this.upgradeSlot_Health = 0;
        this.upgradeSlot_Hunger = 0;
        this.total_Health = 0;
        this.total_Hunger = 0;
    }
}
