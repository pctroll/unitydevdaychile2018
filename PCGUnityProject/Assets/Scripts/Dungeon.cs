using UnityEngine;
using System.Collections.Generic;

public class Dungeon : MonoBehaviour
{
    [Range(3, 200)]
    public int maxHeight;
    [Range(3, 200)]
    public int maxWidth;
    /// <summary>
    /// Minimum Acceptable Size
    /// </summary>
    /// [
    [Range(2, 100)]
    public int minAcceptSize;
    /// <summary>
    /// Dungeon area
    /// </summary>
    private Rect area;
    public Dictionary<int, List<BSPNode>> tree;
    public HashSet<BSPNode> leaves;

    public delegate Rect[] Split(Rect area);
    public Split splitCall;

    public BSPNode root;

    public int[,] grid;


    public void Init()
    {
        area = new Rect(0f, 0f, maxWidth, maxHeight);
        grid = new int[maxHeight, maxWidth];
        //leaves.Clear();
        tree.Clear();
        if (splitCall == null)
            splitCall = SplitNode;
        root = new BSPNode(area, this);
    }

    public void BlockToGrid(BSPNode node)
    {
        print("-------------");
        print("BlockToGrid");
        print("area");
        print(node.area);
        print("block");
        print(node.block);
        BlockToGrid(node.block);
    }

    public void BlockToGrid(Rect block)
    {
        int i, j, w, h;
        w = (int)(block.width + block.x);
        h = (int)(block.height + block.y);
        //print("i:" + (int)block.y + " j:" + (int)block.x + " w:" + w + " h:" + h);
        for (i = (int)block.y; i < h - 1; i++)
        {
            for (j = (int)block.x; j < w - 1; j++)
            {
                grid[i, j] = 1;
            }
        }
    }

    public void Build()
    {
        Init();
        root.Split(splitCall);
        //foreach (BSPNode node in leaves)
        //    node.CreateBlock();
    }

    private void Awake()
    {
        tree = new Dictionary<int, List<BSPNode>>();
        leaves = new HashSet<BSPNode>();
    }

    public Rect[] SplitNode(Rect area)
    {
        int val = (int)Mathf.Min(area.width, area.height);
        //print("minAcceptsize " + minAcceptSize);
        //print("val " + val);
        if (val < minAcceptSize)
            return null;

        //print("SplittingNode");
        //print("area");
        //print(area);
        //print("----");
        Rect[] areas = new Rect[2];
        bool isHeightMax = area.height >= area.width;
        float half, bottom, top;
        //float divider = Random.Range(0.3f, 0.7f);
        float divider = 0.5f;
        if (isHeightMax)
        {
            print("split by height (horizontal)");
            half = Mathf.FloorToInt(area.height * divider);
            bottom = half;
            top = area.height - half;

            areas[0].x = area.x;
            areas[0].y = area.y;
            areas[0].height = bottom;
            areas[0].width = area.width;

            areas[1].x = area.x;
            areas[1].y = half;
            areas[1].height = top;
            areas[1].width = area.width;
        }
        else
        {
            print("split by width (vertical)");
            half = Mathf.FloorToInt(area.width * divider);
            bottom = half;
            top = area.width - half;
            areas[0].x = area.x;
            areas[0].y = area.y;
            areas[0].height = area.height;
            areas[0].width = bottom;

            areas[1].x = half;
            areas[1].y = area.y;
            areas[1].height = area.height;
            areas[1].width = top;
        }
        print("area 0: " + areas[0].ToString());
        print("area 1: " + areas[1].ToString());
        return areas;
    }



    private void Start()
    {
        
    }
}
