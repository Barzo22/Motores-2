using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void OnRetry() => GameManager.Instance.OnPlayButton();
    public void OnMenu() => GameManager.Instance.OnMenuButton();
}