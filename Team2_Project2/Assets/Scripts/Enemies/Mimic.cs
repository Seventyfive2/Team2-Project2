using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : BaseEnemy
{
    private float defaultAttackRange;
    [Header("Mimic Stats")]
    [SerializeField] private float spawnAttackRange = 3f;
    [SerializeField] private float spawnArea = 2f;
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private LayerMask layersToBlockClipping;
    [SerializeField] private GameObject enemyEgg;
    [Space]
    [SerializeField] private float teleportAttackRange = 10f;
    [SerializeField] private float teleportMaxDistance = 7f;
    [SerializeField] private float teleportMinDistance = 3f;

    private int attackIndex = 0;

    public GameObject endScreen;

    public void Start()
    {
        defaultAttackRange = attackRange;

        endScreen = GameObject.Find("Menu Canvas");
    }

    public override void Attack()
    {
        switch (attackIndex)
        {
            case 0:
            default:
                //Bite
                base.Attack();
                attackRange = spawnAttackRange;
                break;
            case 1:
                //Spawn
                for (int i = 0; i < spawnCount; i++)
                {
                    GameObject generatedObject;

                    //Gets random transform values
                    Vector3 randomPosition = GetRandomPositionAround(transform.position + (Vector3.up), 2f);
                    //randomPosition += transform.position;
                    Quaternion randomRotation =  Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

                    //Checks if object is at spawn loaction
                    if (!Physics.CheckBox(randomPosition, Vector3.one, Quaternion.identity, layersToBlockClipping))
                    {
                        generatedObject = Instantiate(enemyEgg, transform.position, Quaternion.identity);

                        generatedObject.GetComponent<LobbedProjectile>().endingPosition = randomPosition;
                        generatedObject.GetComponent<ISetup>().Setup(tag,0,1,1,true);
                        //generatedObject = Instantiate(GalaxyRandom.GetRandomWeightedValue(eneimes), randomPosition, randomRotation);
                    }

                }
                GetAttackSpeed();
                attackRange = teleportAttackRange;
                break;
            case 2:
                //Teleport
                transform.position = GetRandomPositionAround(pathfinding.GetTargetPosition(), teleportMaxDistance, teleportMinDistance);
                GetAttackSpeed();
                attackRange = defaultAttackRange;
                break;
        }

        attackIndex++;
        Debug.Log(attackIndex);
        if(attackIndex >= 3)
        {
            attackIndex = 0;
        }
    }

    public Vector3 GetRandomPositionAround(Vector3 center, float range, float minRange = 0)
    {
        float xOffset = Random.Range(-range, range);
        float zOffset = Random.Range(-range, range);

        if(xOffset > 0)
        {
            xOffset += minRange;
        }
        else
        {
            xOffset -= minRange;
        }

        if (zOffset > 0)
        {
            zOffset += minRange;
        }
        else
        {
            zOffset -= minRange;
        }

        return new Vector3(center.x + xOffset, center.y, center.z + zOffset);
    }

    public override void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        base.HealthSystem_OnDeath(sender, e);
        endScreen.SetActive(true);
    }
}
