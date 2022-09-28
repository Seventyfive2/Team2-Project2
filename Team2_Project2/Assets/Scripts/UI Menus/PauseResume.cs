using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResume : MonoBehaviour
{

    public GameObject PauseScreen;
    public GameObject BeginScreen;

    bool GamePaused;
    // Start is called before the first frame update
    void Start()
    {
        GamePaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void PauseGame()
    {
        GamePaused = true;
        PauseScreen.SetActive(true);

    }
    public void ResumeGame()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
    }
    public void BeginGame()
    {
        print("yes");
        GamePaused = false;
        BeginScreen.SetActive(false);
    }
}
