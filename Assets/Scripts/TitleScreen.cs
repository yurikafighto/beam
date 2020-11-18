using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    Text bestScore;
    [SerializeField]
    Button Bplay,Bcontrols,Bcredits,Bleave,Breturn;
    [SerializeField]
    GameObject Main, Controls, Credits, Return;
    [SerializeField]
    List<GameObject> ObjectList;

    private void Awake()
    {
        // load next scene
        Bplay.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); });
        Breturn.onClick.AddListener(ShowMain);
        Bcontrols.onClick.AddListener(ShowControls);
        Bcredits.onClick.AddListener(ShowCredits);
        Bleave.onClick.AddListener(Application.Quit);
        bestScore.text = $"BEST SCORE : {PlayerPrefs.GetInt(UserInterface.bestScoreKey)}";
    }

    private void ShowControls()
    {
        Main.SetActive(false);
        Controls.SetActive(true);
        Return.SetActive(true);
    }

    private void ShowCredits()
    {
        Main.SetActive(false);
        Credits.SetActive(true);
        Return.SetActive(true);
    }
    private void ShowMain()
    {
        foreach (var obj in ObjectList)
            obj.SetActive(false);

        Main.SetActive(true);
    }
}
