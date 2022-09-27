using System.Collections;
using UnityEngine;

public class HealingBuilding : BaseBuilding
{
    [Header("Heal Effect")]
    [SerializeField] private float healRate = 2f;
    [SerializeField] private int healAmt = 5;
    [SerializeField] private float healRange = 2f;

    public override void Start()
    {
        base.Start();
        StartCoroutine(HealEffect());
    }

    IEnumerator HealEffect()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, healRange);

            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].CompareTag("Player"))
                {
                    colliders[i].GetComponent<IDamagable>().Heal(healAmt);
                }
            }

            yield return new WaitForSeconds(healRate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
