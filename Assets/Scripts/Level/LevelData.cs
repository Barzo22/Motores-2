using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Custom/Create Level", order = 0)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;  
    public Sprite levelPreview; 
    public int levelIndex;   
}
