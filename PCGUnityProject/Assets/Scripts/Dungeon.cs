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

    public delegate Blob Split(Rect area);
    public Split splitCall;

    public BSPNode root;

    public int[,] grid;


    public void Init()
    {
        BSPNode.cutVal = minAcceptSize;
        print("Dungeon.Init");
        area = new Rect();
        area.xMin = area.yMin = 0;
        area.xMax = maxWidth;
        area.yMax = maxHeight;
        grid = new int[maxHeight, maxWidth];
        if (splitCall == null)
            splitCall = SplitArea;
        root = new BSPNode(area, this);
    }


    public void BindBlocks(Rect a, Rect b, SplitType split)
    {
        //print("BindBlocks");
        int originX, originY, targetX, targetY;
        if (split == SplitType.Horizontal)
        {
            originX = (int)Random.Range(a.xMin + 1, a.xMax - 1);
            originY = (int)a.center.x;
            targetX = originX;
            targetY = (int)b.center.y;
        }
        else
        {
            originX = (int)a.center.x;
            originY = (int)Random.Range(a.yMin + 1, a.yMax - 1);
            targetX = (int)b.center.x;
            targetY = originY;
        }
        //print("origin: (" + originX + "," + originY + ")");
        //print("target: (" + targetX + "," + targetY + ")");

        int i, j;
        for (i = originY; i <= targetY; i++)
        {
            // print("i: " + i);
            for (j = originX; j <= targetX; j++)
            {
                // print("j: " + j);
                // print("(" + j + "," + i + ")");
                if (grid[i,j] == 0)
                    grid[i, j] = 1;
            }
        }

    }

    public void BlockToGrid(BSPNode node)
    {
        print("-------------");
        print("BlockToGrid");
        print("area");
        Rect area = node.area;
        Rect block = node.block;
        print(area);
        print("xMin:" + area.xMin + ", yMin:" + area.yMin + ", xMax:" + area.xMax + ", yMax:" + area.yMax);
        print("block");
        print(node.block);
        print("xMin:" + block.xMin + ", yMin:" + block.yMin + ", xMax:" + block.xMax + ", yMax:" + block.yMax);
        int i, j, x, y, w, h;
        x = (int)node.block.xMin;
        y = (int)node.block.yMin;
        w = (int)node.block.xMax;
        h = (int)node.block.yMax;
        //print("i:" + (int)block.y + " j:" + (int)block.x + " w:" + w + " h:" + h);
        for (i = y; i < h; i++)
        {
            for (j = x; j < w; j++)
            {
                grid[i, j] = 1;
                //print("x:" + j + ", y:" + i + " - " + node.type);
            }
        }
    }


    public void Build()
    {
        Init();
        root.Split();
        Raster(root);
    }

    private void Raster(BSPNode node)
    {
        if (node == null)
            return;
        if (node.Type == BSPNodeType.Leaf)
        {
            BlockToGrid(node);
            return;
        }
        Raster(node.left);
        Raster(node.right);
    }

    private void Awake()
    {
        tree = new Dictionary<int, List<BSPNode>>();
        leaves = new HashSet<BSPNode>();
    }

    public Blob SplitArea(Rect area)
    {
        //print("SplitArea");
        int val = (int)Mathf.Max(area.width, area.height);
        // print("area: " + area);
        // Debug.Log("val: " + val);
        // Debug.Log("min: " + minAcceptSize);
        if (val < minAcceptSize)
        {
            // print("stop splitting");
            return null;
        }
        // print("start splitting");
        //print("SplittingNode");
        //print("area");
        //print(area);
        //print("----");
        Rect[] areas = new Rect[2];
        bool isHeightMax = area.height >= area.width;
        float divider, cut;//, cutTop;
        divider = Random.Range(0.4f, 0.6f);
        divider = 0.5f;
        Blob blob = new Blob();
        if (isHeightMax)
        {
            blob.splitType = SplitType.Horizontal;
            cut = Mathf.RoundToInt(area.height * divider);
            //cutTop = Mathf.CeilToInt(area.height * divider);

            areas[0].x = area.x;
            areas[0].y = area.y;
            areas[0].height = cut;
            areas[0].width = area.width;

            areas[1].x = area.x;
            areas[1].y = cut+1;
            areas[1].height = area.height - cut;
            areas[1].width = area.width;
        }
        else
        {
            blob.splitType = SplitType.Vertical;
            cut = Mathf.FloorToInt(area.width * divider);
            //cutTop = Mathf.CeilToInt(area.width * divider);

            areas[0].x = area.x;
            areas[0].y = area.y;
            areas[0].height = area.height;
            areas[0].width = cut;

            areas[1].x = cut=1;
            areas[1].y = area.y;
            areas[1].height = area.height;
            areas[1].width = area.width - cut;
        }
        print("area 0: " + areas[0].ToString());
        print("area 1: " + areas[1].ToString());
        blob.areaLeft = areas[0];
        blob.areaRight = areas[1];
        return blob;
    }


}
