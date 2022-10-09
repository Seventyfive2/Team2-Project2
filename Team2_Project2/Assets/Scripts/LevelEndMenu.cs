using UnityEngine;

public class LevelEndMenu : MonoBehaviour
{
    public GameObject endScreen;

    public void LoadNextLevel(string sceneName)
    {
        PlayerData.instance.LoadNextLevel(sceneName);
    }
}
