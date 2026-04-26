using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;

    void Start()
    {
        if (RemoteConfigManager.Instance != null)
        {
            titleText.text = RemoteConfigManager.Instance.GameName;
        }
    }

    public void OnPlay() => GameManager.Instance.OnPlayButton();
    public void OnExit() => GameManager.Instance.OnExitButton();
}