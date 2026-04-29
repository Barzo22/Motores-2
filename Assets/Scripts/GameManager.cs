using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    float coins = 0;
    HashSet<string> collectedKeys = new HashSet<string>();

    [SerializeField] int maxLives = 3;
    public int currentLives;

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
    }

    public void AddCoins(float amount)
    {
        coins += amount;
    }

    public float GetCoins() => coins;

    public void CollectKey(string keyID) => collectedKeys.Add(keyID);
    public bool HasKey(string keyID) => collectedKeys.Contains(keyID);
    public void UseKey(string keyID) => collectedKeys.Remove(keyID);

    public int GetLives() => currentLives;

    public void PlayerDied()
    {
        currentLives--;
        coins = 0;
        collectedKeys.Clear();

        if (currentLives <= 0)
            SceneManager.LoadScene("GameOver");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete()
    {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);  
        SceneManager.LoadScene("LevelComplete");
    }

    public void OnNextLevelButton()
    {
        int nextLevel = PlayerPrefs.GetInt("LastLevel", 2) + 1;
        currentLives = maxLives;
        coins = 0;
        collectedKeys.Clear();  
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
        SceneManager.LoadScene("Menu");
    }
}