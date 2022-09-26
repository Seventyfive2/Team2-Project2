using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private enum TagToIgnore { Player, Enemies }
    [SerializeField] private TagToIgnore tagToIgnore = TagToIgnore.Enemies;

    public void Setup(int projectileDamage)
    {
        damage = projectileDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(tagToIgnore.ToString()))
        {
            IDamagable target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
