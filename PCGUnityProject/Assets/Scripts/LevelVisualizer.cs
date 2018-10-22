using UnityEngine;
// using UnityEngine.UI;
using System.Collections.Generic;

public class LevelVisualizer : MonoBehaviour
{
    [SerializeField]
    private LevelTileStore _tileStore;
    private const int _minIndex = -1;
    private int _maxIndex = 0;
    private List<GameObject> _tileList;
    private GameObject _prefab;
    private Vector3 _size;

    public void Render(int[,] grid)
    {
        Clear();
        Setup();
        int i, j, height, width, index;
        height = grid.GetLength(0);
        width = grid.GetLength(1);
        for (j = 0; j < width; j++)
        {
            for (i = 0; i < height; i++)
            {
                index = Mathf.Clamp(grid[i,j], _minIndex, _maxIndex);
                if (index == -1)
                    continue;
                GameObject tile = CreateTile(index);
                tile.transform.position = CalculatePosition(i, j);
                tile.transform.parent = transform;
                _tileList.Add(tile);
            }
        }
        // for (i = 0; i < height; i++)
        // {
        //     for (j = 0; j < width; i++)
        //     {
        //     }
        // }
    }

    public void Setup()
    {
        _maxIndex = _tileStore.spriteList.Length - 1;
        Sprite reference = _tileStore.spriteReference;
        print("Rect: " + reference.rect.ToString());
        _prefab = new GameObject();
        SpriteRenderer renderer = _prefab.AddComponent<SpriteRenderer>();
        renderer.sprite = _tileStore.spriteReference;
        _size = renderer.bounds.size;
        Destroy(_prefab);
    }

    public void Clear()
    {
        while (_tileList.Count != 0)
        {
            GameObject obj = _tileList[_tileList.Count - 1];
            obj.transform.parent = null;
            Destroy(obj);
        }
        _tileList.Clear();
    }

    public int[,] GetTestLevel()
    {
        int[,] level = new int[,] {{-1, -1, -1}, {-1, -1, -1}, {0, 0, 0}};
        return level;
    }

    private GameObject CreateTile(int index)
    {
        GameObject tile = new GameObject();
        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = _tileStore.spriteList[index];
        return tile;
    }

    private Vector2 CalculatePosition(int i, int j)
    {
        Vector2 position = Vector2.zero;
        position.x = j * _size.x;
        position.y = i * _size.y;
        return position;
    }
    private void Start()
    {
        _tileList = new List<GameObject>();
        if (_tileStore == null)
            Debug.LogError("No TileStore attached to Visualizer");
        if (_tileStore.spriteList == null || _tileStore.spriteList.Length == 0)
        {
            Debug.LogError("No tiles found in TileStore");
        }


        int[,] level = GetTestLevel();
        Render(level);
    }
}