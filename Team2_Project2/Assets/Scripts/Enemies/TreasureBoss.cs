using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoss : TreasureMob
{
    [SerializeField] private float tacticChangeTime = 1.5f;

    [Header("Phase 2")]
    [SerializeField] private GameObject secondPhase;


    public void Start()
    {
        StartCoroutine(ChangeTactic());
    }

    IEnumerator ChangeTactic()
    {
        while(isAlive)
        {
            int r = Random.Range(0, 2);

            if (r == 0)
            {
                pathfinding.runFromPlayer = true;
            }
            else
            {
                pathfinding.runFromPlayer = false;
            }

            yield return new WaitForSeconds(tacticChangeTime);
        }
    }

    public override void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        Instantiate(secondPhase, transform.position, Quaternion.identity);
        base.HealthSystem_OnDeath(sender, e);
    }
}
