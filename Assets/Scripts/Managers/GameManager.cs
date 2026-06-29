using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    float coins = 0;
    HashSet<string> collectedKeys = new HashSet<string>();
    HashSet<string> openedDoors = new HashSet<string>();
    HashSet<string> coinsThisAttempt = new HashSet<string>();

    [SerializeField] int maxLives = 3;
    public int currentLives;

    int coinsTotalThisLevel = 0;
    int coinsCollectedOnComplete = 0;

    [Header("Death Effect")]
    [SerializeField] ParticleSystem deathEffect;

    [SerializeField] float debugCoins = 100f;

    [ContextMenu("Add Debug Coins")]
    void AddDebugCoins()
    {
        coins = debugCoins;
        PlayerPrefs.SetFloat("TotalCoins", coins);
        PlayerPrefs.Save();
        Debug.Log($"Debug coins set to {coins}");
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        maxLives = RemoteConfigManager.Instance != null
            ? RemoteConfigManager.Instance.MaxLives
            : maxLives;
        currentLives = maxLives;
        coins = PlayerPrefs.GetFloat("TotalCoins", 0);
    }

    // --- Monedas ---

    public void AddCoins(string coinID, float amount)
    {
        if (coinsThisAttempt.Contains(coinID)) return;
        coinsThisAttempt.Add(coinID);
        coins += amount;
        PlayerPrefs.SetFloat("TotalCoins", coins);
        PlayerPrefs.Save();
    }

    public bool IsCoinCollectedThisAttempt(string coinID) => coinsThisAttempt.Contains(coinID);

    public float GetCoins() => coins;

    public bool SpendCoins(float amount)
    {
        if (coins < amount) return false;
        coins -= amount;
        PlayerPrefs.SetFloat("TotalCoins", coins);
        PlayerPrefs.Save();
        return true;
    }

    public void SetTotalCoinsInLevel(int total) => coinsTotalThisLevel = total;
    public int GetCoinsTotalThisLevel() => coinsTotalThisLevel;
    public int GetCoinsCollectedThisAttempt() => coinsThisAttempt.Count;
    public int GetCoinsCollectedOnComplete() => coinsCollectedOnComplete;

    // --- Llaves ---

    public void CollectKey(string keyID) => collectedKeys.Add(keyID);
    public bool HasKey(string keyID) => collectedKeys.Contains(keyID);
    public void UseKey(string keyID) => collectedKeys.Remove(keyID);

    // --- Puertas ---

    public void RegisterDoorOpen(string doorID) => openedDoors.Add(doorID);
    public bool IsDoorOpen(string doorID) => openedDoors.Contains(doorID);

    // --- Estrellas ---

    public void SaveLevelStars(int levelIndex, int stars)
    {
        string key = $"stars_level_{levelIndex}";
        int current = PlayerPrefs.GetInt(key, 0);
        if (stars > current)
        {
            PlayerPrefs.SetInt(key, stars);
            PlayerPrefs.Save();
        }
    }

    public int GetLevelStars(int levelIndex)
    {
        int stars = PlayerPrefs.GetInt($"stars_level_{levelIndex}", 0);
        Debug.Log($"GetLevelStars level {levelIndex}: {stars}");
        return stars;
    }

    // --- Vidas ---

    public int GetLives() => currentLives;

    public void PlayerDied()
    {
        currentLives--;

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.PlayDeathSound();

            if (deathEffect != null)
            {
                ParticleSystem ps = Instantiate(deathEffect, PlayerMovement.Instance.transform.position, Quaternion.identity);

                if (SkinManager.Instance != null)
                    SkinManager.Instance.ApplyColorToParticleSystem(ps);

                ps.Play();
            }

            PlayerMovement.Instance.gameObject.SetActive(false);
        }

        if (currentLives <= 0)
            StartCoroutine(LoadSceneDelayed("GameOver"));
        else
            StartCoroutine(LoadSceneDelayed(SceneManager.GetActiveScene().name));
    }

    IEnumerator LoadSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(0.6f);

        if (sceneName == "GameOver")
        {
            coinsThisAttempt.Clear();
            collectedKeys.Clear();
            openedDoors.Clear();
            if (LevelTimer.Instance != null)
                LevelTimer.Instance.ResetTimer();
        }

        SceneManager.LoadScene(sceneName);
    }

    // --- Navegación ---

    public void LevelComplete(int coinsCollected)
    {
        coinsCollectedOnComplete = coinsCollected;

        if (LevelTimer.Instance != null)
            LevelTimer.Instance.StopTimer();

        coinsThisAttempt.Clear();
        collectedKeys.Clear();
        openedDoors.Clear();

        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("LevelComplete");
    }

    public bool TryEnterLevel(string sceneName)
    {
        if (StaminaSystem.Instance != null && !StaminaSystem.Instance.UseStamina())
        {
            Debug.Log("Sin stamina para entrar al nivel!");
            return false;
        }

        maxLives = RemoteConfigManager.Instance != null
            ? RemoteConfigManager.Instance.MaxLives
            : maxLives;
        currentLives = maxLives;
        coinsThisAttempt.Clear();
        collectedKeys.Clear();
        openedDoors.Clear();

        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();

        SceneManager.LoadScene(sceneName);
        return true;
    }

    public void OnNextLevelButton()
    {
        if (StaminaSystem.Instance != null && !StaminaSystem.Instance.UseStamina())
        {
            Debug.Log("Sin stamina para entrar al nivel!");
            return;
        }

        int nextLevel = PlayerPrefs.GetInt("LastLevel", 2) + 1;
        currentLives = maxLives;
        coinsThisAttempt.Clear();
        collectedKeys.Clear();
        openedDoors.Clear();
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();
        SceneManager.LoadScene(nextLevel);
    }

    public void OnPlayButton()
    {
        TryEnterLevel("Level1");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnMenuButton()
    {
        currentLives = maxLives;
        coinsThisAttempt.Clear();
        collectedKeys.Clear();
        openedDoors.Clear();
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();
        SceneManager.LoadScene("Menu");
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log($"After delete, stars_level_2: {PlayerPrefs.GetInt("stars_level_2", 0)}");
        coins = 0;
        coinsThisAttempt.Clear();
        collectedKeys.Clear();
        openedDoors.Clear();
        if (StaminaSystem.Instance != null)
            StaminaSystem.Instance.ResetStamina();
        if (SkinManager.Instance != null)
            SkinManager.Instance.ResetSkins();
    }
}