using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public event EventHandler<WaveSpawnedEventArgs> OnWaveSpawned;

    public class WaveSpawnedEventArgs : EventArgs
    {
        public int waveSize;
        public SpawnPointInfo[] spawnPoints;

        public WaveSpawnedEventArgs(int enemyCount, SpawnPointInfo[] points)
        {
            waveSize = enemyCount;
            spawnPoints = points;
        }
    }

    [SerializeField] private ShufflebagItem<Transform>[] spawnPoints;

    [SerializeField] private float spawnTime;

    [SerializeField] private GameObject minionPrefab;

    [SerializeField] private WeightedItem<GameObject>[] specialEnemies;

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

    private void Start()
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
        SpawnPointInfo[] spawnpointInfo = new SpawnPointInfo[nbrOfSpawnpoints];

        for (int i = 0; i < nbrOfSpawnpoints; i++)
        {
            activeSpawnpoints.Add(GalaxyRandom.GetValueFromShufflebag(spawnPoints));
            spawnpointInfo[i] = activeSpawnpoints[i].GetComponent<SpawnPointInfo>();
        }
        for (int i = 0; i < nbrOfMinions; i++)
        {
            Instantiate(minionPrefab, GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }
        for (int i = 0; i < nbrOfSpecialEnemies; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(specialEnemies), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }

        if (OnWaveSpawned != null) OnWaveSpawned(this, new WaveSpawnedEventArgs(nbrOfMinions+nbrOfSpecialEnemies, spawnpointInfo));
    }
}
