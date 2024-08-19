using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntUtility
{
    public static bool GetBit(this int value, int index)
    {
        return (value & (1 << index)) != 0;
    }
    public static int[] GetTrueBitIndexArray(this int value) 
    {
        List<int> result = new List<int>();
        for (int i = 0; i < 32; i++)
        {
            if (value.GetBit(i)) result.Add(i);
        }
        return result.ToArray();
    }
}
