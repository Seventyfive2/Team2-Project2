using UnityEngine;

public class AggroSystem : MonoBehaviour
{
    [SerializeField] int threatLevel;
    public int priorityIncrease = 10;
    public bool isBuilding = false;

    public int GetThreatLevel(bool prioritize = false)
    {
        if(prioritize)
        {
            return threatLevel + priorityIncrease;
        }
        return threatLevel;
    }
}
