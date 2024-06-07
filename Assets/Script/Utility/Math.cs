using UnityEngine;

public static class Math
{
    public static int FloorRotateToInt(float rotate, float offset, int count)
    {
        rotate += offset;
        int intRotate = Mathf.FloorToInt((rotate / 360) * count);
        return intRotate % count;
    }
    public static int Rotate4X(int prevRotate, float rotate)
    {
        // 1 1 0
        // 2 X 3
        // 2 3 3
        if (rotate == 0) return 3;
        if (rotate == 180) return 2;
        if (rotate == 90)
        {
            if (prevRotate == 0 || prevRotate == 3) return 0;
            else return 1;
        }
        if (rotate == 270)
        {
            if (prevRotate == 0 || prevRotate == 3) return 3;
            else return 2;
        }
        if (rotate < 90) return 0;
        if (rotate < 180) return 1;
        if (rotate < 270) return 2;
        return 3;
    }
    /// <summary>
    /// {vector}를 각도로 변환
    /// </summary>
    /// <param name="vector"></param>
    /// <returns>0 <= return < 360</returns>
    public static float Vector2ToDegree(Vector2 vector)
    {
        float rotate = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        
        if (rotate < 0) return rotate + 360;
        return rotate;
    }
    public static Vector2 Vector2TransformToEllipse(Vector2 vector, float x, float y)
    {
        return new Vector2(vector.x * x, vector.y * y);
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static int Bigger(int a, int b)
    {
        if (a >= b) return a;
        else return b;
    }
    public static int Smaller(int a, int b)
    {
        if (a >= b) return b;
        else return a;
    }
}
