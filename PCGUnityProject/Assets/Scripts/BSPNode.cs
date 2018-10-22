using UnityEngine;
using System.Collections.Generic;

public enum BSPNodeType
{
    Branch, Leaf
}

public class BSPNode
{
    private BSPNodeType _type;
    private SplitType _split;
    public BSPNodeType Type
    {
        get { return _type; }
    }
    public SplitType SplitDirection
    {
        get { return _split; }
    }
    public Rect area;
    public Rect block;
    public Dungeon dungeon;
    public BSPNode left;
    public BSPNode right;

    public static int cutVal = 10;

    //public int depth;

    public BSPNode(Rect area, Dungeon dungeon)
    {
        this.area = area;
        this.dungeon = dungeon;
        this._type = BSPNodeType.Branch;
        this.block = Rect.zero;
        this.left = null;
        this.right = null;
        //Debug.Log("type: " + type);
        // this.type = Mathf.Clamp(type, 1, 3);
    }

    public void Split()
    {
        int min = (int)Mathf.Min(area.width, area.height);
        if (min <= cutVal)
        {
            _type = BSPNodeType.Leaf;
            block = CreateBlock(area);
            return;
        }
        _split = SplitType.Horizontal;
        if (area.width == area.height)
        {
            int randNum = Random.Range(0, 99);
            if (randNum % 2 == 0)
                _split = SplitType.Vertical;
        }
        else if (area.width > area.height)
            _split = SplitType.Vertical;

        Rect[] areas = new Rect[2];
        // bool isHeightMax = area.height >= area.width;
        float divider, cut;//, cutTop;
        divider = Random.Range(0.4f, 0.6f);
        divider = 0.5f;
        if (_split == SplitType.Horizontal)
        {
            cut = Mathf.RoundToInt(area.height * divider);
            areas[0].xMin = area.xMin;
            areas[0].yMin = area.yMin;
            areas[0].xMax = area.xMax;
            areas[0].yMax = cut;

            areas[1].xMin = area.xMin;
            areas[1].yMin = cut;
            areas[1].xMax = area.xMax;
            areas[1].yMax = area.yMax;
        }
        else
        {
            cut = Mathf.RoundToInt(area.width * divider);
            areas[0].xMin = area.xMin;
            areas[0].yMin = area.yMin;
            areas[0].yMax = area.yMax;
            areas[0].xMax = cut;

            areas[1].xMin = cut;
            areas[1].yMin = area.yMin;
            areas[1].xMax = area.xMax;
            areas[1].yMax = area.yMax;
        }

        left = new BSPNode(areas[0], dungeon);
        right = new BSPNode(areas[1], dungeon);
        left.Split();
        right.Split();
    }

    // public void Split(Dungeon.Split splitCall)
    // {
    //     //int val = (int)(Mathf.Max(area.width, area.height));
    //     //if (val < cutVal)
    //     Blob b = splitCall(area);
    //     if (b == null)
    //     {
    //         Debug.Log("No need to cut");
    //         block = CreateBlock(area);
    //         dungeon.BlockToGrid(this);
    //         //dungeon.BlockToGrid(block);
    //         return;
    //     }
    //     //Debug.Log("Cutting");
    //     //Blob b = splitCall(area);
    //     left = new BSPNode(b.areaLeft, dungeon);
    //     right = new BSPNode(b.areaRight, dungeon);
    //     left.Split(splitCall);
    //     right.Split(splitCall);
    //     //dungeon.BindBlocks(left.area, right.area, b.splitType);
    // }

    public static Rect CreateBlock(Rect area)
    {
        Rect block = new Rect
        {
            xMin = area.xMin + 1f,
            yMin = area.yMin + 1f,
            xMax = area.xMax - 1f,
            yMax = area.yMax - 1f
        };
        return block;
    }

}

public enum SplitType
{
    Horizontal, Vertical
}

[System.Serializable]
public class Blob
{
    public SplitType splitType;
    public Rect areaLeft;
    public Rect areaRight;
}
