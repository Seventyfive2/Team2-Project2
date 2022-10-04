using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : MonoBehaviour, ICollectable
{
    public void Collected(GameObject collector)
    {
        Destroy(gameObject);
    }

    public void Respawn(GameObject collector)
    {
        throw new System.NotImplementedException();
    }
}
