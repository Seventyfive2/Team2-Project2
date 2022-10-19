using UnityEngine;
using UnityEngine.AI;

public class CartMoving : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float cartSpeed = 10;
    void Start()
    {
        agent.destination = endPoint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       //transform.position = Vector3.MoveTowards(transform.position, endPoint.position, cartSpeed * Time.deltaTime);


    }
}
