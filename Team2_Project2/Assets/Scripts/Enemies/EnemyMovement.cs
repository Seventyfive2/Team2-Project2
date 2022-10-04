using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    private BaseEnemy enemy;

    void Start()
    {
        enemy = GetComponent<BaseEnemy>();
    }

    void Update()
    {
        if(enemy.GetEnemyState() == BaseEnemy.State.Moving && target != null)
        {
            agent.destination = GetRandomPositionAround(target.position, 1f);
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

    public NavMeshAgent GetAgent()
    {
        return agent;
    }
}
