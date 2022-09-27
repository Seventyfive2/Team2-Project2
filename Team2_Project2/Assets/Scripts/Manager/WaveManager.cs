using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private WaveDetails[] waveDetails;
    private int waveIndex = 0;

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

    [SerializeField] private UnityEvent wavesCompleted;

    [SerializeField] private ShufflebagItem<Transform>[] spawnPoints;

    [SerializeField] private GameObject[] minionPrefabs;

    [SerializeField] private WeightedItem<GameObject>[] specialEnemies;

    [SerializeField] private int waveSize;
    [SerializeField] private int enemiesLeft;

    [Range(0,1)][SerializeField] private float wavePercentage = .75f;


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
        SpawnWave(waveDetails[waveIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnWave(WaveDetails wave)
    {
        List<Transform> activeSpawnpoints = new List<Transform>();
        SpawnPointInfo[] spawnpointInfo = new SpawnPointInfo[wave.nbrOfSpawnpoints];

        for (int i = 0; i < wave.nbrOfSpawnpoints; i++)
        {
            activeSpawnpoints.Add(GalaxyRandom.GetValueFromShufflebag(spawnPoints));
            spawnpointInfo[i] = activeSpawnpoints[i].GetComponent<SpawnPointInfo>();
        }
        for (int i = 0; i < wave.nbrOfMinions; i++)
        {
            Instantiate(GalaxyRandom.GetRandomFromList(minionPrefabs), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }
        for (int i = 0; i < wave.nbrOfSpecialEnemies; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(specialEnemies), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }

        waveSize = wave.nbrOfMinions + wave.nbrOfSpecialEnemies;
        enemiesLeft = waveSize;

        if (OnWaveSpawned != null) OnWaveSpawned(this, new WaveSpawnedEventArgs(waveSize, spawnpointInfo));
    }

    public void EnemyDefeated()
    {
        enemiesLeft--;

        if((float)enemiesLeft / waveSize <= 1 - wavePercentage)
        {
            if(waveIndex < waveDetails.Length)
            {
                SpawnWave(waveDetails[waveIndex]);
                waveIndex++;
            }
            else
            {
                if(wavesCompleted != null)
                {
                    wavesCompleted.Invoke();
                }
            }
        }
    }
}

[Serializable]
public struct WaveDetails
{
    public int nbrOfSpawnpoints;
    public int nbrOfMinions;
    public int nbrOfSpecialEnemies;

    public WaveDetails(int nbrOfSpawnpoints, int nbrOfMinions, int nbrOfSpecialEnemies)
    {
        this.nbrOfSpawnpoints = nbrOfSpawnpoints;
        this.nbrOfMinions = nbrOfMinions;
        this.nbrOfSpecialEnemies = nbrOfSpecialEnemies;
    }
}

