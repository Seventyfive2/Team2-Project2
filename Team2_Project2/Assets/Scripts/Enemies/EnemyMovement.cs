using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private BaseEnemy enemy;
    public bool canMove = true;

    public bool runFromPlayer;
    [SerializeField] private float didtanceToRun = 4f;

    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
    }

    void Update()
    {
        if(enemy.GetEnemyState() == BaseEnemy.State.Moving && target != null && canMove)
        {
            if(!runFromPlayer)
            {
                agent.destination = GetRandomPositionAround(target.position, 1f);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if(distance < didtanceToRun)
                {
                    Vector3 dirToPlayer = transform.position - target.position;

                    Vector3 newPos = transform.position + dirToPlayer;

                    agent.destination = newPos;
                }
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Vector3 GetRandomPositionAround(Vector3 center, float range)
    {
        float xOffset = Random.Range(-range, range);
        float zOffset = Random.Range(-range, range);

        return new Vector3(center.x + xOffset, center.y, center.z + zOffset);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public Vector3 GetTargetPosition()
    {
        return target.position;
    }

    public Vector3 GetAgentDestination()
    {
        return agent.destination;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
