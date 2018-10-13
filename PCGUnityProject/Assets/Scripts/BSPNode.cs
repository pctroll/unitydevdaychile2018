using UnityEngine;
using System.Collections.Generic;

public class BSPNode
{
    public Dungeon dungeon;
    public Rect area;
    public Rect block;
    public BSPNode left;
    public BSPNode right;

    //public int depth;

    public BSPNode(Rect area, Dungeon root, int depth = 0)
    {
        this.area = area;
        this.dungeon = root;
    }

    public void Split(Dungeon.Split splitCall)
    {
        Blob b = splitCall(area);
        if (b == null)
        {
            // Debug.Log("areas are NULL | Creating block...");
            block = CreateBlock(area);
            dungeon.BlockToGrid(block);
            return;
        }
        //Debug.Log("area 0: " + areas[0].ToString());
        //Debug.Log("area 1: " + areas[1].ToString());
        // Debug.Log("Creating new nodes");
        left = new BSPNode(b.areaLeft, dungeon);
        right = new BSPNode(b.areaRight, dungeon);
        left.Split(splitCall);
        right.Split(splitCall);
        dungeon.BindBlocks(left.area, right.area, b.splitType);
    }

    public static Rect CreateBlock(Rect area)
    {
        Rect block = new Rect();
        block.x = area.x + 1;
        block.y = area.y + 1;
        block.width = area.width - 1;
        block.height = area.height - 1;
        return block;
    }

    // public static Blob SplitNode(BSPNode node)
    // {
    //     Rect area = node.area;
    //     int val = (int)Mathf.Min(area.width, area.height);
    //     if (val < minAccecptedSize)
    //         return null;
    //     Rect[] areas = new Rect[2];
    //     bool isHeightMax = area.height > area.width;
    //     float cut, bottom, top;

    //     return blob;
    // }

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
