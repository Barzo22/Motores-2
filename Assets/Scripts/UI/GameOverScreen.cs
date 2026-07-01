using UnityEngine;
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject noStaminaPanel;

    public void OnRetry()
    {
        if (StaminaSystem.Instance != null && !StaminaSystem.Instance.HasStamina())
        {
            if (noStaminaPanel != null)
                noStaminaPanel.SetActive(true);
            return;
        }
        GameManager.Instance.OnPlayButton();
    }

    public void OnMenu() => GameManager.Instance.OnMenuButton();
}