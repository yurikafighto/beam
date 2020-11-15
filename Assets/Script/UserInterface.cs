using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviourSingleton<UserInterface> 
{
    [SerializeField]
    GameObject gameOver, win, player, boss, pausePanel, playing;
    [SerializeField]
    Button playAgain, resume, mainMenu;
    [SerializeField]
    Text score;
    [SerializeField]
    Slider hpBar, hpBoss;


    // Start is called before the first frame update
    void Start()
    {
        int max = player.GetComponent<Player>().GetMaxHP();
        int maxBoss = boss.GetComponent<Boss1>().GetMaxHP();
        // initialize HP bars to max HP
        hpBar.maxValue = max;
        hpBar.value = max;
        hpBoss.maxValue = maxBoss;
        hpBoss.value = maxBoss;
    }


    private void OnHPChange(int currentHP)
    {
        hpBar.value = currentHP;
        if (currentHP <= 0)
        {
            
            Cursor.visible = true;
            gameOver.SetActive(true);
            playing.SetActive(false);
            GameManager.Instance.StopActivity();
        }
    }

    private void OnBossHPChange(int currentHP)
    {
        hpBoss.value = currentHP;
        if (currentHP <= 0)
        {
            Cursor.visible = true;
            win.SetActive(true);
            playing.SetActive(false);
        }
    }

    private void OnBossAppear()
    {
        hpBoss.gameObject.SetActive(true);
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
        Boss1.OnBossHPChange = OnBossHPChange;

        Boss1.OnBossAppear = OnBossAppear;

        playAgain.onClick.AddListener(GameManager.Instance.ResetLevel);
        mainMenu.onClick.AddListener(GameManager.Instance.BackToMain);
        resume.onClick.AddListener(delegate { GameManager.Instance.Play(true); });

    }
    public void TogglePlayPause(bool pause)
    {
        pausePanel.SetActive(pause);
    }


}
