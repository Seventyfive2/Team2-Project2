using UnityEngine;
using static UnityEditor.PlayerSettings;

[CreateAssetMenu(menuName = "Scriptable Objects/New Ability")]
public class AbilitySO : ScriptableObject
{
    public int cost = 20;
    public float cooldown = 10;
    public Sprite cooldownImage;

    public enum HitDetectionStyle { FrontOfPlayer, AroundPlayer, AtCursor }

    public HitDetectionStyle hitDetectionStyle = HitDetectionStyle.AroundPlayer;
    public int damage = 10;
    public float range = 2;
    public float duration = 5;
    public GameObject projectile;
    public bool doesDamage = true;

    public void UseAbility(Vector3 pos, string tag, int damage)
    {
        GameObject itemPrefab = Instantiate(projectile, pos, Quaternion.identity);
        if (doesDamage)
        {
            itemPrefab.GetComponent<ISetup>().Setup(tag, damage, range, true);
        }
    }
}
