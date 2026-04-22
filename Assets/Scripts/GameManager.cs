using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    int coins = 0;
    HashSet<string> collectedKeys = new HashSet<string>();

    [SerializeField] int maxLives = 3;
    int currentLives;

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
        currentLives = maxLives;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public int GetCoins() => coins;

    public void CollectKey(string keyID) => collectedKeys.Add(keyID);
    public bool HasKey(string keyID) => collectedKeys.Contains(keyID);
    public void UseKey(string keyID) => collectedKeys.Remove(keyID);

    public int GetLives() => currentLives;

    public void PlayerDied()
    {
        currentLives--;
        collectedKeys.Clear();

        if (currentLives <= 0)
            SceneManager.LoadScene("GameOver");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete()
    {
        SceneManager.LoadScene("Victory");
    }

    // Menú
    public void OnPlayButton()
    {
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