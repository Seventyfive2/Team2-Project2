using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private ShufflebagItem<Transform>[] spawnPoints;

    [SerializeField] private float spawnTime;

    [SerializeField] private GameObject minionPrefab;

    [SerializeField] private WeightedItem<GameObject>[] specialEnemies;

    void Start()
    {
        SpawnWave(2, 15, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnWave(int nbrOfSpawnpoints, int nbrOfMinions, int nbrOfSpecialEnemies)
    {
        List<Transform> activeSpawnpoints = new List<Transform>();

        for (int i = 0; i < nbrOfSpawnpoints; i++)
        {
            activeSpawnpoints.Add(GalaxyRandom.GetValueFromShufflebag(spawnPoints));
        }
        for (int i = 0; i < nbrOfMinions; i++)
        {
            Instantiate(minionPrefab, GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }
        for (int i = 0; i < nbrOfSpecialEnemies; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(specialEnemies), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }
    }
}
