using UnityEngine;
using System.Collections.Generic;

public class BSPNode
{
    public Dungeon root;
    public Rect area;
    public Rect block;
    public BSPNode left;
    public BSPNode right;
    //public int depth;

    public BSPNode(Rect area, Dungeon root, int depth = 0)
    {
        this.area = area;
        this.root = root;
        //this.depth = depth;
        //this.root.leaves.Add(this);
        //if (!this.root.tree.ContainsKey(depth))
        //    this.root.tree.Add(depth, new List<BSPNode>());
        //this.root.tree[depth].Add(this);
    }

    public void Split(Dungeon.Split splitCall)
    {
        Rect[] areas = splitCall(area);
        if (areas == null)
        {
            Debug.Log("areas are NULL | Creating block...");
            block = CreateBlock(area);
            Debug.Log(block);
            root.BlockToGrid(block);
            return;
        }
        //root.leaves.Remove(this);
        //Debug.Log("area 0: " + areas[0].ToString());
        //Debug.Log("area 1: " + areas[1].ToString());
        Debug.Log("Creating new nodes");
        left = new BSPNode(areas[0], root);
        right = new BSPNode(areas[1], root);
        left.Split(splitCall);
        right.Split(splitCall);
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

}
