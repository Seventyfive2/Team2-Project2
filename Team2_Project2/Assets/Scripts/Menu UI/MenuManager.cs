using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    //public AudioSource clickNoise;

    public void OnPlayButtonClick()
    {
        //ButtonClick();
        SceneManager.LoadScene("Level_1");

    }
    public void OnTrainButtonClick()
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
        //ButtonClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      

    }

}
