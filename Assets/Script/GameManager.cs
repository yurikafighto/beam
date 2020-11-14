using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private bool pauseStatus, playerDied;
    public void ResetLevel()
    {
        // re load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        playerDied = false;

    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Cursor.visible = false;
    }
    protected override void Awake()
    {
        pauseStatus = false;
        playerDied = false;
    }

    public void Play(bool play)
    {
        pauseStatus = !play;

        UserInterface.Instance.TogglePlayPause(pauseStatus);
        if (pauseStatus)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

    }

    private void Update()
    {
        // move backward
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !playerDied)
        {
            Play(pauseStatus);
        }
    }

    // stop activity
    public void StopActivity()
    {
        Time.timeScale = 0.0f;
        playerDied = true;
    }

    // load Main menu
    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }
}
