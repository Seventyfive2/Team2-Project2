using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : MonoBehaviour
{
    [SerializeField] private Transform origin;
    public float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    private float time;

    public Rigidbody rb;

    public GameObject landposition;
    private GameObject landref;
    public LayerMask targetMask;

    void OnEnable()
    {
        time = lifeTime;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, time);

        if(landposition != null)
        {
            if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, targetMask))
            {
                //Debug.Log(hit.transform.position);
                landref = Instantiate(landposition,hit.point+ (Vector3.up * .1f),Quaternion.identity);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }

    public void OnDestroy()
    {
        Destroy(landref);
    }
}
