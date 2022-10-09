using UnityEngine;

public class SpawnPointInfo : MonoBehaviour
{
    [SerializeField] private string spawnPointName = "Unnamed spawn";
    [SerializeField] private Transform[] subSpawnPoints; 

    public string GetSpawnName()
    {
        return spawnPointName;
    }

    public Transform GetSpawnTransform()
    {
        if(subSpawnPoints.Length != 0)
        {
            return GalaxyRandom.GetRandomFromList(subSpawnPoints);
        }

        return transform;
    }
    public Vector3 GetSpawnPosition()
    {
        if (subSpawnPoints.Length != 0)
        {
            return GalaxyRandom.GetRandomFromList(subSpawnPoints).position;
        }

        return transform.position;
    }
}
