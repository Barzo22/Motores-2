using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    [SerializeField] AudioClip victorySound;
    [SerializeField] AudioClip defeatSound;
    [SerializeField] AudioClip buttonClickSound;

    [SerializeField] float menuMusicVolume = 1f;
    [SerializeField] float gameplayMusicVolume = 0.4f;

    float musicVolume = 1f;
    float sfxVolume = 1f;

    float currentSceneVolume = 1f;

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
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyMusicVolume();
        ApplySFXVolume();

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSliders();

        switch (scene.name)
        {
            case "Menu":
            case "Splash":
                SetSceneVolume(menuMusicVolume);
                break;

            case "LevelComplete":
                SetSceneVolume(gameplayMusicVolume);
                PlaySFX(victorySound);
                break;

            case "GameOver":
                SetSceneVolume(gameplayMusicVolume);
                PlaySFX(defeatSound);
                break;

            default:
                SetSceneVolume(gameplayMusicVolume);
                break;
        }
    }

    void SetSceneVolume(float sceneVolume)
    {
        currentSceneVolume = sceneVolume;
        ApplyMusicVolume();
    }

    void FindSliders()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);

        musicSlider = null;
        sfxSlider = null;

        GameObject musicObj = GameObject.Find("MusicSlider");
        if (musicObj != null)
        {
            musicSlider = musicObj.GetComponent<Slider>();
            if (musicSlider != null)
            {
                musicSlider.SetValueWithoutNotify(musicVolume);
                musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }
        }
        else
        {
            Debug.LogWarning("VolumeManager: no se encontró ningún GameObject llamado 'MusicSlider'.");
        }

        GameObject sfxObj = GameObject.Find("SFXSlider");
        if (sfxObj != null)
        {
            sfxSlider = sfxObj.GetComponent<Slider>();
            if (sfxSlider != null)
            {
                sfxSlider.SetValueWithoutNotify(sfxVolume);
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }
        else
        {
            Debug.LogWarning("VolumeManager: no se encontró ningún GameObject llamado 'SFXSlider'.");
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        musicVolume = value;
        ApplyMusicVolume();
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        sfxVolume = value;
        ApplySFXVolume();
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
    public void PlaySFXAtVolume(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
    void ApplyMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume * currentSceneVolume;
    }

    void ApplySFXVolume()
    {
        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
    public void RefreshSliders()
    {
        FindSliders();
    }
    public void ResetVolume()
    {
        musicVolume = 1f;
        sfxVolume = 1f;

        ApplyMusicVolume();
        ApplySFXVolume();
        RefreshSliders();
    }
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }
}