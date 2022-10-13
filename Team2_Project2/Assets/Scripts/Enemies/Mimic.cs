using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : BaseEnemy
{
    [SerializeField] private float tacticChangeTime = 1.5f;

    private int attackIndex = 0;

    public void Start()
    {
        StartCoroutine(ChangeTactic());
    }

    public override void Attack()
    {
        base.Attack();

        switch (attackIndex)
        {
            default:
                break;
        }

        attackIndex++;

        if(attackIndex <= 4)
        {
            attackIndex = 0;
        }

        GetAttackSpeed();
    }

    IEnumerator ChangeTactic()
    {
        while (isAlive)
        {
            

            yield return new WaitForSeconds(tacticChangeTime);
        }
    }
}
