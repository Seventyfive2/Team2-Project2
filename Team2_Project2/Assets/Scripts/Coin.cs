using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collected(GameObject collector)
    {
        Destroy(gameObject);
    }

    public void Respawn(GameObject collector)
    {
        
    }
}
