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

    private Animator redBoxAnim;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 1;
    public int attackDamage = 2;
    public float attackRange = 1f;
    [SerializeField] private float attackSpeed = 1f;
    private float attackTime = 0f;

    [SerializeField] private bool prioritizeBuilding = false;

    [Header("UI")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private TMP_Text enemyTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Components")]
    public EnemyMovement pathfinding;

    void Awake()
    {

        redBoxAnim = GetComponent<Animator>();

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
            Attack();
            attackTime = attackSpeed;
        }
        else if(attackTime > 0 && currentState == State.Attacking)
        {
            attackTime -= Time.deltaTime;
        }

        if(enemyCanvas.activeInHierarchy)
        {
            Vector3 lookDir = Vector3.forward;
            float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
            Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);

            enemyCanvas.transform.rotation = qt;
        }
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            //float distanceToTarget = Vector3.Distance(GetAttackPosition(), pathfinding.GetTarget().position);

            Collider[] colliders = GetTargetsInRange();

            List<Collider> colliderList = GalaxyRandom.ConvertToList(colliders);

            bool targetInRange = colliderList.Contains(pathfinding.GetTarget().GetComponent<Collider>());

            if (pathfinding.GetTarget() == null)
            {
                currentState = State.Idle;
            }
            else if(targetInRange)
            {
                currentState = State.Attacking;
                redBoxAnim.SetBool("Attack", true);
            }
            else if(!targetInRange)
            {
                currentState = State.Moving;
                redBoxAnim.SetBool("Attack", false);
            }
            yield return new WaitForSeconds(stateRefreshRate);
        }
    }


    public virtual IEnumerator GetTarget()
    {
        while (true)
        {
            Transform highestThreat = null;

            Collider[] targets = Physics.OverlapSphere(transform.position, 50);
            if (targets.Length != 0)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    AggroSystem aggro = targets[i].transform.GetComponent<AggroSystem>();
                    if (aggro != null && targets[i].gameObject != gameObject)
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
                        else
                        {
                            //Gets adjusted aggro level
                            int tl = aggro.GetThreatLevel();

                            if (aggro.isBuilding && prioritizeBuilding)
                            {
                                tl = aggro.GetThreatLevel(true);
                            }

                            //Finds aggro level to beat
                            AggroSystem highestAggro = highestThreat.GetComponent<AggroSystem>();

                            int htl = highestAggro.GetThreatLevel();

                            if (highestAggro.isBuilding && prioritizeBuilding)
                            {
                                htl = highestAggro.GetThreatLevel(true);
                            }

                            //Compares aggro level
                            if (tl > htl)
                            {
                                highestThreat = targets[i].transform;
                            }
                        }
                    }
                }
            }

            pathfinding.SetTarget(highestThreat);
            yield return new WaitForSeconds(targetRefreshRate);
        }
    }

    public virtual Collider[] GetTargetsInRange()
    {
        return Physics.OverlapSphere(GetAttackPosition(), attackRange);
    }

    public virtual void Attack()
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
        }
    }

    #region Health Functions
    public void TakeDamage(int damage)
    {
        redBoxAnim.SetBool("Damaged", true);
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

    public virtual void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        redBoxAnim.SetBool("Dead", true);
        LootManager.instance.GetStandardDrops(transform.position);
        WaveManager.instance.EnemyDefeated();
        //Destroy(gameObject);
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

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRange);
    }
}
