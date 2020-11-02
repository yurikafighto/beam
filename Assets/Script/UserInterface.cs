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
        // subscribe to Player on score change
        Player.OnScoreChange = OnScoreChange;
        // subscribe to Player on score change
        Player.OnHPChange = OnHPChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnHPChange()
    {

    }

    private void OnScoreChange(int i)
    {
        score.text = $"SCORE : {i}";
    }
}
