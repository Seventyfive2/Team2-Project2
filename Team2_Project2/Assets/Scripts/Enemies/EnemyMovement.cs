using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    private bool targetInRange = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(!targetInRange && target != null)
        {
            agent.destination = GetRandomPositionAround(target.position,.5f);
        }
        else
        {
            //Attack State
        }

        /*
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }
        */
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
}
