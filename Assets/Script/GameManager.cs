using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    private bool pauseStatus;
    public void ResetLevel()
    {
        //string sceneName = PlayerPrefs.GetString("lastLoadedScene");
        //SceneManager.LoadScene(sceneName);//back to previous scene

        // re load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.visible = false;
    }

    protected override void Awake()
    {
        pauseStatus = false;
    }

    public void Play (bool play)
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
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            Play(pauseStatus);
        }
    }


}
