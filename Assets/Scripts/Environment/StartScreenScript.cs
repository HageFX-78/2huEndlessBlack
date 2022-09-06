using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreenScript : MonoBehaviour
{
    public GameObject controls;
    public GameObject startScreen;
    public void startGame()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        SceneManager.LoadScene("Main");
    }

    public void controlScreen()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        startScreen.SetActive(false);
        controls.SetActive(true);
    }

    public void goStartScreen()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        startScreen.SetActive(true);
        controls.SetActive(false);
    }
    public void endGame()
    {
        Application.Quit();
    }
}
