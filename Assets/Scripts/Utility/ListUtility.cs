using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListUtility
{
    public static ObjT[][] ToDoubleArray<ObjT>(this List<List<ObjT>> doubleList)
    {
        List<ObjT[]> result = new List<ObjT[]>();

        for (int i = 0; i < doubleList.Count; i++)
        {
            result.Add(doubleList[i].ToArray());
        }
        return result.ToArray();
    }
    public static ObjT[] ToSingleArray<ObjT>(this List<List<ObjT>> doubleList)
    {
        List<ObjT> result = new List<ObjT>();

        foreach(List<ObjT> list in doubleList)
        {
            result.AddRange(list);
        }
        return result.ToArray();
    }
    public static int[] GetCounts<ObjT>(this List<List<ObjT>> doubleList)
    {
        List<int> result = new List<int>();

        foreach (List<ObjT> list in doubleList)
        {
            result.Add(list.Count);
        }
        return result.ToArray();
    }
}
