using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenChest : MonoBehaviour, IDamagable
{
    public GameObject Chest;

    public Transform dropLocation;

    [SerializeField] private int minCoinsToDrop = 10;
    [SerializeField] private int maxCoinsToDrop = 10;
    
    [SerializeField] private int minItemsToDrop = 1;
    [SerializeField] private int maxItemsToDrop = 1;

    public void Heal(int amt)
    {
        
    }

    public void TakeDamage(int damage)
    {
        int c = Random.Range(minCoinsToDrop, maxCoinsToDrop);
        int i = Random.Range(minItemsToDrop, maxItemsToDrop);

        LootManager.instance.GetTreasureDrops(dropLocation.position, c,i);

        Chest.SetActive(false);
    }
}
