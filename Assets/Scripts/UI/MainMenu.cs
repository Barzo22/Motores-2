using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject noStaminaPanel;

    [SerializeField] CanvasGroup menuButtonsGroup;
    [SerializeField] LevelSelector levelSelector;
    void Start()
    {
        if (noStaminaPanel != null)
            noStaminaPanel.SetActive(false);

        SetMenuInteractable(true);
    }

    void Update()
    {
        if (coinsText != null && GameManager.Instance != null)
            coinsText.text = GameManager.Instance.GetCoins().ToString();
    }

    public void SetMenuInteractable(bool interactable)
    {
        if (menuButtonsGroup == null) return;
        menuButtonsGroup.interactable = interactable;
        menuButtonsGroup.blocksRaycasts = interactable;
    }

    public void OnPlay()
    {
        VolumeManager.Instance.PlayButtonClick();

        if (StaminaSystem.Instance == null)
        {
            GameManager.Instance?.OnPlayButton();
            return;
        }

        if (!StaminaSystem.Instance.HasStamina())
        {
            if (noStaminaPanel != null)
                noStaminaPanel.SetActive(true);
            SetMenuInteractable(false);
            return;
        }

        GameManager.Instance?.OnPlayButton();
    }

    public void OnOpenPanel(GameObject panel)
    {
        VolumeManager.Instance.PlayButtonClick();
        panel.SetActive(true);
        VolumeManager.Instance.RefreshSliders();
        SetMenuInteractable(false);
    }

    public void OnClosePanel(GameObject panel)
    {
        VolumeManager.Instance.PlayButtonClick();
        panel.SetActive(false);
        SetMenuInteractable(true);
    }

    public void OnDeleteSaveData()
    {
        VolumeManager.Instance.PlayButtonClick();
        GameManager.Instance.DeleteSaveData();
        VolumeManager.Instance?.ResetVolume();

        if (Shop.Instance != null)
            Shop.Instance.RefreshAllButtons();
        if (levelSelector != null)
            levelSelector.RefreshButtons();
    }
    public void OnUnequipSkin()
    {
        VolumeManager.Instance.PlayButtonClick();
        if (SkinManager.Instance != null)
            SkinManager.Instance.UnequipSkin();
    }
    public void OnExit() => GameManager.Instance?.OnExitButton();
}