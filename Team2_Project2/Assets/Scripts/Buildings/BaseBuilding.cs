using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseBuilding : MonoBehaviour, IDamagable
{
    private HealthSystem healthSystem;

    [SerializeField] private string buildingName = "Building";
    [SerializeField] private int maxHealth;
    public bool isDestroyed;
    private BuildingData buildingData;
    [SerializeField] private Sprite buildingSprite;

    [Header("UI")]
    [SerializeField] private TMP_Text buildingTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Components")]
    [SerializeField] private GameObject model;
    [SerializeField] private new Collider collider;
    [SerializeField] private NavMeshObstacle obstacle;

    public UnityEvent damaged;
    public UnityEvent destroyed;

    public virtual void Start()
    {
        buildingTextName.text = buildingName;

        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDeath += HealthSystem_OnDeath;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        healthSlider.value = healthSystem.GetHealthPercent();

        //FindBuildingStatus();

        if(buildingData != null)
        {
            TakeDamage(buildingData.healthSystem.GetMissingHealth());
        }

        Vector3 lookDir = Vector3.forward;
        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);

        buildingTextName.transform.parent.rotation = qt;
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
        if(damaged!= null)
        {
            damaged.Invoke();
        }
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

        if(destroyed != null)
        {
            destroyed.Invoke();
        }
    }

    public void FindBuildingStatus()
    {
        for (int i = 0; i < PlayerData.instance.buildingStatus.Count; i++)
        {
           if(PlayerData.instance.buildingStatus[i].name == buildingName)
           {
                buildingData = PlayerData.instance.buildingStatus[i];
                return;
           }
        }

        buildingData = new BuildingData(buildingName, healthSystem, buildingSprite);
        PlayerData.instance.buildingStatus.Add(buildingData);
    }
}
