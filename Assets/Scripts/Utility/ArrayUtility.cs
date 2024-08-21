using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public static class ArrayUtility
{
    public static void RemoveAt<TObj>(this TObj[] array, int index)
    {
        TObj[] newArray = new TObj[array.Length - 1];
        int i;
        for (i = 0; i < index; i++)
        {
            newArray[i] = array[i];
        }
        for (i = index; i < array.Length - 1; i++)
        {
            newArray[i] = array[i + 1];
        }
    }
    public static int IndexOf<ObjT>(this ObjT[] array, ObjT obj, int startIndex = 0)
    {
        for (int i = startIndex; i < array.Length; i++)
        {
            if (array[i].Equals(obj)) return i;
        }
        return -1;
    }
    public static int LastIndexOf<ObjT>(this ObjT[] array, ObjT obj, int startIndex = 0)
    {
        for (int i = startIndex; i >= 0; i--)
        {
            if (array[i].Equals(obj)) return i;
        }
        return -1;
    }

    #region Double Array

    public static bool TryTransformSingleToDoubleArrayIndex<ObjT>(this ObjT[][] doubleArray, int index, out int i1, out int i2)
    {
        for (i1 = 0; i1 < doubleArray.Length; i1++)
        {
            ObjT[] array = doubleArray[i1];
            int arrayLength = array.Length;

            if (index < arrayLength)
            {
                index -= arrayLength;
                continue;
            }
            i2 = index;
        }
        i1 = -1;
        i2 = -1;
        return false;
    }
    public static bool TryGetIndex<ObjT>(this ObjT[][] doubleArray, ObjT obj, out int i1, out int i2)
    {
        for (int arrayIndex = 0; arrayIndex < doubleArray.Length; arrayIndex++)
        {
            ObjT[] array = doubleArray[arrayIndex];

            for (int elementIndex = 0; elementIndex < array.Length; elementIndex++)
            {
                if (array[elementIndex].Equals(obj))
                {
                    i1 = arrayIndex;
                    i2 = elementIndex;
                    return true;
                }
            }
        }
        i1 = -1;
        i2 = -1;
        return false;
    }
    public static ObjT GetElement<ObjT>(this ObjT[][] doubleArray, int i)
    {
        int index = 0;
        for (int arrayIndex = 0; arrayIndex < doubleArray.Length; arrayIndex++)
        {
            ObjT[] array = doubleArray[arrayIndex];
            
            if (index + array.Length < i)
            {
                index += array.Length;
                continue;
            }

            for (int elementIndex = 0; elementIndex < array.Length; elementIndex++)
            {
                if (index == i) return array[elementIndex];

                index++;
            }
        }
        return default;
    }
    public static ObjT[][] NewDoubleArray<ObjT>(object[][] mimicLength)
    {
        List<ObjT[]> result = new List<ObjT[]>();
        for(int i = 0; i < mimicLength.Length; i++)
        {
            result.Add(new ObjT[mimicLength[i].Length]);
        }
        return result.ToArray();
    }
    public static int GetSingleLength<ObjT>(this ObjT[][] doubleArray)
    {
        int result = 0;
        for(int i = 0; i < doubleArray.Length; i++)
        {
            result += doubleArray[i].Length;
        }
        return result;
    }
    public static ObjT[] ToSingleArray<ObjT>(this ObjT[][] doubleArray)
    {
        ObjT[] result = new ObjT[doubleArray.GetSingleLength()];
        int index = 0;
        for(int i = 0; i < doubleArray.Length; i++)
        {
            for(int j = 0; j < doubleArray[i].Length; j++)
            {
                result[index] = doubleArray[i][j];

                index++;
            }
        }
        return result;
    }
    public static int[] GetLengthArray<ObjT>(this ObjT[][] doubleArray)
    {
        int[] result = new int[doubleArray.Length];
        for(int i = 0; i < doubleArray.Length; i++)
        {
            result[i] = doubleArray[i].Length;
        }
        return result;
    }

    #endregion

}
