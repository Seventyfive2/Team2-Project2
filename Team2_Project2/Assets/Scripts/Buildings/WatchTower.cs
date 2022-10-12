using System;
using UnityEngine;

public class WatchTower : BaseBuilding
{
    private PlayerUI playerUI;
    public override void Start()
    {
        base.Start();
        playerUI = GameObject.Find("Player Canvas").GetComponent<PlayerUI>();
        if (WaveManager.instance != null)
        {
            WaveManager.instance.OnWaveSpawned += WaveManager_OnWaveSpawned;
        }
    }

    private void WaveManager_OnWaveSpawned(object sender, WaveManager.WaveSpawnedEventArgs e)
    {
        if(!isDestroyed)
        {
            string spawnNotification = "Enemies coming from ";

            for (int i = 0; i < e.spawnPoints.Length; i++)
            {
                if(i == 0)
                {
                    spawnNotification = spawnNotification + e.spawnPoints[i].GetSpawnName();
                }
                else if(i == e.spawnPoints.Length-1)
                {
                    spawnNotification = spawnNotification + ", and " +e.spawnPoints[i].GetSpawnName();
                }
                else
                {
                    spawnNotification = spawnNotification + ", " + e.spawnPoints[i].GetSpawnName();
                }
            }

            playerUI.TriggerNotification(spawnNotification);
            Debug.Log(spawnNotification);
        }    
    }
}
