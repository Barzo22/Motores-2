using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    float coins = 0;
    HashSet<string> collectedKeys = new HashSet<string>();
    HashSet<string> collectedCoins = new HashSet<string>();
    HashSet<string> openedDoors = new HashSet<string>();

    [SerializeField] int maxLives = 3;
    public int currentLives;

    // guardamos las monedas del nivel actual para mostrarlas en la victoria
    int coinsCollectedThisLevel = 0;
    int coinsTotalThisLevel = 0;

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
        LoadCollectedCoins();
    }

    // --- Monedas ---

    public void CollectCoin(string coinID, float amount)
    {
        if (collectedCoins.Contains(coinID)) return;
        collectedCoins.Add(coinID);
        coins += amount;
        coinsCollectedThisLevel++;
        SaveCollectedCoins();
    }

    public bool IsCoinCollected(string coinID) => collectedCoins.Contains(coinID);

    public float GetCoins() => coins;

    public bool SpendCoins(float amount)
    {
        if (coins < amount) return false;
        coins -= amount;
        return true;
    }

    // lo llama el HUD al iniciar el nivel para registrar el total
    public void SetTotalCoinsInLevel(int total)
    {
        coinsTotalThisLevel = total;
        coinsCollectedThisLevel = 0;
    }

    public int GetCoinsCollectedThisLevel() => coinsCollectedThisLevel;
    public int GetCoinsTotalThisLevel() => coinsTotalThisLevel;

    void SaveCollectedCoins()
    {
        string joined = string.Join(",", collectedCoins);
        PlayerPrefs.SetString("CollectedCoins", joined);
        PlayerPrefs.Save();
    }

    void LoadCollectedCoins()
    {
        string joined = PlayerPrefs.GetString("CollectedCoins", "");
        if (string.IsNullOrEmpty(joined)) return;
        foreach (string id in joined.Split(','))
            collectedCoins.Add(id);
    }

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
        return PlayerPrefs.GetInt($"stars_level_{levelIndex}", 0);
    }

    // --- Vidas ---

    public int GetLives() => currentLives;

    public void PlayerDied()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            coins = 0;
            collectedKeys.Clear();
            openedDoors.Clear();
            if (LevelTimer.Instance != null)
                LevelTimer.Instance.ResetTimer();
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // --- Navegación ---

    public void LevelComplete()
    {
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.StopTimer();

        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("LevelComplete");
    }

    public void OnNextLevelButton()
    {
        int nextLevel = PlayerPrefs.GetInt("LastLevel", 2) + 1;
        currentLives = maxLives;
        coins = 0;
        collectedKeys.Clear();
        openedDoors.Clear();
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();
        SceneManager.LoadScene(nextLevel);
    }

    public void OnPlayButton()
    {
        maxLives = RemoteConfigManager.Instance != null
            ? RemoteConfigManager.Instance.MaxLives
            : maxLives;
        currentLives = maxLives;
        coins = 0;
        collectedKeys.Clear();
        openedDoors.Clear();
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();
        SceneManager.LoadScene("Level1");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnMenuButton()
    {
        currentLives = maxLives;
        coins = 0;
        collectedKeys.Clear();
        openedDoors.Clear();
        if (LevelTimer.Instance != null)
            LevelTimer.Instance.ResetTimer();
        SceneManager.LoadScene("Menu");
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        collectedCoins.Clear();
        coins = 0;
    }
}