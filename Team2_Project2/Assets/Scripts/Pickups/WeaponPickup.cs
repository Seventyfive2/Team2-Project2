using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private WeaponSO weaponToGive;
    [SerializeField] private GameObject newPickup;

    [SerializeField] private BoxCollider itemTrigger;

    public void Start()
    {
        if(weaponToGive != null)
        {
            SetWeapon(weaponToGive);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public void SetWeapon(WeaponSO newWeapon)
    {
        weaponToGive = newWeapon;

        if(weaponToGive.weaponPrefab != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            Instantiate(weaponToGive.weaponPrefab, transform.position + Vector3.up, Quaternion.identity,transform).GetComponent<Collider>().enabled = false;

            name = "Weapon Pickup (" + weaponToGive.name + ")";


        }
    }

    public void Collected(GameObject collector)
    {
        Destroy(gameObject);

        if (collector.GetComponent<IDamagable>() != null)
        {
            collector.GetComponent<PlayerAttack>().ChangeWeapon(weaponToGive, out WeaponSO weaponToDrop);
            if(weaponToDrop != null)
            {
                WeaponPickup newPickupObject = Instantiate(newPickup, transform.position, Quaternion.identity).GetComponent<WeaponPickup>();
                newPickupObject.StartCoroutine(newPickupObject.DisableTrigger());
                newPickupObject.SetWeapon(weaponToDrop);
            }
        }
    }

    public void Respawn(GameObject collector)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator DisableTrigger()
    {
        itemTrigger.enabled = false;

        yield return new WaitForSeconds(1.5f);

        itemTrigger.enabled = true;

        yield return new WaitForSeconds(.5f);
    }
}
