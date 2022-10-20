using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResume : MonoBehaviour
{

    public GameObject PauseScreen;
    //public GameObject BeginScreen;

    bool GamePaused;
    // Start is called before the first frame update
    void Start()
    {
        //GamePaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameState();
        }
    }


    public void PauseGame()
    {
        GamePaused = true;
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Debug.Log("l");
        GamePaused = false;
        PauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ToggleGameState()
    {
        if(GamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    /*public void BeginGame()
    {
        print("yes");
        GamePaused = false;
        BeginScreen.SetActive(false);
    }
    */
}
