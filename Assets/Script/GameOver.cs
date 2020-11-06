using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Button playAgain;

    private void Awake()
    {
        playAgain.onClick.AddListener(GameManager.Instance.ResetLevel);
    }
}
