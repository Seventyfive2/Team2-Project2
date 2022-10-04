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

    void OnEnable()
    {
        time = lifeTime;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, time);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
