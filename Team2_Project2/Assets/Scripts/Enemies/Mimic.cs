using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : BaseEnemy
{
    [Header("Mimic Stats")]
    [SerializeField] private float tacticChangeTime = 1.5f;
    //[SerializeField] private GameObject enemySpawn;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private LayerMask layersToBlockClipping;
    [SerializeField] private List<WeightedItem<GameObject>> eneimes;

    private int attackIndex = 0;

    public void Start()
    {
        StartCoroutine(ChangeTactic());
    }

    public override void Attack()
    {
        switch (attackIndex)
        {
            case 0:
            default:
                //Bite
                base.Attack();
                break;
            case 1:
                //Spawn
                for (int i = 0; i < spawnCount; i++)
                {
                    GameObject generatedObject;

                    //Gets random transform values
                    Vector3 randomPosition = GetRandomPositionAround(transform.position - (Vector3.up), 2f);
                    //randomPosition += transform.position;
                    //Vector3 randomRotation = new Vector3(0, Random.Range(0, rotationRange), 0);

                    //Checks if object is at spawn loaction
                    if (!Physics.CheckBox(randomPosition, Vector3.one, Quaternion.identity, layersToBlockClipping))
                    {
                        generatedObject = Instantiate(GalaxyRandom.GetRandomWeightedValue(eneimes), randomPosition, Quaternion.identity);
                    }

                }
                GetAttackSpeed();
                break;
            case 2:
                //Teleport
                transform.position = GetRandomPositionAround(pathfinding.GetTargetPosition(), 5f);
                GetAttackSpeed();
                break;
        }

        attackIndex++;
        Debug.Log(attackIndex);
        if(attackIndex >= 3)
        {
            attackIndex = 0;
        }
    }

    IEnumerator ChangeTactic()
    {
        while (isAlive)
        {
            

            yield return new WaitForSeconds(tacticChangeTime);
        }
    }

    public Vector3 GetRandomPositionAround(Vector3 center, float range)
    {
        float xOffset = Random.Range(-range, range);
        float zOffset = Random.Range(-range, range);

        return new Vector3(center.x + xOffset, center.y, center.z + zOffset);
    }
}
