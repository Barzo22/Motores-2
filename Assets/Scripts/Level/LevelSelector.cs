using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] LevelButton buttonPrefab;
    [SerializeField] Transform content;

    [SerializeField] Sprite starFilled;
    [SerializeField] Sprite starEmpty;

    [SerializeField] GameObject noStaminaPanel;

    void Start()
    {
        if (noStaminaPanel != null)
            noStaminaPanel.SetActive(false);

        for (int i = 0; i < levels.Length; i++)
        {
            var newButton = Instantiate(buttonPrefab, content);
            newButton.SetLevel(levels[i], starFilled, starEmpty, noStaminaPanel);
        }
    }
}
