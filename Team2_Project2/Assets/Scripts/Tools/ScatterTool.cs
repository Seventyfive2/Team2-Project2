using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterTool : MonoBehaviour
{
    [SerializeField] private ScatteredObject[] scatterObjects;
    private List<GameObject> instancedObjects = new List<GameObject>();

    private readonly int objectAmountHardCap = 5000;
    [Tooltip("Can not be less than 2 or greater than 5000")][SerializeField] private int maxNumberOfObjects = 20;

    [Header("Transform Values")]
    [SerializeField] private Vector2 range = Vector2.one * 10;
    [SerializeField] private float yOffset = -1;
    [Range(0,360)][SerializeField] private float rotationRange = 360;
    [SerializeField] private float minScale = .8f;
    [SerializeField] private float maxScale = 1.25f;

    [Header("Settings")]
    [SerializeField] private LayerMask layersToBlockClipping;
    [Tooltip("Generates on scene load.")][SerializeField] private bool generateOnStart = false;
    [SerializeField] private bool useWeights = false;
    [SerializeField] private bool cluster = false;
    [SerializeField] private bool snapToGround = false;
    [Space]
    [SerializeField] private bool showDebugSettings = false;

    [Header("Extra")]
    [ConditionalValue("cluster")][SerializeField] private float clusterDensity = 1.8f;

    [Header("Debug Settings")]
    [ConditionalValue("showDebugSettings")][SerializeField] private bool showConsoleMessages = true;
    [ConditionalValue("showDebugSettings")][SerializeField] private Color debugColor = Color.red;

    [Space]
    [Tooltip("Prevents generation, so user won't re-generate on accident.")][SerializeField] private bool lockCurrentLayout = false;

    public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!instancedObjects.Contains(transform.GetChild(i).gameObject))
            {
                instancedObjects.Add(transform.GetChild(i).gameObject);
            }
        }

        if(generateOnStart)
        {
            Generate();
        }

        if (showDebugSettings)
        {
            //Here to stop warning message
        }
    }

    public void Generate()
    {
        int rerolls = 0;
        int spawnCount = Mathf.Clamp(maxNumberOfObjects, 2, objectAmountHardCap);
        if(lockCurrentLayout)
        {
            Debug.LogWarning(name + " is locked, Check Inspector.");
        }
        if (scatterObjects.Length != 0 && !lockCurrentLayout)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!instancedObjects.Contains(transform.GetChild(i).gameObject))
                {
                    instancedObjects.Add(transform.GetChild(i).gameObject);
                }
            }
            for (int i = instancedObjects.Count-1; i > -1; i--)
            {
                DestroyImmediate(instancedObjects[i]);
            }
            instancedObjects.Clear();

            float xDistFromCenter = range.x / 2;
            float zDistFromCenter = range.y / 2;

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject generatedObject;

                //Gets random transform values
                Vector3 randomPosition = GetRandomVector3(-xDistFromCenter, xDistFromCenter, yOffset, yOffset, -zDistFromCenter, zDistFromCenter);
                Vector3 randomRotation = new Vector3(0, Random.Range(0, rotationRange), 0);
                Vector3 randomScale = Vector3.one * Random.Range(minScale, maxScale);

                ScatteredObject chooseObject = GetObjectToSpawn();

                //Checks if object is at spawn loaction
                if (!Physics.CheckBox(randomPosition, randomScale, Quaternion.Euler(randomRotation), layersToBlockClipping))
                {
                    generatedObject = Instantiate(chooseObject.objectToSpawn, randomPosition, Quaternion.Euler(randomRotation), transform);

                    generatedObject.transform.localScale = randomScale;

                    //Moves the spawned object to the ground
                    if(snapToGround)
                    {
                        Ray ray = new Ray(generatedObject.transform.position, generatedObject.transform.localScale/2);
                        RaycastHit hitInfo;

                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            generatedObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y+randomScale.y/2, hitInfo.point.z);
                        }
                    }

                    rerolls = 0;
                    instancedObjects.Add(generatedObject);
                }
                else
                {
                    //Prevents infinite loops by breaking the loop after multiple failed spawns
                    i--;
                    rerolls++;
                    if(rerolls >= 10)
                    {
                        if (showConsoleMessages) { Debug.Log("Rerolled " + rerolls + " breaking loop"); }
                        break;
                    }
                    if (showConsoleMessages) { Debug.Log("Space occuiped rerolling"); }
                }
            }
        }
    }

    public ScatteredObject GetObjectToSpawn()
    {
        if (useWeights)
        {
            return GetRandomObject(scatterObjects);
        }
        else
        {
            return GetRandomFromList(scatterObjects);
        }
    }

    #region Randomizer
    public Vector3 GetRandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        Vector3 result;
        float adjust = 1;

        if(cluster)
        {
            adjust = Random.Range(1, clusterDensity);
        }

        result = new Vector3(Random.Range(xMin, xMax) / adjust, Random.Range(yMin, yMax), Random.Range(zMin, zMax) / adjust);

        return result;
    }

    public ScatteredObject GetRandomObject(ScatteredObject[] objectList)
    {
        ScatteredObject output = default;

        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in objectList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in objectList)
        {
            processedWeight += entry.weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry;
                break;
            }
        }

        return output;
    }
    public T GetRandomFromList<T>(T[] list)
    {
        T output = default;

        int index = Random.Range(0, list.Length);

        output = list[index];

        return output;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireCube(transform.position, new Vector3(range.x,1,range.y));
    }
}

[System.Serializable]
public class ScatteredObject
{
    public string name = "ObjectName";
    public GameObject objectToSpawn;
    [Tooltip("Only need if using weights. \nMust be greater than zero. ")][ConditionalValue("useWeights")] public int weight = 1;

    public ScatteredObject(int itemWeight, GameObject value)
    {
        weight = itemWeight;
        objectToSpawn = value;
    }

    public bool CanPlace()
    {
        return true;
    }
}
