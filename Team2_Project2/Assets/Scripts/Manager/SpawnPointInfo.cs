using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointInfo : MonoBehaviour
{
    [SerializeField] private string spawnPointName = "Unnamed spawn";

    public string GetSpawnName()
    {
        return spawnPointName;
    }
}
