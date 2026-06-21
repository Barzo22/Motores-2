using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    // referencias a los AudioSources del juego
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

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
        // cargamos los volúmenes guardados
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyMusicVolume(savedMusic);
        ApplySFXVolume(savedSFX);

        // suscribimos al evento de carga de escena para reasignar sliders
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // limpiamos los listeners de los sliders anteriores antes de buscar los nuevos
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);

        musicSlider = null;
        sfxSlider = null;

        FindSliders();
    }

    void FindSliders()
    {
        GameObject musicObj = GameObject.Find("MusicSlider");
        if (musicObj != null)
        {
            musicSlider = musicObj.GetComponent<Slider>();
            if (musicSlider != null)
            {
                musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }
        }

        GameObject sfxObj = GameObject.Find("SFXSlider");
        if (sfxObj != null)
        {
            sfxSlider = sfxObj.GetComponent<Slider>();
            if (sfxSlider != null)
            {
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    void ApplyMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    void ApplySFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}