using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDamagable
{
    [SerializeField] CharacterSO characterStats = null;
    private HealthSystem healthSystem;

    [SerializeField] private Slider hpSlider;

    void Start()
    {
        healthSystem = new HealthSystem(characterStats.maxHealth);

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        if(hpSlider != null)
        {
            hpSlider.value = healthSystem.GetHealthPercent();
        }
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

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ICollectable>() != null)
        {
            other.GetComponent<ICollectable>().Collected(gameObject);
        }
    }
}
