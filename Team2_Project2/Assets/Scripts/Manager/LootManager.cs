using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void GetStandardDrops(Vector3 deathPosition)
    {
        GetGoldDrop(deathPosition);
        GetItemDrop(deathPosition);
    }

    public void GetGoldDrop(Vector3 deathPosition)
    {
        enemiesKilledSinceGoldDrop++;

        int randomNumber = Random.Range(0, 101);

        Vector3 dropPosition = new Vector3(deathPosition.x, deathPosition.y - 1, deathPosition.z);

        if (randomNumber <= 10 * enemiesKilledSinceGoldDrop)
        {
            Instantiate(coin, dropPosition, Quaternion.identity);
            enemiesKilledSinceGoldDrop = 0;
        }
    }

    public void GetItemDrop(Vector3 deathPosition)
    {
        GameObject drop = GalaxyRandom.GetRandomWeightedValue(enemyDrop);

        Vector3 dropPosition = new Vector3(deathPosition.x, deathPosition.y - 1, deathPosition.z);

        if (drop != null)
        {
            Instantiate(drop, dropPosition, Quaternion.identity);
        }
    }
}
