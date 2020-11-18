using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviourSingleton<UserInterface> 
{
    [SerializeField]
    GameObject gameOver, win, player, boss, pausePanel, playing;
    [SerializeField]
    Button playAgain, resume, mainMenuPause, mainMenuGameOver;
    [SerializeField]
    Text score, newBest, gameOverScore;
    [SerializeField]
    Slider hpBar, hpBoss;
    [SerializeField]
    Animator animation;

    public static string bestScoreKey = "BESTSCORE";
    private int currentScore;
    

    // Start is called before the first frame update
    void Start()
    {
        int max = player.GetComponent<Player>().GetMaxHP();
        int maxBoss = boss.GetComponent<Boss>().GetMaxHP();
        // initialize HP bars to max HP
        hpBar.maxValue = max;
        hpBar.value = max;
        hpBoss.maxValue = maxBoss;
        hpBoss.value = maxBoss;
        currentScore = 0;
        newBest.gameObject.SetActive(false);
        animation.SetTrigger("LevelStart");
    }


    private void OnHPChange(int currentHP)
    {
        hpBar.value = currentHP;
        if (currentHP <= 0)
        {
            Cursor.visible = true;
            playing.SetActive(false);
            GameManager.Instance.StopActivity();
            gameOver.SetActive(true);

            // display score
            gameOverScore.text = $"SCORE : {currentScore}";

            // get the current best score
            int bestScore = PlayerPrefs.GetInt(bestScoreKey);

            if(currentScore > bestScore)
            {
                newBest.gameObject.SetActive(true);
                PlayerPrefs.SetInt(bestScoreKey, currentScore);
                PlayerPrefs.Save();
            }
            
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

    private void OnBossAppear(bool b)
    {
        hpBoss.gameObject.SetActive(b);
    }

    private void OnScoreChange(int playerScore)
    {
        currentScore = playerScore;
        score.text = $"SCORE : {currentScore}";
    }

    protected override void Awake()
    {
        Cursor.visible = false;
        // subscribe to Player on score change
        Player.OnScoreChange = OnScoreChange;
        // subscribe to Player on score change
        Player.OnHPChange = OnHPChange;

        Boss.OnBossHPChange = OnBossHPChange;
        Boss.OnBossAppear = OnBossAppear;

        playAgain.onClick.AddListener(GameManager.Instance.ResetLevel);
        mainMenuPause.onClick.AddListener(GameManager.Instance.BackToMain);
        mainMenuGameOver.onClick.AddListener(GameManager.Instance.BackToMain);
        resume.onClick.AddListener(delegate { GameManager.Instance.Play(true); });

    }
    public void TogglePlayPause(bool pause)
    {
        pausePanel.SetActive(pause);
    }


}
