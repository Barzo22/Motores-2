using UnityEngine;
public class LevelTimer : MonoBehaviour
{
    public static LevelTimer Instance;

    [SerializeField] float star3Time = 30f;
    [SerializeField] float star2Time = 60f;

    float elapsedTime = 0f;
    bool isRunning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.star3Time = star3Time;
            Instance.star2Time = star2Time;
            Destroy(gameObject);
        }
    }

    void Start() => StartTimer();

    void Update()
    {
        if (!isRunning) return;
        elapsedTime += Time.deltaTime;
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public float GetElapsedTime() => elapsedTime;
    public float GetStar3Time() => star3Time;
    public float GetStar2Time() => star2Time;

    public int GetStars()
    {
        if (elapsedTime <= star3Time) return 3;
        if (elapsedTime <= star2Time) return 2;
        return 1;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}