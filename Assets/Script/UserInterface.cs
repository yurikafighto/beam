using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour 
{
    [SerializeField]
    GameObject gameOver, player;
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
            gameOver.SetActive(true);
            playAgain.gameObject.SetActive(true);
        }
    }

    private void OnScoreChange(int currentScore)
    {
        score.text = $"SCORE : {currentScore}";
    }

    private void Awake()
    {
        // subscribe to Player on score change
        Player.OnScoreChange = OnScoreChange;
        // subscribe to Player on score change
        Player.OnHPChange = OnHPChange;


        
        playAgain.onClick.AddListener(GameManager.Instance.ResetLevel);
        
    }
}
