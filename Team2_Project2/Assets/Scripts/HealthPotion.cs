using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, ICollectable
{
    [SerializeField] private int healAmt;

    public void Collected(GameObject collector)
    {
        Destroy(gameObject);
        if(collector.GetComponent<IDamagable>() != null)
        {
            collector.GetComponent<IDamagable>().Heal(healAmt);
        }
    }

    public void Respawn(GameObject collector)
    {

    }
}
