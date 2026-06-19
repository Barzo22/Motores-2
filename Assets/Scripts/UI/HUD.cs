using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text timerText;

    Coin[] allCoinsInLevel;

    void Start()
    {
        // guardamos referencia a todas las monedas del nivel (incluyendo inactivas)
        allCoinsInLevel = FindObjectsOfType<Coin>(true);
        GameManager.Instance.SetTotalCoinsInLevel(allCoinsInLevel.Length);
    }

    void Update()
    {
        // contamos cuántas monedas de este nivel ya fueron recolectadas
        // lo hacemos chequeando el HashSet persistente del GameManager
        int collected = 0;
        foreach (Coin coin in allCoinsInLevel)
        {
            string coinID = $"coin_{coin.transform.position.x}_{coin.transform.position.y}";
            if (GameManager.Instance.IsCoinCollected(coinID))
                collected++;
        }

        int total = GameManager.Instance.GetCoinsTotalThisLevel();
        coinsText.text = $"{collected}/{total}";

        if (LevelTimer.Instance != null)
            timerText.text = LevelTimer.Instance.GetFormattedTime();

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < GameManager.Instance.currentLives;
    }
}