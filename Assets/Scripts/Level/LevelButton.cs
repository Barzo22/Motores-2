using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TMP_Text levelNameText;
    [SerializeField] Image levelPreviewImage;
    [SerializeField] Image[] starImages;
    [SerializeField] Sprite starFilled;
    [SerializeField] Sprite starEmpty;
    [SerializeField] Button button;

    [SerializeField] GameObject noStaminaPanel;

    LevelData levelData;

    void Awake()
    {
        button.onClick.AddListener(OnLevelButtonClicked);
    }

    public void SetLevel(LevelData data, Sprite filled, Sprite empty, GameObject noStamina)
    {
        levelData = data;
        starFilled = filled;
        starEmpty = empty;
        noStaminaPanel = noStamina;

        levelNameText.text = data.levelName;

        if (data.levelPreview != null)
            levelPreviewImage.sprite = data.levelPreview;

        int stars = GameManager.Instance.GetLevelStars(data.levelIndex);
        for (int i = 0; i < starImages.Length; i++)
            starImages[i].sprite = i < stars ? starFilled : starEmpty;
    }

    void OnLevelButtonClicked()
    {
        if (StaminaSystem.Instance != null && !StaminaSystem.Instance.HasStamina())
        {
            if (noStaminaPanel != null)
                noStaminaPanel.SetActive(true);
            return;
        }

        GameManager.Instance.TryEnterLevel(levelData.sceneName);
    }
}
