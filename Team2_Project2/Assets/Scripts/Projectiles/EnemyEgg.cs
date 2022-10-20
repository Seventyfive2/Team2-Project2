using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEgg : AOEEffect
{
    [SerializeField] private List<WeightedItem<GameObject>> eneimes;

    public override void Activate()
    {
        Instantiate(GalaxyRandom.GetRandomWeightedValue(eneimes), transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
