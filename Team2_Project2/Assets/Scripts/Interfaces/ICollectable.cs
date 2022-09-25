using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    public void Collected(GameObject collector);
    public void Respawn(GameObject collector);
}
