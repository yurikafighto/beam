﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviourSingleton<UserInterface> 
{
    [SerializeField]
    GameObject gameOverCanvas, player, pausePanel;
    [SerializeField]
    Button playAgain;
    [SerializeField]
    Text score;
    [SerializeField]
    Slider hpBar;


    // Start is called before the first frame update
    void Start()
    {
        int max = player.GetComponent<Player>().GetMaxHP();
        // initialize HP bar to max HP
        hpBar.maxValue = max;
        hpBar.value = max;

    }


    private void OnHPChange(int currentHP)
    {
        hpBar.value = currentHP;
        if (currentHP <= 0)
        {
            
            Cursor.visible = true;
            //SceneManager.LoadScene("Game Over");
            gameOverCanvas.SetActive(true);
            playAgain.gameObject.SetActive(true);
        }
    }

    private void OnScoreChange(int currentScore)
    {
        score.text = $"SCORE : {currentScore}";
    }

    protected override void Awake()
    {
        Cursor.visible = false;
        // subscribe to Player on score change
        Player.OnScoreChange = OnScoreChange;
        // subscribe to Player on score change
        Player.OnHPChange = OnHPChange;

        playAgain.onClick.AddListener(GameManager.Instance.ResetLevel);

    }
    public void TogglePlayPause(bool pause)
    {
        pausePanel.SetActive(pause);
    }


}
