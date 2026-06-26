using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;

    bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Debug.Log($"LevelTimer instance: {LevelTimer.Instance}");
        if (LevelTimer.Instance != null)
        {
            if (isPaused) LevelTimer.Instance.StopTimer();
            else LevelTimer.Instance.StartTimer();
        }
    }

    public void OnResume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.StartTimer();
    }

    public void OnOptions()
    {
        // mostramos el panel de opciones y ocultamos el de pausa
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnCloseOptions()
    {
        // volvemos al panel de pausa
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OnMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.OnMenuButton();
    }
}
