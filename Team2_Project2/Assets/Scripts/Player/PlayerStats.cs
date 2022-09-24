using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamagable
{
    [SerializeField] CharacterSO characterStats = null;
    private HealthSystem healthSystem;

    void Start()
    {
        healthSystem = new HealthSystem(characterStats.maxHealth);

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    void Update()
    {

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
        Debug.Log("Player Died");
    }
}
