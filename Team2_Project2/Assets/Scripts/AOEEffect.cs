using UnityEngine;

public class AOEEffect : MonoBehaviour, ISetup
{
    [HideInInspector] public int damage;
    [SerializeField] private float range = 2.5f;

    public void Activate()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, range);
        if (targets.Length != 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].transform.GetComponent<IDamagable>() != null && !targets[i].CompareTag(tag))
                {
                    targets[i].transform.GetComponent<IDamagable>().TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    public void Setup(string tag, int damage, float range = 2.5f, float duration = 1, bool activate = false)
    {
        this.tag = tag;
        this.damage = damage;
        this.range = range;

        if(activate)
        {
            Activate();
        }    
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
