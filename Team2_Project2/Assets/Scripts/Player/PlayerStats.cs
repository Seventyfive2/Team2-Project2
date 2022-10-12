using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour, IDamagable
{
    private int baseMaxHealth = 10;

    [SerializeField] private UnityEvent playerDiedEvent;

    private HealthSystem healthSystem;
    private PlayerUI ui;

    [SerializeField] private PlayerMovement movement;

    void Start()
    {
        ui = GameObject.Find("Player Canvas").GetComponent<PlayerUI>();

        PlayerAttribute playerHealth = PlayerData.instance.GetAttribute("Health");

        healthSystem = new HealthSystem(baseMaxHealth + playerHealth.GetStatIncrease());

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDeath += HealthSystem_OnDeath;

        ui.UpdateHealth(healthSystem.GetHealthPercent());
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        ui.UpdateHealth(healthSystem.GetHealthPercent());
    }

    public void Heal(int amt)
    {
        healthSystem.Heal(amt);
    }

    public void TakeDamage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        movement.isControllable = false;
        if (playerDiedEvent != null)
        {
            playerDiedEvent.Invoke();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ICollectable>() != null)
        {
            other.GetComponent<ICollectable>().Collected(gameObject);
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
