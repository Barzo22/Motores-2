using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class StaminaSystem : MonoBehaviour
{
    public static StaminaSystem Instance;

    [SerializeField] Sprite staminaFull;
    [SerializeField] Sprite staminaEmpty;
    [SerializeField] int maxStamina = 5;
    [SerializeField] int secondsToRecover = 60;

    Image[] staminaIcons;
    TMP_Text timerText;

    int currentStamina;
    bool recharging;

    DateTime nextStaminaTime;
    DateTime lastStaminaTime;
    bool staminaAdded = false;

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
        Load();
        StartCoroutine(RechargeStamina());
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        staminaIcons = null;
        timerText = null;
        FindStaminaUI();
        UpdateStaminaUI();
        UpdateTimerUI();
    }

    void FindStaminaUI()
    {
        GameObject staminaContainer = GameObject.Find("StaminaIcons");
        if (staminaContainer != null)
            staminaIcons = staminaContainer.GetComponentsInChildren<Image>();

        GameObject timerObj = GameObject.Find("StaminaTimer");
        if (timerObj != null)
            timerText = timerObj.GetComponent<TMP_Text>();
    }

    IEnumerator RechargeStamina()
    {
        recharging = true;

        while (currentStamina < maxStamina)
        {
            DateTime now = DateTime.Now;
            DateTime nextTime = nextStaminaTime;

            while (now > nextTime)
            {
                if (currentStamina >= maxStamina) break;

                staminaAdded = true;

                if (lastStaminaTime > nextTime)
                    nextTime = lastStaminaTime;

                currentStamina += 1;
                nextTime = nextTime.AddSeconds(secondsToRecover);
            }

            if (staminaAdded)
            {
                nextStaminaTime = nextTime;
                lastStaminaTime = DateTime.Now;
                staminaAdded = false;
                UpdateStaminaUI();
                Save();
            }

            UpdateTimerUI();
            yield return null;
        }

        recharging = false;
    }

    public bool UseStamina(int amount = 1)
    {
        if (currentStamina < amount)
        {
            Debug.Log("No hay suficiente stamina!");
            return false;
        }

        currentStamina -= amount;

        if (!recharging)
        {
            nextStaminaTime = DateTime.Now.AddSeconds(secondsToRecover);
            StartCoroutine(RechargeStamina());
        }

        UpdateStaminaUI();
        Save();
        return true;
    }

    public void AddStamina(int amount = 1)
    {
        currentStamina += amount;

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            if (recharging)
            {
                recharging = false;
                StopAllCoroutines();
            }
        }

        UpdateStaminaUI();
        UpdateTimerUI();
        Save();
    }

    public void ResetStamina()
    {
        StopAllCoroutines();
        currentStamina = maxStamina;
        recharging = false;
        nextStaminaTime = DateTime.Now;
        lastStaminaTime = DateTime.Now;
        UpdateStaminaUI();
        UpdateTimerUI();
    }

    public int GetCurrentStamina() => currentStamina;
    public int GetMaxStamina() => maxStamina;
    public bool HasStamina() => currentStamina > 0;

    void UpdateStaminaUI()
    {
        if (staminaIcons == null) return;

        for (int i = 0; i < staminaIcons.Length; i++)
        {
            if (staminaIcons[i] == null) continue;
            staminaIcons[i].sprite = i < currentStamina ? staminaFull : staminaEmpty;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        if (currentStamina >= maxStamina)
        {
            timerText.text = "Full";
            return;
        }

        var timer = nextStaminaTime - DateTime.Now;
        if (timer.TotalSeconds < 0)
            timerText.text = "00:00";
        else
            timerText.text = $"{timer.Minutes:00}:{timer.Seconds:00}";
    }

    void Save()
    {
        PlayerPrefs.SetInt("CurrentStamina", currentStamina);
        PlayerPrefs.SetString("NextStaminaTime", nextStaminaTime.ToString());
        PlayerPrefs.SetString("LastStaminaTime", lastStaminaTime.ToString());
        PlayerPrefs.Save();
    }

    void Load()
    {
        currentStamina = PlayerPrefs.GetInt("CurrentStamina", maxStamina);
        nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("NextStaminaTime", ""));
        lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("LastStaminaTime", ""));
    }

    DateTime StringToDateTime(string date)
    {
        if (string.IsNullOrEmpty(date)) return DateTime.Now;
        return DateTime.Parse(date);
    }
}