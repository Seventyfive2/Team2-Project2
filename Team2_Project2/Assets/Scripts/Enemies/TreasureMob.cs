using System;
using System.Collections;
using UnityEngine;

public class TreasureMob : BaseEnemy
{
    [SerializeField] private int nbrOfCoins = 10;
    [SerializeField] private int nbrOfItems = 3;

    public override void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LootManager.instance.GetTreasureDrops(transform.position, nbrOfCoins, nbrOfItems);
        base.HealthSystem_OnDeath(sender, e);
    }

    public override IEnumerator GetTarget()
    {
        pathfinding.SetTarget(GameObject.Find("Player").transform);

        yield return new WaitForSeconds(10);
    }
}
