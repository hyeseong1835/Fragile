#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class TextPopupFloatingArea : FloatingArea
{
    string[] array;
    Func<int, bool> selectEvent;
    int mouseOnIndex = -1;
    float height;
    float topSpace, bottomSpace, rightSpace, leftSpace;

    public static GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);

    public TextPopupFloatingArea(
        string[] array,
        Func<int, bool> selectEvent,
        float height = 20,
        float topSpace = 5,
        float bottomSpace = 10,
        float rightSpace = 1,
        float leftSpace = 1
    )
    {
        this.array = array;
        this.selectEvent = selectEvent;
        this.height = height;

        this.topSpace = topSpace;
        this.bottomSpace = bottomSpace;
        this.rightSpace = rightSpace;
        this.leftSpace = leftSpace;
    }
    public override void OnCreated()
    {

    }
    public override void EventListen()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Debug.Log($"{mouseOnIndex}\n{manager.rect}\n{Event.current.mousePosition}");
            mouseOnIndex = Mathf.FloorToInt((Event.current.mousePosition.y - (manager.rect.y + topSpace)) / height);

            Debug.Log(mouseOnIndex);
            if (0 <= mouseOnIndex && mouseOnIndex < array.Length)
            {
                if(selectEvent.Invoke(mouseOnIndex))
                {
                    manager.Destroy();
                    Event.current.Use();
                    return;
                }
                Event.current.Use();
                return;
            }
        }
    }
    public override void Draw()
    {
        for (int i = 0; i < array.Length; i++)
        {
            GUI.Label(
                new Rect(
                    manager.rect.x + leftSpace,
                    manager.rect.y + height * i + topSpace,
                    manager.rect.width - (rightSpace + leftSpace),
                    height
                ),
                array[i],
                labelStyle
            );
        }
        
        GUI.Label(new Rect(Event.current.mousePosition, new Vector2(50, 20)), $"{mouseOnIndex}");
    }

    public override float GetHeight()
    {
        return height * array.Length + topSpace + bottomSpace;
    }
}
#endif