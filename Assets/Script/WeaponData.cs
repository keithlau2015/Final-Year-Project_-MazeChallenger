using System.Collections;
using System.Collections.Generic;

public class WeaponData
{
    private int damage;
    private float attackSpeed;
    private int durability;

    public WeaponData(int damage, float attackSpeed, int durability)
    {
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.durability = durability;
    }

    /// <summary>
    /// set function
    /// </summary>
    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setAttackSpeed(float attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }

    public void setDurability(int durability)
    {
        this.durability = durability;
    }

    /// <summary>
    /// get function
    /// <summary>    
    public int getDamage()
    {
        return this.damage;
    }

    public float getAttackSpeed()
    {
        return this.attackSpeed;
    }

    public int getDurability()
    {
        return this.durability;
    }
}
