using UnityEngine;

public class ObjectsWithDuration : MonoBehaviour, ISetup
{
    [SerializeField] private float objectDuration = 1f;

    public virtual void Setup(string tag, int damage, float range = 1, float duration = 1, bool activate = false)
    {
        objectDuration = duration;
        if(activate)
        {
            Activate();
        }
    }

    public void Activate()
    {
        Destroy(gameObject, objectDuration);
    }
}
