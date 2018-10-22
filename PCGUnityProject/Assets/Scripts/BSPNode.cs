using UnityEngine;
using System.Collections.Generic;

public class BSPNode
{
    public Dungeon dungeon;
    public Rect area;
    public Rect block;
    public BSPNode left;
    public BSPNode right;
    public int type;

    public static int cutVal = 10;

    //public int depth;

    public BSPNode(Rect area, Dungeon dungeon, int type = 1)
    {
        this.area = area;
        this.dungeon = dungeon;
        //Debug.Log("type: " + type);
        this.type = Mathf.Clamp(type, 1, 3);
    }

    public void Split(Dungeon.Split splitCall)
    {
        //int val = (int)(Mathf.Max(area.width, area.height));
        //if (val < cutVal)
        Blob b = splitCall(area);
        if (b == null)
        {
            Debug.Log("No need to cut");
            block = CreateBlock(area);
            dungeon.BlockToGrid(this);
            //dungeon.BlockToGrid(block);
            return;
        }
        //Debug.Log("Cutting");
        //Blob b = splitCall(area);
        left = new BSPNode(b.areaLeft, dungeon);
        right = new BSPNode(b.areaRight, dungeon);
        left.Split(splitCall);
        right.Split(splitCall);
        //dungeon.BindBlocks(left.area, right.area, b.splitType);
    }

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
