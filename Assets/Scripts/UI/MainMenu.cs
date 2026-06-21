using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] GameObject noStaminaPanel;

    void Start()
    {
        if (RemoteConfigManager.Instance != null)
            titleText.text = RemoteConfigManager.Instance.GameName;

        if (noStaminaPanel != null)
            noStaminaPanel.SetActive(false);
    }

    void Update()
    {
        if (coinsText != null && GameManager.Instance != null)
            coinsText.text = GameManager.Instance.GetCoins().ToString();
    }

    public void OnPlay()
    {
        // null check por si arrancamos directo desde la escena del menú
        if (StaminaSystem.Instance == null)
        {
            GameManager.Instance?.OnPlayButton();
            return;
        }

        if (!StaminaSystem.Instance.HasStamina())
        {
            if (noStaminaPanel != null)
                noStaminaPanel.SetActive(true);
            return;
        }

        GameManager.Instance?.OnPlayButton();
    }

    public void OnExit() => GameManager.Instance?.OnExitButton();
}