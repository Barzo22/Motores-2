using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] LevelButton buttonPrefab;
    [SerializeField] Transform content;

    [SerializeField] Sprite starFilled;
    [SerializeField] Sprite starEmpty;

    [SerializeField] GameObject noStaminaPanel;

    void OnEnable()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        for (int i = 0; i < levels.Length; i++)
        {
            var newButton = Instantiate(buttonPrefab, content);
            newButton.SetLevel(levels[i], starFilled, starEmpty, noStaminaPanel);
        }
    }
}
