using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private WeaponSO weaponToGive;

    public void Start()
    {
        if(weaponToGive != null)
        {
            SetWeapon(weaponToGive);
        }
    }

    public void SetWeapon(WeaponSO newWeapon)
    {
        weaponToGive = newWeapon;

        if(weaponToGive.weaponPrefab != null)
        {
            Destroy(transform.GetChild(0).gameObject);

            Instantiate(weaponToGive.weaponPrefab, transform.position + Vector3.up, Quaternion.identity,transform);
        }
    }

    public void Collected(GameObject collector)
    {
        Destroy(gameObject);
        if (collector.GetComponent<IDamagable>() != null)
        {
            collector.GetComponent<PlayerAttack>().ChangeWeapon(weaponToGive);
        }
    }

    public void Respawn(GameObject collector)
    {
        throw new System.NotImplementedException();
    }
}
