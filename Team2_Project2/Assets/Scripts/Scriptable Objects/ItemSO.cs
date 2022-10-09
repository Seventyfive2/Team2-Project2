using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Item")]
public class ItemSO : ScriptableObject
{
    public int cost = 20;
    public int maxHeld = 5;
    public int damage = 2;
    public float range = 1;
    public Sprite uiImage;

    public GameObject gameObject;

    public enum ActivationMethod { UseOnPlayer, UseAtCursor }
    public ActivationMethod activationMethod;

    public bool doesDamage = false;

    public void UseItem(Vector3 pos, string tag)
    {
        GameObject itemPrefab = Instantiate(gameObject, pos, Quaternion.identity);
        if(doesDamage)
        {
            itemPrefab.GetComponent<ISetup>().Setup(tag, damage, range);
        }
    }
}
