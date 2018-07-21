using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 存储索引
/// </summary>
public class BlendShape
{
    public int postiveIndex { get; set; }
    public int negativeIndex { get; set; }

    public BlendShape(int postiveIndex, int negativeIndex)
    {
        this.postiveIndex = postiveIndex;
        this.negativeIndex = negativeIndex;
    }
}
