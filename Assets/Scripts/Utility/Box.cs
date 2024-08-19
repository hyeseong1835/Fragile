using System;
using UnityEngine;

[Serializable]
public struct Box
{
    public Vector2 size;
    public Vector2 center;
    public float sizeX { get => size.x; set { size.x = value; } }
    public float sizeY { get => size.y; set { size.y = value; } }

    public float centerX { get => center.x; set { center.x = value; } }
    public float centerY { get => center.y; set { center.y = value; } }

    public float Left => center.x - 0.5f * size.x;
    public float Right => center.x + 0.5f * size.x;
    public float Top => center.y + 0.5f * size.y;
    public float Bottom => center.y - 0.5f * size.y;

    public Vector2 RightTop => new Vector2(Right, Top);
    public Vector2 LeftTop => new Vector2(Left, Top);
    public Vector2 RightBottom => new Vector2(Right, Bottom);
    public Vector2 LeftBottom => new Vector2(Left, Bottom);

    public Box(Vector2 size, Vector2 center)
    {
        this.size = size;
        this.center = center;
    }
    public Box(Collider2D coll)
    {
        size = coll.bounds.size;
        center = coll.offset;
    }
    
    #region Contact

    /// <param name="y">box의 Y 좌표 </param>
    /// <returns>Box가 line에 접하거나 그 위일 때 true를 반환합니다.</returns>
    public bool IsContactTop(float y, float line) => line < y + center.y + 0.5f * size.y;

    /// <param name="y">box의 Y 좌표 </param>
    /// <returns>Box가 line에 접하거나 그 아래일 때 true를 반환합니다.</returns>
    public bool IsContactBottom(float y, float line) => line > y + center.y - 0.5f * size.y;

    /// <param name="x">box의 X 좌표</param>
    /// <returns>Box가 line에 접하거나 그보다 오른쪽일 때 true를 반환합니다.</returns>
    public bool IsContactRight(float x, float line) => line < x + center.x + 0.5f * size.x;

    /// <param name="x">box의 X 좌표</param>
    /// <returns>Box가 line에 접하거나 그보다 왼쪽일 때 true를 반환합니다.</returns>
    public bool IsContactLeft(float x, float line) => line > x + center.x - 0.5f * size.x;

    /// <param name="pos">box의 WorldPosition</param>
    /// <param name="up">위 기준선</param>
    /// <param name="bottom">아래 기준선</param>
    /// <param name="right">우측 기준선</param>
    /// <param name="left">좌측 기준선</param>
    /// <returns>Box가 접하거나 그 밖일 때 true를 반환합니다.</returns>
    public bool IsContact(Vector2 pos, float up, float bottom, float right, float left)
    {
        return IsContactTop(pos.y, up)
            || IsContactBottom(pos.y, bottom)
            || IsContactRight(pos.x, right)
            || IsContactLeft(pos.x, left);
    }

    #endregion

    #region Contact + Out

    /// <param name="y">box의 Y 좌표 </param>
    /// <param name="line">위 기준선</param>
    /// <returns>Box가 line에 접하거나 그 위일 때 true를 반환합니다.</returns>
    public bool IsContactTop(float y, float line, out float contact)
    {
        contact = line - center.y - 0.5f * size.y;
        if (y > contact)
        {
            return true;
        }
        else
        {
            contact = y;
            return false;
        }
    }

    /// <param name="y">box의 Y 좌표</param>
    /// <param name="line">아래 기준선</param>
    /// <returns>Box가 line에 접하거나 그 아래일 때 true를 반환합니다.</returns>
    public bool IsContactBottom(float y, float line, out float contact)
    {
        contact = line - center.y + 0.5f * size.y;
        if (y < contact)
        {
            return true;
        }
        else
        {
            contact = y;
            return false;
        }
    }

    /// <param name="x">box의 X 좌표</param>
    /// <param name="line">우측 기준선</param>
    /// <returns>Box가 line에 접하거나 그보다 오른쪽일 때 true를 반환합니다.</returns>
    public bool IsContactRight(float x, float line, out float contact)
    {
        contact = line - center.x - 0.5f * size.x;
        if (x > contact)
        {
            return true;
        }
        else
        {
            contact = x;
            return false;
        }
    }

    /// <param name="x">box의 X 좌표</param>
    /// <param name="line">좌측 기준선</param>
    /// <returns>Box가 line에 접하거나 그보다 왼쪽일 때 true를 반환합니다.</returns>
    public bool IsContactLeft(float x, float line, out float contact)
    {
        contact = line - center.x + 0.5f * size.x;
        if (x < contact)
        {
            return true;
        }
        else
        {
            contact = x;
            return false;
        }
    }

    /// <param name="pos">box의 WorldPosition</param>
    /// <param name="top">위 기준선</param>
    /// <param name="bottom">아래 기준선</param>
    /// <param name="right">우측 기준선</param>
    /// <param name="left">좌측 기준선</param>
    /// <param name="contact"></param>
    /// <returns>Box가 기준선에 접하거나 그 밖일 때 true를 반환합니다. // false일 때 contact = pos</returns>
    public bool IsContact(Vector2 pos, float top, float bottom, float right, float left, out Vector2 contact)
    {
        return (
            (
                IsContactTop(pos.y, top, out contact.y)
                || IsContactBottom(pos.y, bottom, out contact.y)
            )
            | (
                IsContactRight(pos.x, right, out contact.x)
                || IsContactLeft(pos.x, left, out contact.x)
            )
        );
    }
    /// <param name="pos">box의 WorldPosition</param>
    /// <param name="top">위 기준선</param>
    /// <param name="bottom">아래 기준선</param>
    /// <param name="right">우측 기준선</param>
    /// <param name="left">좌측 기준선</param>
    /// <param name="contact"></param>
    /// <returns>Box가 기준선에 접하거나 그 밖일 때 true를 반환합니다. // false일 때 contact = pos</returns>
    public bool IsContact(Vector2 pos, float top, float bottom, float right, float left, out Vector2 contact, out int horizontal, out int vertical)
    {
        bool result = false;
        horizontal = 0;
        vertical = 0;

        if (IsContactTop(pos.y, top, out contact.y))
        {
            result = true;
            vertical = 1;
        }
        else if (IsContactBottom(pos.y, bottom, out contact.y))
        {
            result = true;
            vertical = -1;
        }

        if (IsContactRight(pos.x, right, out contact.x))
        {
            result = true;
            horizontal = 1;
        }
        else if (IsContactLeft(pos.x, left, out contact.x))
        {
            result = true;
            horizontal = -1;
        }
        return result;
    }
    #endregion

    #region Exit

    /// <returns>Box가 line보다 위일 때 true를 반환합니다.</returns>
    public bool IsExitTop(float y, float line) => line < y + center.y - 0.5f * size.y;
    
    /// <returns>Box가 line보다 아래일 때 true를 반환합니다.</returns>
    public bool IsExitBottom(float y, float line) => line > y + center.y + 0.5f * size.y;
    
    /// <returns>Box가 line보다 오른쪽일 때 true를 반환합니다.</returns>
    public bool IsExitRight(float x, float line) => line < x + center.x - 0.5f * size.x;

    /// <returns>Box가 line보다 왼쪽일 때 true를 반환합니다.</returns>
    public bool IsExitLeft(float x, float line) => line > x + center.x + 0.5f * size.x;

    /// <param name="pos">box의 WorldPosition</param>
    /// <param name="top">위 기준선</param>
    /// <param name="bottom">아래 기준선</param>
    /// <param name="right">우측 기준선</param>
    /// <param name="left">좌측 기준선</param>
    /// <returns>Box가 기준선 밖에 있을 때 true를 반환합니다.</returns>
    public bool IsExit(Vector2 pos, float top, float bottom, float right, float left)
    {
        return IsExitTop(pos.y, top) 
            || IsExitBottom(pos.y, bottom) 
            || IsExitRight(pos.x, right) 
            || IsExitLeft(pos.x, left);
    }

    #endregion

    #region Exit + Out

    /// <returns>Box가 line보다 위일 때 true를 반환합니다.</returns>
    public bool IsExitUp(float y, float line, out float contact)
    {
        contact = line - center.y + 0.5f * size.y;
        if (y >= contact)
        {
            return true;
        }
        else
        {
            contact = y;
            return false;
        }
    }
    /// <returns>Box가 line보다 아래일 때 true를 반환합니다.</returns>
    public bool IsExitDown(float y, float line, out float contact)
    {
        contact = line - center.y - 0.5f * size.y;
        if (y <= contact)
        {
            return true;
        }
        else
        {
            contact = y;
            return false;
        }
    }
    /// <returns>Box가 line보다 오른쪽일 때 true를 반환합니다.</returns>
    public bool IsExitRight(float x, float line, out float contact)
    {
        contact = line - center.x + 0.5f * size.x;
        if (x >= contact)
        {
            return true;
        }
        else
        {
            contact = x;
            return false;
        }
    }

    /// <returns>Box가 line보다 왼쪽일 때 true를 반환합니다.</returns>
    public bool IsExitLeft(float x, float line, out float contact)
    {
        contact = line - center.x - 0.5f * size.x;
        if (x <= contact)
        {
            return true;
        }
        else
        {
            contact = x;
            return false;
        }
    }

    /// <param name="pos">box의 WorldPosition</param>
    /// <param name="top">위 기준선</param>
    /// <param name="bottom">아래 기준선</param>
    /// <param name="right">우측 기준선</param>
    /// <param name="left">좌측 기준선</param>
    /// <param name="contact">접한 위치</param>
    /// <returns>Box가 접하거나 그 밖일 때 true를 반환합니다. // false일 때 contact = pos</returns>
    public bool IsExit(Vector2 pos, float top, float bottom, float right, float left, out Vector2 contact)
    {
        if (IsExitUp(pos.y, top, out float contactUp))
        {
            contact = new Vector2(pos.x, contactUp);
            return true;
        }
        if (IsExitDown(pos.y, bottom, out float contactDown))
        {
            contact = new Vector2(pos.x, contactDown);
            return true;
        }
        if (IsExitRight(pos.x, right, out float contactRight))
        {
            contact = new Vector2(contactRight, pos.y);
            return true;
        }
        if (IsExitLeft(pos.x, left, out float contactLeft))
        {
            contact = new Vector2(contactLeft, pos.y);
            return true;
        }
        contact = pos;
        return false;
    }

    #endregion
}