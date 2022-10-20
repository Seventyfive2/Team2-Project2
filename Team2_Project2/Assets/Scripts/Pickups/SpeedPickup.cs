using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour, ICollectable
{
    public float speedInc;
    public float speedTime;

    public void Collected(GameObject collector)
    {
        if (collector.GetComponent<PlayerMovement>() != null)
        {
            collector.GetComponent<PlayerMovement>().SpeedBoost(speedInc,speedTime);
        }

        Destroy(gameObject);
    }

    public void Respawn(GameObject collector)
    {
        throw new System.NotImplementedException();
    }
}
