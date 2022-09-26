using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    [SerializeField] private string enemyName = "Enemy";

    [SerializeField] private bool isSpecialEnemy = false;
    private HealthSystem healthSystem;

    [SerializeField] private float stateRefreshRate = .5f;
    [SerializeField] private float targetRefreshRate = .5f;

    public enum State { Idle, Moving, Attacking }
    private State currentState = State.Idle;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int attackDamage = 2;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackSpeed = 1f;
    private float attackTime = 0f;

    [Header("UI")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private TMP_Text enemyTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Components")]
    [SerializeField] private EnemyMovement pathfinding;

    void Awake()
    {
        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDeath += HealthSystem_OnDeath;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;

        if(isSpecialEnemy)
        {
            enemyCanvas.SetActive(true);

            enemyTextName.text = enemyName;
        }

        pathfinding.GetAgent().stoppingDistance = attackRange;

        //pathfinding.SetTarget(GameObject.Find("Player").transform);
        StartCoroutine(GetTarget());
        StartCoroutine(StateMachine());
        attackTime = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == State.Attacking && attackTime <= 0)
        {
            Collider[] meleeTargets = Physics.OverlapSphere(GetAttackPosition(), attackRange);
            if (meleeTargets.Length != 0)
            {
                for (int i = 0; i < meleeTargets.Length; i++)
                {
                    if (meleeTargets[i].transform.GetComponent<IDamagable>() != null && meleeTargets[i].CompareTag("Player"))
                    {
                        meleeTargets[i].transform.GetComponent<IDamagable>().TakeDamage(attackDamage);
                    }
                }

                Debug.Log("Attack");
                attackTime = attackSpeed;
            }
        }
        else if(attackTime > 0 && currentState == State.Attacking)
        {
            attackTime -= Time.deltaTime;
        }
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            //float distanceToTarget = Vector3.Distance(GetAttackPosition(), pathfinding.GetTarget().position);

            Collider[] colliders = Physics.OverlapSphere(GetAttackPosition(), attackRange);

            List<Collider> colliderList = GalaxyRandom.ConvertToList(colliders);

            bool targetInRange = colliderList.Contains(pathfinding.GetTarget().GetComponent<Collider>());

            if (pathfinding.GetTarget() == null)
            {
                currentState = State.Idle;
            }
            else if(targetInRange)
            {
                currentState = State.Attacking;
            }
            else if(!targetInRange)
            {
                currentState = State.Moving;
            }
            yield return new WaitForSeconds(stateRefreshRate);
        }
    }


    IEnumerator GetTarget()
    {
        while (true)
        {
            Transform highestThreat = null;

            Collider[] targets = Physics.OverlapSphere(transform.position, 50);
            if (targets.Length != 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].transform.GetComponent<AggroSystem>() != null && targets[i].gameObject != gameObject)
                    {
                        if(targets[i].GetComponent<BaseBuilding>() != null)
                        {
                            if(targets[i].GetComponent<BaseBuilding>().GetBuildingState())
                            {
                                continue;
                            }
                        }

                        if (highestThreat == null)
                        {
                            highestThreat = targets[i].transform;
                        }
                        else if (targets[i].transform.GetComponent<AggroSystem>().threatLevel > highestThreat.GetComponent<AggroSystem>().threatLevel)
                        {
                            highestThreat = targets[i].transform;
                        }
                    }
                }
            }

            pathfinding.SetTarget(highestThreat);
            yield return new WaitForSeconds(targetRefreshRate);
        }
    }

    #region Health Functions
    public void TakeDamage(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void Heal(int amt)
    {
        healthSystem.Heal(amt);
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        healthSlider.value = healthSystem.GetHealthPercent();
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        LootManager.instance.GetGoldDrop(transform.position);
        Destroy(gameObject);
    }
    #endregion

    public State GetEnemyState()
    {
        return currentState;
    }

    public Vector3 GetAttackPosition()
    {
        return transform.position + transform.forward * attackRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRange);
    }
}
