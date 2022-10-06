using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;


    public bool autoStart = true;
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

    [SerializeField] private UnityEvent allWavesCompleted;

    [SerializeField] private ShufflebagItem<Transform>[] spawnPoints;

    public GameObject boss;
    public Transform bossSpawn;

    [SerializeField] private WeightedItem<GameObject>[] minionPrefabs;

    [SerializeField] private WeightedItem<GameObject>[] specialEnemies;

    [SerializeField] private int waveSize;
    [SerializeField] private int enemiesLeft;
    [SerializeField] private int totalEnemies;

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
        if(autoStart)
        {
            SpawnWave(waveDetails[waveIndex]);
            waveIndex++;
        }
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
            Instantiate(GalaxyRandom.GetRandomWeightedValue(minionPrefabs), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }
        for (int i = 0; i < wave.nbrOfSpecialEnemies; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(specialEnemies), GalaxyRandom.GetRandomFromList(activeSpawnpoints).position, Quaternion.identity);
        }

        waveSize = wave.nbrOfMinions + wave.nbrOfSpecialEnemies;
        enemiesLeft = waveSize;
        totalEnemies += waveSize;

        if (OnWaveSpawned != null) OnWaveSpawned(this, new WaveSpawnedEventArgs(waveSize, spawnpointInfo));
    }

    public void SpawnIndex(int index)
    {
        SpawnWave(waveDetails[index]);
    }


    public void SpawnBoss()
    {
        Instantiate(boss, bossSpawn.position, Quaternion.identity);
    }

    public void EnemyDefeated()
    {
        enemiesLeft--;
        totalEnemies--;

        if((float)enemiesLeft / waveSize <= 1 - wavePercentage)
        {
            if(waveIndex < waveDetails.Length && autoStart)
            {
                SpawnWave(waveDetails[waveIndex]);
                waveIndex++;
            }
            else
            {
                if(totalEnemies <= 0)
                {
                    if (allWavesCompleted != null)
                    {
                        allWavesCompleted.Invoke();
                    }

                    if (PlayerData.instance != null)
                    {
                        PlayerData.instance.LevelEnded();
                    }
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

