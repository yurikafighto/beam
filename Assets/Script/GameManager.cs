using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{

    public void ResetLevel()
    {
        // re load current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
