using UnityEngine;

public static class CustomGizmos
{
    public static void DrawVector(Vector2 start, Vector2 vector, float radian, float length) => DrawArrow(start, start + vector, radian, length);
    public static void DrawArrow(Vector2 start, Vector2 end, float radian, float length)
    {
        Gizmos.DrawLine(start, end);

        Vector2 reverseDir = (start - end).normalized;

        Vector2 a = end + reverseDir.Rotate(radian) * length;
        Vector2 b = end + reverseDir.Rotate(-radian) * length;

        Gizmos.DrawLine(end, a);
        Gizmos.DrawLine(end, b);
    }
}