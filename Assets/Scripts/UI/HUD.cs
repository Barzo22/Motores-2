using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Image[] hearts;
    [SerializeField] TMP_Text coinsText;


    void Update()
    {
        coinsText.text = GameManager.Instance.GetCoins().ToString(); 

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < GameManager.Instance.currentLives;
        }
    }
}