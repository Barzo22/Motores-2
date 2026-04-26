using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using TMPro;

public class RemoteConfigManager : MonoBehaviour
{
    public struct UserAttributes { }
    public struct AppAttributes { }

    public static RemoteConfigManager Instance;

    public static event System.Action OnConfigLoaded;

    public int MaxLives { get; private set; } = 3;
    public float MoveSpeed { get; private set; } = 5f;
    public float EnemySpeed { get; private set; } = 3f;
    public float CoinValue { get; private set; } = 1f;
    public string GameName { get; private set; } = "Slidebound";

    [SerializeField] TMP_Text gameNameText;

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
    async void Start()
    {
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfig();
        }

        RemoteConfigService.Instance.FetchCompleted += FetchRemoteConfig;
        RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
    }

    async Task InitializeRemoteConfig()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    void FetchRemoteConfig(ConfigResponse response)
    {
        MaxLives = RemoteConfigService.Instance.appConfig.GetInt("max_lives", 3);
        MoveSpeed = RemoteConfigService.Instance.appConfig.GetFloat("move_speed", 10f);
        EnemySpeed = RemoteConfigService.Instance.appConfig.GetFloat("enemy_speed", 3f);
        CoinValue = RemoteConfigService.Instance.appConfig.GetFloat("coin_value", 1f);
        GameName = RemoteConfigService.Instance.appConfig.GetString("game_name", "Slidebound");

        ApplyConfig();
        OnConfigLoaded?.Invoke();
    }

    void ApplyConfig()
    {
        if (gameNameText != null)
            gameNameText.text = GameName;
    }
}