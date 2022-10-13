using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    private int enemiesKilledSinceGoldDrop;

    [SerializeField] private GameObject coin;
    [SerializeField] private WeightedItem<GameObject>[] enemyDrop;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    public void GetStandardDrops(Vector3 deathPosition)
    {
        GetGoldDrop(deathPosition);
        GetItemDrop(deathPosition);
    }

    public void GetTreasureDrops(Vector3 deathPosition, int nbrOfCoins, int nbrOfItems)
    {
        GetMultipleGoldDrops(deathPosition,nbrOfCoins);
        for (int i = 0; i < nbrOfItems + 1; i++)
        {
            GetItemDrop(deathPosition,true);
        }
    }

    public void GetMultipleGoldDrops(Vector3 deathPosition, int amt)
    {
        for (int i = 0; i < amt+1; i++)
        {
            Vector3 dropPosition = GetItemDropPosition(deathPosition, 1.5f);

            Instantiate(coin, dropPosition, Quaternion.identity);
        }
    }

    public void GetGoldDrop(Vector3 deathPosition)
    {
        enemiesKilledSinceGoldDrop++;

        int randomNumber = Random.Range(0, 101);

        Vector3 dropPosition = GetItemDropPosition(deathPosition, 1.5f);

        if (randomNumber <= 10 * enemiesKilledSinceGoldDrop)
        {
            Instantiate(coin, dropPosition, Quaternion.identity);
            enemiesKilledSinceGoldDrop = 0;
        }
    }

    public void GetItemDrop(Vector3 deathPosition, bool forceDrop = false)
    {
        GameObject drop = GalaxyRandom.GetRandomWeightedValue(enemyDrop);

        Vector3 dropPosition = GetItemDropPosition(deathPosition, 1.5f);

        if (drop != null)
        {
            Instantiate(drop, dropPosition, Quaternion.identity);
        }
        else if(forceDrop)
        {
            while (drop == null)
            {
                drop = GalaxyRandom.GetRandomWeightedValue(enemyDrop);
            }
        }
    }

    public Vector3 GetItemDropPosition(Vector3 pos, float range, float yOffset = -1f)
    {
        float xOffset = Random.Range(-range, range);
        float zOffset = Random.Range(-range, range);

        Vector3 result = new Vector3(pos.x + xOffset, pos.y + yOffset, pos.z + zOffset);

        return result;
    }
}
