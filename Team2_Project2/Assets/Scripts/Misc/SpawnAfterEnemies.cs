using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnAfterEnemies : MonoBehaviour
{
    public bool go = true;

    public void Start()
    {
        StartCoroutine(CheckEnemies());
    }

    public IEnumerator CheckEnemies()
    {
        while(go)
        {
            bool allDead = true;

            if(transform.childCount > 0)
            {
                allDead = false;
            }

            if(allDead)
            {
                WaveManager.instance.autoStart = true;
                WaveManager.instance.SpawnIndex(0);

                go = true;
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
