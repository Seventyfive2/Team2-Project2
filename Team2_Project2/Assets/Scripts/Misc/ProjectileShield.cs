using UnityEngine;

public class ProjectileShield : ObjectsWithDuration
{
    private float size = 0;

    public override void Setup(string tag, int damage, float range = 1, float duration = 1, bool activate = false)
    {
        size = range;
        
        transform.localScale = new Vector3(size, size, size);

        base.Setup(tag, damage, range, duration, activate);
    }
}
