using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static readonly int layerMaskBlock = (1 << LayerMask.NameToLayer("Block"));

    public static bool BlockInPosition(Vector2 position, int size)
    {
        return Physics2D.OverlapBox(position, new Vector2(size - 0.05f, size - 0.05f), 0, Utils.layerMaskBlock);
    }



}
