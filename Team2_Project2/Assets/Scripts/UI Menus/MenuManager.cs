using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    //public AudioSource clickNoise;

    public GameObject deathMenu;

    public void OnPlayButtonClick()
    {
        //ButtonClick();
        PlayerData.instance.NewGame();
        SceneManager.LoadScene("Level_1");

    }
    public void OnTrainingButtonClick()
    {
        //ButtonClick();
        SceneManager.LoadScene("Training");

    }
    public void OnHelpButtonClick()
    {
        //ButtonClick();
        SceneManager.LoadScene("Help");

    }
    public void OnCreditsButtonClick()
    {
        //ButtonClick();
        SceneManager.LoadScene("Credits");

    }
    public void OnMainMenuButtonClick()
    {
        //ButtonClick();
        SceneManager.LoadScene("Title");

    }
    public void OnQuitButtonClick()
    {
        //ButtonClick();
        Application.Quit();

    }
    public void OnRestartButtonClick()
    {
        Time.timeScale = 1;
        //ButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerLose()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /*private void ButtonClick()
    {
        clickNoise.Play();
    }
    */
}
