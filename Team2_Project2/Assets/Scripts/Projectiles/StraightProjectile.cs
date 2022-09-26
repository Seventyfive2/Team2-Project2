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

    void Start()
    {
        time = lifeTime;
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
