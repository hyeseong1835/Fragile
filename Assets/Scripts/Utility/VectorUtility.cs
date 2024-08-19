using UnityEngine;

public static class VectorUtility
{
    public static Vector2 SetX(this Vector2 vector, float value) => new Vector2(value, vector.y);
    public static Vector2 SetY(this Vector2 vector, float value) => new Vector2(vector.x, value);

    public static Vector2 AddX(this Vector2 vector, float value) => vector.SetX(vector.x + value);
    public static Vector2 AddY(this Vector2 vector, float value) => vector.SetY(vector.y + value);

    public static Vector2 MultiplyX(this Vector2 vector, float value) => vector.SetX(vector.x * value);
    public static Vector2 MultiplyY(this Vector2 vector, float value) => vector.SetY(vector.y * value);
    
    public static Vector3 SetX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
    public static Vector3 SetY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);
    public static Vector3 SetZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);

    public static Vector3 AddX(this Vector3 vector, float value) => vector.SetX(vector.x + value);
    public static Vector3 AddY(this Vector3 vector, float value) => vector.SetY(vector.y + value);
    public static Vector3 AddZ(this Vector3 vector, float value) => vector.SetZ(vector.z + value);

    public static Vector3 MultiplyX(this Vector3 vector, float value) => vector.SetX(vector.x * value);
    public static Vector3 MultiplyY(this Vector3 vector, float value) => vector.SetY(vector.y * value);
    public static Vector3 MultiplyZ(this Vector3 vector, float value) => vector.SetZ(vector.z * value);

    public static Vector2 Rotate(this Vector2 vector, float delta)
    {
        float sin = Mathf.Sin(delta);
        float cos = Mathf.Cos(delta);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    public static Vector2 Bezier(Vector2 startPos, Vector2 endPos, Vector2 handlePos, float t)
    {
        Vector2 a = Vector2.Lerp(startPos, handlePos, t);
        Vector2 b = Vector2.Lerp(handlePos, endPos, t);

        return Vector2.Lerp(a, b, t);
    }
    public static Vector3 Bezier(Vector3 startPos, Vector3 endPos, Vector3 handlePos, float t)
    {
        Vector3 a = Vector3.Lerp(startPos, handlePos, t);
        Vector3 b = Vector3.Lerp(handlePos, endPos, t);

        return Vector3.Lerp(a, b, t);
    }
    public static Vector2 Abs(this Vector2 vector) => new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    public static Vector3 Abs(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
}
