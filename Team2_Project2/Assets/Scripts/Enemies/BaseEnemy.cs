using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] private string enemyName = "Enemy";

    [SerializeField] private bool isSpecialEnemy = false;
    private HealthSystem healthSystem;

    [SerializeField] private int maxHealth = 1;

    [Header("UI")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private TMP_Text enemyTextName;
    [SerializeField] private Slider healthSlider;

    void Start()
    {
        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDeath += HealthSystem_OnDeath;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        if(isSpecialEnemy)
        {
            enemyCanvas.SetActive(true);

            enemyTextName.text = enemyName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void Heal(int amt)
    {
        throw new System.NotImplementedException();
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        healthSlider.value = healthSystem.GetHealthPercent();
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
