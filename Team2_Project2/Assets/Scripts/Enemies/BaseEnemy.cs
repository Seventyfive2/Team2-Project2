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

    [SerializeField] private float stateRefreshRate = .5f;
    [SerializeField] private float targetRefreshRate = .5f;

    public enum State { Idle, Moving, Attacking }
    private State currentState = State.Idle;
    [HideInInspector] public bool isAlive = true;

    [Header("Stats")]
    [SerializeField] private int maxHealth = 1;
    public int attackDamage = 2;
    public float attackRange = 1f;
    public float attackSpeed = 1f;
    [HideInInspector] public float attackTime = 0f;

    [SerializeField] private bool prioritizeBuilding = false;

    public bool lockedInState;


    [Header("UI")]
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private TMP_Text enemyTextName;
    [SerializeField] private Slider healthSlider;

    [Header("Animations")]
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private string attackParameter = "Attack";
    [SerializeField] private string damageParameter = "Damaged";
    [SerializeField] private string deathParameter = "Dead";
    [SerializeField] private float deathAnimLength = 1.67f;

    private bool hasAttackAnimation = false;
    private bool hasDamageAnimation = false;
    private bool hasDeathAnimation = false;

    [Header("Components")]
    public EnemyMovement pathfinding;
    public GameObject warning;

    void Awake()
    {
        if(enemyAnim == null)
        {
            enemyAnim = GetComponent<Animator>();
        }

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

        hasAttackAnimation = enemyAnim.GetBool(attackParameter);
        hasDamageAnimation = enemyAnim.GetBool(damageParameter);
        hasDeathAnimation = enemyAnim.GetBool(deathParameter);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(isAlive)
        {
            if (currentState == State.Attacking && attackTime <= 0)
            {
                Attack();
            }
            else if (attackTime > 0 && currentState == State.Attacking && !lockedInState)
            {
                attackTime -= Time.deltaTime;
            }

            if (enemyCanvas.activeInHierarchy)
            {
                Vector3 lookDir = Vector3.forward;
                float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
                Quaternion qt = Quaternion.AngleAxis(angle, Vector3.up);

                enemyCanvas.transform.rotation = qt;
            }
        }
    }

    IEnumerator StateMachine()
    {
        while (isAlive)
        {
            if(!lockedInState)
            {
                Collider[] colliders = GetTargetsInRange();

                List<Collider> colliderList = GalaxyRandom.ConvertToList(colliders);

                bool targetInRange = colliderList.Contains(pathfinding.GetTarget().GetComponent<Collider>());

                if (pathfinding.GetTarget() == null)
                {
                    currentState = State.Idle;
                }
                else if (targetInRange)
                {
                    currentState = State.Attacking;

                    if (enemyAnim != null)
                    {
                        if(!hasAttackAnimation)
                        {
                            enemyAnim.SetBool(attackParameter, true);
                        }
                    }
                }
                else if (!targetInRange)
                {
                    currentState = State.Moving;
                    if (enemyAnim != null)
                    {
                        if (!hasAttackAnimation)
                        {
                            enemyAnim.SetBool(attackParameter, false);
                        }
                    }
                }
            }
            
            yield return new WaitForSeconds(stateRefreshRate);
        }
    }

    public virtual IEnumerator GetTarget()
    {
        while (isAlive && pathfinding.GetTarget() == null)
        {
            Transform highestThreat = null;

            List<Transform> equalThreats = new List<Transform>();

            Collider[] targets = Physics.OverlapSphere(transform.position, 200);
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
                                equalThreats.Clear();
                            }
                            else if (tl == htl)
                            {
                                equalThreats.Add(targets[i].transform);
                            }
                        }
                    }
                }
            }
            else
            {
                highestThreat = GameObject.Find("PLayer").transform;
            }

            if(equalThreats.Count > 0)
            {
                equalThreats.Add(highestThreat);

                Transform closest = null;
                float shortestDistance = Mathf.Infinity;

                for (int i = 0; i < equalThreats.Count; i++)
                {
                    float distance = Vector3.Distance(transform.position, equalThreats[i].position);

                    if (distance < shortestDistance)
                    {
                        closest = equalThreats[i];
                        shortestDistance = distance;
                    }
                }

                highestThreat = closest;
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

        GetAttackSpeed();
    }

    #region Health Functions
    public void TakeDamage(int damage)
    {
        if (enemyAnim != null)
        {
            if (!hasDamageAnimation)
            {
                enemyAnim.SetBool(damageParameter, true);
            }
        }
        healthSystem.Damage(damage);

        if (enemyAnim != null)
        {
            if (!hasDamageAnimation)
            {
                enemyAnim.SetBool(damageParameter, false);
            }
        }
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
        if(isAlive)
        {
            isAlive = false;
            pathfinding.canMove = false;

            if (enemyAnim != null)
            {
                if (!hasDeathAnimation)
                {
                    enemyAnim.SetBool(deathParameter, true);
                }
            }

            if (LootManager.instance != null)
            {
                LootManager.instance.GetStandardDrops(transform.position);
            }
            if (WaveManager.instance != null)
            {
                WaveManager.instance.EnemyDefeated(gameObject);
            }

            pathfinding.GetAgent().ResetPath();

            Destroy(gameObject, deathAnimLength);
        }
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

    public float GetAttackSpeed()
    {
        return attackTime = attackSpeed;
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetAttackPosition(), attackRange);

        if(pathfinding.GetTarget() != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, pathfinding.GetTargetPosition());
            Gizmos.DrawWireSphere(pathfinding.GetTargetPosition(), .75f);
        }
    }
}
