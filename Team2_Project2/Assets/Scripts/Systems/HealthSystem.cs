using System;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    private int health;
    private int maxHealth;
    public event EventHandler OnDeath;

    public HealthSystem(int value)
    {
        maxHealth = value;
        health = maxHealth;
    }
    
    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return (float) health / maxHealth;
    }

    public void Damage(int amt)
    {
        health -= amt;
        if (health < 0) health = 0;

        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);

        if(health <= 0)
        {
            if (OnDeath != null) OnDeath(this, EventArgs.Empty);
        }
    }
    public void Heal(int amt)
    {
        health += amt;
        if (health > maxHealth) health = maxHealth;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
