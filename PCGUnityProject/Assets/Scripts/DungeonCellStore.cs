using UnityEngine;

[CreateAssetMenu(fileName = "DungeonCellStore", menuName = "PCG/Dungeon Cell Store")]
public class DungeonCellStore : ScriptableObject
{
    public GameObject prefab;
    public Sprite[] spriteList;
}
