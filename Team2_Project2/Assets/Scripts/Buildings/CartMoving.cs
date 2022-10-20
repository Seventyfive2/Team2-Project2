using UnityEngine;
using UnityEngine.AI;

public class CartMoving : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float cartSpeed = 10;

    private bool canMove = true;

    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private float damageWait = .5f;
    private float waitTime;

    public Transform checkpos;
    public float checkRange;
    public LayerMask layerMask;

    void FixedUpdate()
    {
        if(Physics.CheckSphere(checkpos.position,checkRange, layerMask))
        {
            canMove = false;
        }


        if(Vector3.Distance(transform.position,endPoint.position) > stopDistance && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, cartSpeed * Time.deltaTime);
        }

        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            canMove = true;
        }
    }

    public void StopMoving()
    {
        waitTime = damageWait;
        canMove = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(checkpos.position, checkRange);
    }
}
