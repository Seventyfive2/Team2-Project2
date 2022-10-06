using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMoving : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    [SerializeField] private float cartSpeed = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       transform.position = Vector3.MoveTowards(transform.position, endPoint.position, cartSpeed * Time.deltaTime);
    }
}
