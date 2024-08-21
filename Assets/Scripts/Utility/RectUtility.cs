using UnityEngine;

public enum Anchor
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}
public static class RectUtility
{
    public static Vector2 GetOffset(Anchor anchor)
    {
        switch (anchor)
        {
            case Anchor.TopLeft: return new Vector2(0, 0);
            case Anchor.TopCenter: return new Vector2(0.5f, 0);
            case Anchor.TopRight: return new Vector2(1, 0);
            case Anchor.MiddleLeft: return new Vector2(0, 0.5f);
            case Anchor.MiddleCenter: return new Vector2(0.5f, 0.5f);
            case Anchor.MiddleRight: return new Vector2(1, 0.5f);
            case Anchor.BottomLeft: return new Vector2(0, 1);
            case Anchor.BottomCenter: return new Vector2(0.5f, 1);
            case Anchor.BottomRight: return new Vector2(1, 1);
            default: Debug.LogError("Not implemented"); return default;
        }
    }
    public static Vector2 GetPivotPos(this Rect rect, Anchor pivot)
    {
        return rect.position + GetOffset(pivot) * rect.size;
    }

    #region Position

    public static Rect SetPos(this Rect rect, float x, float y) => SetPos(rect, new Vector2(x, y));
    public static Rect SetPos(this Rect rect, Vector2 position)
    {
        rect.position = position;
        return rect;
    }
    public static Rect SetX(this Rect rect, float x)
    {
        rect.x = x;
        return rect;
    }
    public static Rect SetY(this Rect rect, float y)
    {
        rect.y = y;
        return rect;
    }

    #endregion

    #region Size

    public static Rect SetSize(this Rect rect, float x, float y, Anchor anchor = Anchor.TopLeft) => SetSize(rect, new Vector2(x, y), anchor);
    public static Rect SetSize(this Rect rect, Vector2 size, Anchor anchor = Anchor.TopLeft)
    {
        rect.position = rect.position + (rect.size - size) * GetOffset(anchor);
        rect.size = size;

        return rect;
    }
    public static Rect SetWidth(this Rect rect, float width, float alignment = 0)
    {
        rect.position = rect.position.AddX((rect.width - width) * alignment);
        rect.width = width;

        return rect;
    }
    public static Rect SetHeight(this Rect rect, float height, float alignment = 0)
    {
        rect.position = rect.position.AddY((rect.height - height) * alignment);
        rect.height = height;

        return rect;
    }

    #endregion

    public static Rect AddPos(this Rect rect, float x, float y) => rect.AddPos(new Vector2(x, y));
    public static Rect AddPos(this Rect rect, Vector2 position)
    {
        rect.position += position;
        return rect;
    }

    public static Rect AddX(this Rect rect, Rect add) => rect.AddX(add.x);
    public static Rect AddX(this Rect rect, float x)
    {
        rect.x += x;
        return rect;
    }

    public static Rect AddY(this Rect rect, Rect add) => rect.AddY(add.y);
    public static Rect AddY(this Rect rect, float y)
    {
        rect.y += y;
        return rect;
    }

    public static Rect AddSize(this Rect rect, Rect add, Anchor anchor = Anchor.TopLeft) => rect.AddSize(add.size, anchor);
    public static Rect AddSize(this Rect rect, Vector2 size, Anchor anchor = Anchor.TopLeft)
    {
        return rect.AddSize(size).AddPos(GetOffset(anchor) * rect.size);
    }

    public static Rect AddWidth(this Rect rect, Rect add) => rect.AddWidth(add.width);
    public static Rect AddWidth(this Rect rect, float width, float alignment = 0)
    {
        rect.width += width;
        rect.position = rect.position.AddX(-width * alignment);
        return rect;
    }

    public static Rect AddHeight(this Rect rect, Rect add) => rect.AddHeight(add.height);
    public static Rect AddHeight(this Rect rect, float height)
    {
        rect.height += height;
        return rect;
    }

    public static Rect[] HorizontalSplit(this Rect rect, int count, int alignment = 0)
    {
        Rect[] result = new Rect[count];
        float width = rect.width / count;

        result[0] = rect.SetWidth(width, alignment);
        for(int i = 1; i < count; i++)
        {
            result[i] = result[i - 1].AddX(width);
        }

        return result;
    }
    public static Rect HorizontalSlice(this Rect rect, float right, float width)
    {
        return new Rect(
            rect.xMax - width,
            rect.y,
            width,
            rect.height
        );
    }
}
