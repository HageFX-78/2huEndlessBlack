using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool gamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject goMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !goMenu.activeSelf)
        {
            FindObjectOfType<AudioManager>().plyAudio("confirm");
            if(gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    void Pause()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    void Resume()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void mainMenu()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        SceneManager.LoadScene("StartScreen");
        Time.timeScale = 1f;
    }


    public void restartGame()
    {
        FindObjectOfType<AudioManager>().plyAudio("confirm");
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }
}
