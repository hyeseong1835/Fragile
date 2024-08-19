#if UNITY_EDITOR
using UnityEngine;

public class FloatingAreaManager
{
    public FloatingArea area;
    public Vector2 position;
    public float width;
    public Rect rect;
    bool created;

    public void SetRect(Vector2 position, float width)
    {
        this.position = position;
        this.width = width;
    }
    public void SetRect(Rect headerRect)
    {
        SetRect(headerRect.position.AddY(headerRect.height), headerRect.width);
    }
    public void EventListen(Event e)
    {
        if (area != null)
        {
            area.EventListen(e);

            if (e.isMouse && rect.Contains(e.mousePosition))
            {
                e.Use();
            }
        }
    }
    public void Create(FloatingArea area)
    {
        area.manager = this;
        this.area = area;

        created = true;
    }
    public void Draw()
    {
        if (area != null)
        {
            rect = new Rect(position, new Vector2(width, area.GetHeight()));

            if (created)
            {
                area.OnCreated();
                created = false;
            }
            area.CreateField();
            area.Draw();
        }
    }
    public void Destroy()
    {
        area = null;
    }
}

public abstract class FloatingArea
{
    public FloatingAreaManager manager;
    public SquareColor backGroundColor = new SquareColor(Color.black, Color.white);
    
    public abstract float GetHeight();

    public abstract void EventListen(Event e);

    public abstract void Draw();

    public virtual void CreateField()
    {
        CustomGUI.DrawSquare(manager.rect, backGroundColor);
    }
    public virtual void OnCreated()
    {

    }
}
#endif