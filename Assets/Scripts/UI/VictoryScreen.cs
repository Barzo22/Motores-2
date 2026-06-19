using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] Button nextLevelButton;
    [SerializeField] int lastLevel = 3;

    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] Image[] starImages;
    [SerializeField] Sprite starFilled;
    [SerializeField] Sprite starEmpty;

    void Start()
    {
        int completedLevel = PlayerPrefs.GetInt("LastLevel", 0);
        nextLevelButton.gameObject.SetActive(completedLevel < lastLevel);

        // mostramos el tiempo y las estrellas
        if (LevelTimer.Instance != null)
        {
            int stars = LevelTimer.Instance.GetStars();
            timeText.text = $"Tiempo: {LevelTimer.Instance.GetFormattedTime()}";
            GameManager.Instance.SaveLevelStars(completedLevel, stars);

            for (int i = 0; i < starImages.Length; i++)
                starImages[i].sprite = i < stars ? starFilled : starEmpty;
        }

        // mostramos las monedas recolectadas en este nivel
        int collected = GameManager.Instance.GetCoinsCollectedThisLevel();
        int total = GameManager.Instance.GetCoinsTotalThisLevel();
        coinsText.text = $"Monedas: {collected}/{total}";
    }

    public void OnMenu() => GameManager.Instance.OnMenuButton();
    public void OnNextLevel() => GameManager.Instance.OnNextLevelButton();
}