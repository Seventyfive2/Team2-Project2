using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndMenu : MonoBehaviour
{
    public GameObject endScreen;

    public void LoadNextLevel()
    {
        PlayerData.instance.LoadNextLevel();
    }
}
