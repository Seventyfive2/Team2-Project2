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

    [Header("Stats")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackSpeed = 1f;

    [Header("UI")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private TMP_Text enemyTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Components")]
    [SerializeField] private EnemyMovement pathfinding;

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

        pathfinding.SetTarget(GameObject.Find("Player").transform);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] meleeTargets = Physics.OverlapSphere(transform.position, 3);
        if (meleeTargets.Length != 0)
        {
            for (int i = 0; i < meleeTargets.Length; i++)
            {
                if (meleeTargets[i].transform.GetComponent<IDamagable>() != null && meleeTargets[i].gameObject != gameObject)
                {
                    meleeTargets[i].transform.GetComponent<IDamagable>().TakeDamage(2);
                }
            }
        }
    }

    #region Health Functions
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
    #endregion
}
