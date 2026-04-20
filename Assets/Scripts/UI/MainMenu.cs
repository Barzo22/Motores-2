using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnPlay() => GameManager.Instance.OnPlayButton();
    public void OnExit() => GameManager.Instance.OnExitButton();
}