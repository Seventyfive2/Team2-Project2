using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseBuilding : MonoBehaviour, IDamagable
{
    private HealthSystem healthSystem;

    [SerializeField] private int maxHealth;
    private bool isDestroyed;

    [Header("UI")]
    [SerializeField] private TMP_Text buildingTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Components")]
    [SerializeField] private GameObject model;
    [SerializeField] private Collider collider;
    [SerializeField] private NavMeshObstacle obstacle;

    // Start is called before the first frame update
    void Start()
    {
        buildingTextName.text = name;

        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDeath += HealthSystem_OnDeath;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSlider.value = healthSystem.GetHealthPercent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetBuildingState()
    {
        return isDestroyed;
    }

    public void Heal(int amt)
    {
        healthSystem.Heal(amt);
    }

    public void TakeDamage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        healthSlider.value = healthSystem.GetHealthPercent();
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        isDestroyed = true;
        model.SetActive(false);
        obstacle.enabled = false;
        collider.enabled = false;
    }
}
