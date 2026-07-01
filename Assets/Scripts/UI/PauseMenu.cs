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

        if (isPaused && VolumeManager.Instance != null)
            VolumeManager.Instance.RefreshSliders();

        if (LevelTimer.Instance != null)
        {
            if (isPaused) LevelTimer.Instance.StopTimer();
            else LevelTimer.Instance.StartTimer();
        }
    }

    public void OnResume()
    {
        VolumeManager.Instance.PlayButtonClick();

        isPaused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.StartTimer();
    }

    public void OnOptions()
    {
        VolumeManager.Instance.PlayButtonClick();

        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);

        if (VolumeManager.Instance != null)
            VolumeManager.Instance.RefreshSliders();
    }

    public void OnCloseOptions()
    {
        VolumeManager.Instance.PlayButtonClick();

        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OnMenu()
    {
        VolumeManager.Instance.PlayButtonClick();

        Time.timeScale = 1f;
        GameManager.Instance.OnMenuButton();
    }
}
