using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "ScriptableObjects/ToolType", order = 1)]
public class ToolType : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
}
