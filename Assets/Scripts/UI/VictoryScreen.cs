using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] Button nextLevelButton;
    [SerializeField] int lastLevel = 3;

    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] Image[] starImages;
    [SerializeField] Sprite starFilled;
    [SerializeField] Sprite starEmpty;

    [SerializeField] Image[] row3Stars;
    [SerializeField] TMP_Text row3Text;
    [SerializeField] Image[] row2Stars;
    [SerializeField] TMP_Text row2Text;
    [SerializeField] Image[] row1Stars;
    [SerializeField] TMP_Text row1Text;

    [SerializeField] Color activeColor = new Color(1f, 0.85f, 0f);
    [SerializeField] Color inactiveColor = new Color(0.5f, 0.5f, 0.5f);

    [SerializeField] GameObject noStaminaPanel;

    void Start()
    {
        int completedLevel = PlayerPrefs.GetInt("LastLevel", 0);
        nextLevelButton.gameObject.SetActive(completedLevel < lastLevel);

        if (noStaminaPanel != null)
            noStaminaPanel.SetActive(false);

        if (LevelTimer.Instance != null)
        {
            int stars = LevelTimer.Instance.GetStars();
            float star3Time = LevelTimer.Instance.GetStar3Time();
            float star2Time = LevelTimer.Instance.GetStar2Time();

            timeText.text = LevelTimer.Instance.GetFormattedTime();
            GameManager.Instance.SaveLevelStars(completedLevel, stars);

            for (int i = 0; i < starImages.Length; i++)
                starImages[i].sprite = i < stars ? starFilled : starEmpty;

            SetRow(row3Stars, row3Text, 3, $"{FormatTime(star3Time)}", stars == 3);
            SetRow(row2Stars, row2Text, 2, $"{FormatTime(star2Time)}", stars == 2);
            SetRow(row1Stars, null, 1, "", stars == 1);
        }

        int collected = GameManager.Instance.GetCoinsCollectedOnComplete();
        int total = GameManager.Instance.GetCoinsTotalThisLevel();
        coinsText.text = $"{collected}/{total}";
    }

    void SetRow(Image[] stars, TMP_Text label, int filledCount, string text, bool isActive)
    {
        if (stars == null) return;

        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = i < filledCount ? starFilled : starEmpty;

        foreach (Image star in stars)
            star.color = isActive ? Color.white : inactiveColor;

        if (label != null)
        {
            label.text = text;
            label.color = isActive ? activeColor : inactiveColor;
        }
    }

    string FormatTime(float seconds)
    {
        int m = Mathf.FloorToInt(seconds / 60f);
        int s = Mathf.FloorToInt(seconds % 60f);
        return $"{m:00}:{s:00}";
    }

    public void OnMenu()
    {
        VolumeManager.Instance.PlayButtonClick();
        GameManager.Instance.OnMenuButton();
    }

    public void OnNextLevel()
    {
        VolumeManager.Instance.PlayButtonClick();

        if (StaminaSystem.Instance != null && !StaminaSystem.Instance.HasStamina())
        {
            if (noStaminaPanel != null)
                noStaminaPanel.SetActive(true);
            return;
        }

        GameManager.Instance.OnNextLevelButton();
    }
}