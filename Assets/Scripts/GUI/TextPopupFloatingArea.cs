#if UNITY_EDITOR
using System;
using UnityEngine;

public class TextPopupFloatingArea : FloatingArea
{
    string[] array;
    Rect[] rects;
    Action<int> selectEvent;

    public TextPopupFloatingArea(string[] array, Action<int> selectEvent)
    {
        this.array = array;
        this.selectEvent = selectEvent;
    }

    public override void EventListen(Event e)
    {
        if (EventUtility.MouseDown(0))
        {
            for (int i = 0; rects != null && i < rects.Length; i++)
            {
                if (rects[i].AddPos(manager.rect.position).Contains(e.mousePosition))
                {
                    selectEvent?.Invoke(i);
                    e.Use();
                    manager.area = null;
                }
            }
        }
    }
    public override void Draw()
    {
        GUILayout.BeginArea(manager.rect);
        {
            if (Event.current.type == EventType.Repaint)
            {
                rects = new Rect[array.Length];
            }
            for (int i = 0; i < array.Length; i++)
            {
                GUILayout.Label(array[i], GUI.skin.label);
                if (Event.current.type == EventType.Repaint)
                {
                    rects[i] = GUILayoutUtility.GetLastRect();
                }
            }
        }
        GUILayout.EndArea();
    }


    public override float GetHeight()
    {
        return GUI.skin.label.lineHeight * array.Length + 20;
    }
}
#endif