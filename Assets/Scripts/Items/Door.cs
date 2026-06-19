using UnityEngine;

public class Door : MonoBehaviour
{
    string doorID;

    void Start()
    {
        doorID = $"door_{transform.position.x}_{transform.position.y}";

        if (GameManager.Instance.IsDoorOpen(doorID))
            gameObject.SetActive(false);
    }

    public void Open()
    {
        GameManager.Instance.RegisterDoorOpen(doorID);
        gameObject.SetActive(false);
    }
}