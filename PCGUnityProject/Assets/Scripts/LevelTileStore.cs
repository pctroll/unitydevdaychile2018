using UnityEngine;

[CreateAssetMenu(fileName = "LevelTileStore", menuName = "PCG/LevelTileStore", order = 0)]
public class LevelTileStore : ScriptableObject
{
    public Sprite spriteReference;
    public Sprite[] spriteList;
}