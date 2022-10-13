using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public bool autoStart = true;
    private bool hasBoss = false;
    [SerializeField] private WaveDetails[] waveDetails;
    public int waveIndex = 0;

    public int nextLevelIndex = 0;

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
    [SerializeField] private UnityEvent bossDefeated;

    [SerializeField] private ShufflebagItem<SpawnPointInfo>[] spawnPoints;

    public GameObject boss;
    public Transform bossSpawn;
    private GameObject currentBoss;

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

    public void SpawnWave(WaveDetails wave)
    {
        List<SpawnPointInfo> activeSpawnpoints = new List<SpawnPointInfo>();

        for (int i = 0; i < wave.nbrOfSpawnpoints; i++)
        {
            activeSpawnpoints.Add(GalaxyRandom.GetValueFromShufflebag(spawnPoints));
        }
        for (int i = 0; i < wave.nbrOfMinions; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(minionPrefabs), GalaxyRandom.GetRandomFromList(activeSpawnpoints).GetSpawnPosition(), Quaternion.identity);
        }
        for (int i = 0; i < wave.nbrOfSpecialEnemies; i++)
        {
            Instantiate(GalaxyRandom.GetRandomWeightedValue(specialEnemies), GalaxyRandom.GetRandomFromList(activeSpawnpoints).GetSpawnPosition(), Quaternion.identity);
        }

        waveSize = wave.nbrOfMinions + wave.nbrOfSpecialEnemies;
        enemiesLeft = waveSize;
        totalEnemies += waveSize;

        if (OnWaveSpawned != null) OnWaveSpawned(this, new WaveSpawnedEventArgs(waveSize, GalaxyRandom.ConvertToArray(activeSpawnpoints)));
    }

    public void SpawnIndex(int index)
    {
        SpawnWave(waveDetails[index]);
    }


    public void SpawnBoss()
    {
        currentBoss = Instantiate(boss, bossSpawn.position, Quaternion.identity);
        hasBoss = true;
    }

    public void EnemyDefeated(GameObject enemy)
    {
        if(currentBoss == enemy)
        {
            bossDefeated.Invoke();

            if (PlayerData.instance != null)
            {
                PlayerData.instance.LevelEnded(nextLevelIndex);
            }
        }
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
                    if (allWavesCompleted != null && !hasBoss)
                    {
                        allWavesCompleted.Invoke();

                        if (PlayerData.instance != null)
                        {
                            PlayerData.instance.LevelEnded(nextLevelIndex);
                        }
                    }

                    if (allWavesCompleted != null && hasBoss && currentBoss == null)
                    {
                        //bossDefeated.Invoke();

                        if (PlayerData.instance != null)
                        {
                            PlayerData.instance.LevelEnded(nextLevelIndex);
                        }
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

