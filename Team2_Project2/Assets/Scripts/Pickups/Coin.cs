using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private int coinValue = 1;

    public void Collected(GameObject collector)
    {
        PlayerData.instance.coins += coinValue;
        GameObject.Find("Player Canvas").GetComponent<PlayerUI>().UpdateCoins();
        Destroy(gameObject);
    }

    public void Respawn(GameObject collector)
    {
        
    }
}
