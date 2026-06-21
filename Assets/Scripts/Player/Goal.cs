using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.LevelComplete(GameManager.Instance.GetCoinsCollectedThisAttempt());
    }
}