using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Item")]
public class ItemSO : ScriptableObject
{
    public int cost = 20;
    //public float cooldown = 10;
    public Sprite uiImage;
}
