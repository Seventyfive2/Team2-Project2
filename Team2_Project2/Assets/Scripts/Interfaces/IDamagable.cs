using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(int damage);

    public void Heal(int amt);
}
