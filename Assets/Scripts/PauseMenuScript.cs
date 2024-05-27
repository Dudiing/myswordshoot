using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool activePauseUI = false;
    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        DisplayUI();
    }
    public void DisplayUI()
    {
        if (activePauseUI)
        {
            pauseMenuUI.SetActive(false);
            activePauseUI = false;
            Time.timeScale = 1;
            music.UnPause();
        }
        else
        {
            pauseMenuUI.SetActive(true);
            activePauseUI = true;
            Time.timeScale = 0;
            music.Pause();
        }
    }
    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayUI();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
