using UnityEngine;
using UnityEngine.Events;

public class ColliderConnector : MonoBehaviour, IDamagable
{
    public UnityEvent<Collision> collisionEvent;
    public UnityEvent<Collider> triggerEvent;

    public GameObject connector = null;
    private IDamagable damagable;

    public void Start()
    {
        damagable = connector.GetComponent<IDamagable>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collisionEvent != null)
        {
            collisionEvent.Invoke(collision);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(triggerEvent != null)
        {
            triggerEvent.Invoke(other);
        }
    }

    public void Heal(int amt)
    {
        if(damagable != null)
        {
            damagable.Heal(amt);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
        }
    }
}
