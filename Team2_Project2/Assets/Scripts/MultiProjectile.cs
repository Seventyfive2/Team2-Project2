using UnityEngine;

public class MultiProjectile : Projectile
{
    [SerializeField] private Projectile[] projectiles;

    private void Start()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].gameObject.SetActive(true);
            projectiles[i].Setup(tag, damage);
            projectiles[i].transform.parent = null;
        }

        Destroy(gameObject);
    }
}
