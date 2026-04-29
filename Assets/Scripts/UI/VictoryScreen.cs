using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] Button nextLevelButton;
    [SerializeField] int lastLevel = 3;

    void Start()
    {
        int completedLevel = PlayerPrefs.GetInt("LastLevel", 0);
        nextLevelButton.gameObject.SetActive(completedLevel < lastLevel);
    }
    public void OnMenu() => GameManager.Instance.OnMenuButton();
   public void OnNextLevel() => GameManager.Instance.OnNextLevelButton();

}