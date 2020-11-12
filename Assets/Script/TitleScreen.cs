using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    Button Bplay,Bcontrols,Bscores,Bcredits,Bleave,Breturn;
    [SerializeField]
    GameObject Main, Controls, Scores, Credits, Return;
    [SerializeField]
    List<GameObject> ObjectList;


    private void Awake()
    {
        Bplay.onClick.AddListener(GameManager.Instance.NextLevel);
        Breturn.onClick.AddListener(ShowMain);
        Bcontrols.onClick.AddListener(ShowControls);
        Bscores.onClick.AddListener(ShowScores);
        //Bcredits.onClick.AddListener(GameManager.Instance.nextLevel);
        Bleave.onClick.AddListener(Application.Quit);
    }

    private void test()
    {
        Debug.Log("aa");
    }
    private void ShowControls()
    {
        Main.SetActive(false);
        Controls.SetActive(true);
        Return.SetActive(true);
    }

    private void ShowScores()
    {
        Main.SetActive(false);
        Scores.SetActive(true);
        Return.SetActive(true);
    }
    private void ShowMain()
    {
        foreach (var obj in ObjectList)
            obj.SetActive(false);

        Main.SetActive(true);
    }

}
