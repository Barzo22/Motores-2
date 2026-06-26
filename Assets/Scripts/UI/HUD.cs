using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] PauseMenu pauseMenu;

    void Start()
    {
        Coin[] allCoins = FindObjectsOfType<Coin>(true);
        GameManager.Instance.SetTotalCoinsInLevel(allCoins.Length);
    }

    void Update()
    {
        int collected = GameManager.Instance.GetCoinsCollectedThisAttempt();
        int total = GameManager.Instance.GetCoinsTotalThisLevel();
        coinsText.text = $"{collected}/{total}";

        if (LevelTimer.Instance != null)
            timerText.text = LevelTimer.Instance.GetFormattedTime();

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < GameManager.Instance.currentLives;
    }

    public void OnPauseButton()
    {
        Debug.Log("OnPauseButton called");
        pauseMenu.TogglePause();
    }
}