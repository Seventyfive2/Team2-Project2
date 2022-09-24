using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private int damage;

    public void Setup(int projectileDamage)
    {
        damage = projectileDamage;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable target = other.GetComponent<IDamagable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
