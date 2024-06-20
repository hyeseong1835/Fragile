using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtility
{
    public static void MoveElement<ElementT>(this List<ElementT> list, ElementT target, int index)
    {
        list.Insert(index, target);
        list.Remove(target);
    }
    public static ElementT[] ToArray<ElementT>(this List<ElementT> list, int length)
    {
        ElementT[] array = new ElementT[length];

        IEnumerator listIt = list.GetEnumerator();
        for (int i = 0; i < length; i++) 
        {
            listIt.MoveNext();
            array[i] = (ElementT)listIt.Current;
        }

        return array;
    }
}
